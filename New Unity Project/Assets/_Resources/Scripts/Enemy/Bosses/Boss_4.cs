using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_4 : MonoBehaviour
{
    Transform _t;

    Animator _a;
    AIMethods AI;
    GameObject player;

    int points;

    float delay, delta;
    GridOfNodes gon;

    int actionType;
    float actionTimer;

    public Vector2 dir;

    List<Vector2> path = new List<Vector2>();

    void Start()
    {
        _t = transform;
        AI = GetComponent<AIMethods>();
        _a = GetComponent<Animator>();
        player = GameObject.Find("Player");
        if (gon == null)
            gon = FindObjectOfType<GridOfNodes>();
    }

    [SerializeField] float spetialActionTime;
    bool makingAction;

    IEnumerator action_0()
    {
        makingAction = true;

        _a.SetTrigger("Event_0");

        GameObject traps = GameObject.Find("Traps");
        for (int i = 0; i < traps.transform.childCount; i++)
            traps.transform.GetChild(i).GetComponent<Animator>().SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        makingAction = false;
    }
    
    IEnumerator action_1()
    {
        makingAction = true;

        _a.SetTrigger("Event_1");

        yield return new WaitForSeconds(1f);

        makingAction = false;

        Transform swills = GameObject.Find("Swills").transform;

        for (int i = 0; i < swills.childCount; i++)
            swills.GetChild(i).GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(4);

        for (int i = 0; i < swills.childCount; i++)
            swills.GetChild(i).GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator action_2()
    {
        makingAction = true;

        _a.SetBool("Event_2", true);

        float _t = 4;
        
        while (_t > 0)
        {
            _t -= Time.deltaTime;

            delay += Time.deltaTime;

            if (delay >= Random.Range(0, 0.3f))//Random.Range(0.8f, 1.2f))
            {
                path = AI.setDestination(transform.position, player.transform.position);
                points = path.Count - 1;
                delay = 0;
            }

            if (AI.netting) yield return null;

            if (path != null)
                if (path.Count != 0 && !isAttacking)
                {
                    transform.position = Vector2.MoveTowards(transform.position, path[points], AI.ms * Time.deltaTime);

                    if (Vector2.Distance(transform.position, path[points]) < 0.1f && points != 0)
                    {
                        points--;

                        if (points <= 0) yield return null;
                    }
                }

            yield return null;
        }

        _a.SetBool("Event_2", false);
        stopDamage = true;
        makingAction = false;
    }

    PlayerHP _php;
    bool stopDamage;
    IEnumerator damage()
    {
        if (_php == null) _php = FindObjectOfType<PlayerHP>();

        while (!stopDamage && _a.GetBool("Event_2"))
        {
            _php.toDamage(1);
            yield return new WaitForSeconds(0.3f);
        }

        stopDamage = false;
    }

    [SerializeField] GameObject mag_arrow, arrow;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _a.GetBool("Event_2"))
        {
            StartCoroutine(damage());
        }
        else if (_a.GetBool("Event_2") && collision.GetComponent<ArrowFly>() != null && Vector2.SqrMagnitude(collision.transform.position - transform.position) < 10)
        {
            GameObject enemy_arrow = Instantiate(collision.GetComponent<ArrowFly>().isMagArrow() ? mag_arrow : arrow);

            enemy_arrow.transform.position = collision.transform.position;

            enemy_arrow.GetComponent<EnemyArrowFly>().target = (collision.transform.position - transform.position).normalized;
            enemy_arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((collision.transform.position - transform.position).normalized.y,
                (collision.transform.position - transform.position).normalized.x) * 180 / Mathf.PI);

            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _a.GetBool("Event_2"))
        {
            stopDamage = true;
        }
    }

    void Update()
    {
        if (AI.stanned) return;

        if (actionTimer > spetialActionTime)
        {
            actionTimer = 0;

            int action = Random.Range(1, 3);
            if (action == 0)
                StartCoroutine(action_0());
            else if (action == 1)
                StartCoroutine(action_1());
            else
                StartCoroutine(action_2());
        }

        if (!makingAction)
            if (actionType == 0)
            {
                actionTimer += Time.deltaTime;

                delay += Time.deltaTime;
                float _d = Vector2.SqrMagnitude(_t.position - player.transform.position);
                GetComponent<CapsuleCollider2D>().enabled = _d < 20;

                if (_d < 4.5f && !isAttacking)
                {
                    StartCoroutine(attacking());
                    return;
                }

                if (delay >= Random.Range(0, 0.3f))//Random.Range(0.8f, 1.2f))
                {
                    path = AI.setDestination(transform.position, player.transform.position);
                    points = path.Count - 1;
                    delay = 0;
                }

                if (AI.netting) return;

                if (path != null)
                    if (path.Count != 0 && !isAttacking)
                    {
                        _t.position = Vector2.MoveTowards(_t.position, path[points], AI.ms * Time.deltaTime);

                        if (Vector2.Distance(_t.position, path[points]) < 0.1f && points != 0)
                        {
                            points--;

                            if (points <= 0) return;

                            Vector2 dirToPlayer = path[points - 1] - (Vector2)_t.position;
                            float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * 180 / Mathf.PI - 90;

                            if (angle <= 41 && angle > -57) AI.setDirTo(ref dir, new Vector2(0, 1));
                            else
                            if (angle <= -57 && angle > -126) AI.setDirTo(ref dir, new Vector2(1, 0));
                            else
                            if (angle <= -126 && angle > -230) AI.setDirTo(ref dir, new Vector2(0, -1));
                            else
                            if (angle <= -230 || angle > 41) AI.setDirTo(ref dir, new Vector2(-1, 0));

                            _a.SetFloat("MoveX", dir.x);
                            _a.SetFloat("MoveY", dir.y);
                            _a.SetBool("Moving", true);
                        }
                    }
            }
    }

    bool isAttacking;
    IEnumerator attacking()
    {
        Vector2 dirToPlayer = player.transform.position - _t.position;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * 180 / Mathf.PI - 90;

        if (angle <= 41 && angle > -57) AI.setDirTo(ref dir, new Vector2(0, 1));
        else
        if (angle <= -57 && angle > -126) AI.setDirTo(ref dir, new Vector2(1, 0));
        else
        if (angle <= -126 && angle > -230) AI.setDirTo(ref dir, new Vector2(0, -1));
        else
        if (angle <= -230 || angle > 41) AI.setDirTo(ref dir, new Vector2(-1, 0));

        isAttacking = true;
        _a.SetBool("Moving", false);
        _a.SetTrigger("Attack");
        yield return new WaitForSeconds(_a.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }
}