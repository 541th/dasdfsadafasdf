using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1 : MonoBehaviour
{
    Transform _t;
    public LayerMask mask;

    Animator _a;
    AIMethods AI;
    GameObject player;

    int points;

    float delay, delta;
    GridOfNodes gon;

    int actionType;
    float actionTimer;
    //0 - usuall attack 
    //1 - spetial attack 1
    //2 - spetial attack 2

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

    [SerializeField] GameObject object_0;
    IEnumerator action_0()
    {
        makingAction = true;

        _a.SetTrigger("Attack_0");
        yield return new WaitForSeconds(0.2f);

        float animTimer = _a.GetCurrentAnimatorStateInfo(0).length;
        float timer = 0;
        while (animTimer > 0)
        {
            timer += Time.deltaTime;
            animTimer -= Time.deltaTime;

            if (timer >= 0.05f)
            {
                GameObject _arrow = Instantiate(object_0);
                _arrow.transform.position = transform.position;
                Vector3 target = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                _arrow.GetComponent<EnemyArrowFly>().target = target.normalized;
                _arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((target).normalized.y,
                    (target).normalized.x) * 180 / Mathf.PI + 90);

                timer = 0;
            }

            yield return null;
        }

        makingAction = false;
    }

    [SerializeField] GameObject particle_0;
    IEnumerator action_1()
    {
        makingAction = true;

        _a.SetTrigger("Attack_1");
        float flyTimer = 0;

        GetComponent<CapsuleCollider2D>().enabled = false;

        GameObject shadow = transform.GetChild(2).gameObject;
        float shadowStartY = shadow.transform.position.y;

        StartCoroutine(GetComponentInChildren<EnemyHP>().hitBlink());

        yield return new WaitForSeconds(0.6f);

        GetComponent<SortingOrder>().dontChangeSortingOrger = true;

        while (flyTimer < 1.8f)
        {
            flyTimer += Time.deltaTime;

            shadow.transform.position = new Vector3(_t.position.x, shadowStartY);
            float flyValue = Mathf.Sin(flyTimer + 90.2f) / 8;
            transform.position = transform.position + new Vector3(0, flyValue * 40) * Time.deltaTime;
            
            if (flyValue > 0)
                shadow.transform.localScale -= new Vector3(Time.deltaTime * 10, Time.deltaTime * 3);
            else
                shadow.transform.localScale += new Vector3(Time.deltaTime * 10, Time.deltaTime * 3);

            yield return null;
        }

        shadow.transform.localScale = new Vector3(18, 6);
        shadow.transform.localPosition = new Vector3(0, -0.19f);

        GameObject particle = Instantiate(particle_0, shadow.transform.position, Quaternion.identity);
        Destroy(particle, 1);

        GetComponent<CapsuleCollider2D>().enabled = true;
        GetComponent<SortingOrder>().dontChangeSortingOrger = false;
        makingAction = false;
    }

    void Update()
    {
        if (AI.stanned) return;

        if (actionTimer > spetialActionTime)
        {
            actionTimer = 0;
            int action = Random.Range(0, 2);
            if (action == 0) StartCoroutine(action_0());
            else StartCoroutine(action_1());
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