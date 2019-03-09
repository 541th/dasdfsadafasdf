using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_2 : MonoBehaviour
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

    IEnumerator action_0()
    {
        makingAction = true;

        _a.SetTrigger("Event_0");
        yield return new WaitForSeconds(.1f);

        if (Random.Range(0, 2) == 0)
            FindObjectOfType<Boss_2_TilesGrid>().startLine(transform.position, player.transform.position);
        else
            FindObjectOfType<Boss_2_TilesGrid>().startEllipse(transform.position);

        makingAction = false;
    }

    [SerializeField] GameObject resurrectPrefab;
    IEnumerator action_1()
    {
        makingAction = true;

        _a.SetTrigger("Event_1");

        yield return new WaitForSeconds(.6f);

        GameObject _enemy = Instantiate(resurrectPrefab);
        _enemy.transform.position = _t.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));

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