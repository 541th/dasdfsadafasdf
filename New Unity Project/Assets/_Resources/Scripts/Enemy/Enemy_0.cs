using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0 : MonoBehaviour
{
    Transform _t;
    public LayerMask mask;

    Animator _a;
    AIMethods AI;
    GameObject player;

    public float walkTime = 2.5f;
    public float waitTime = 3f;

    public float ms;
    int points;

    float delay, delta;
    GridOfNodes gon;

    public enum State { walk, attack };
    public State curState;

    public Vector2 dir;

    List<Vector2> path = new List<Vector2>();

    float walkCounter;
    float waitCounter;

    bool isWalking;
    bool gotDir;

    Vector2 playerRandomPoint;

    void Start()
    {
        _t = transform;
        AI = GetComponent<AIMethods>();
        _a = GetComponent<Animator>();
        if (gon == null)
            gon = FindObjectOfType<GridOfNodes>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        foreach (Vector2 item in path)
        {
            Gizmos.DrawCube(item, new Vector3(0.5f, 0.5f, 0.5f));
        }
    }

    void Update()
    {
        if (curState == State.walk)
        {
            if (isWalking)
            {
                if (!gotDir)
                {
                    dir = AI.chooseDir();
                    gotDir = true;

                    RaycastHit2D ray = Physics2D.Raycast(_t.position, dir, 10, mask);

                    if (ray.collider != null && !ray.collider.isTrigger)
                    {
                        dir = AI.chooseDirectionWithException(AI.getDirByVector(dir));
                    }
                }

                walkCounter -= Time.deltaTime;

                _t.position = Vector2.MoveTowards(_t.position, (Vector2)_t.position + dir, ms * Time.deltaTime / 2);

                _a.SetFloat("MoveX", dir.x);
                _a.SetFloat("MoveY", dir.y);
                _a.SetFloat("LastMoveX", dir.x);
                _a.SetFloat("LastMoveY", dir.y);
                _a.SetBool("Walk", true);

                if (walkCounter <= 0) stopWalking();
            }
            else
            {
                waitCounter -= Time.deltaTime;

                _a.SetBool("Walk", false);
                if (waitCounter <= 0) startWalking();
            }
        }
        else if (curState == State.attack)
        {
            delay += Time.deltaTime;
            delta -= Time.deltaTime;

            if ((Vector2.Distance(_t.position, player.transform.position) < 1))
            {
                _a.SetTrigger("Attack");
                return;
            }

            if (delta <= 0)
            {
                //if (Vector2.Distance(transform.position, ((targets.Count == 0) ? player.transform.position : targets[0].transform.position)) > 2)
                {
                    playerRandomPoint = new Vector2(player.transform.position.x + Random.Range(-2.1f, 2.1f), 
                        player.transform.position.y + Random.Range(-2.1f, 2.1f));

                    if (gon.GetNodeByPos(playerRandomPoint) != null)
                        while (!gon.GetNodeByPos(playerRandomPoint).walkable)
                        {
                            playerRandomPoint = new Vector2(player.transform.position.x + Random.Range(-2.1f, 2.1f), 
                                player.transform.position.y + Random.Range(-2.1f, 2.1f));
                        }
                    else
                    {
                        playerRandomPoint = player.transform.position;
                    }

                    playerRandomPoint = gon.GetNodeByPos(playerRandomPoint).pos;
                }

                delta = 2;
            }

            if (Vector2.SqrMagnitude(transform.position - player.transform.position) < 20)
            {
                playerRandomPoint = player.transform.position;
            }

            if (delay >= Random.Range(0, 0.3f))//Random.Range(0.8f, 1.2f))
            {
                path = AI.setDestination(transform.position, playerRandomPoint);
                points = path.Count - 1;
                delay = 0;
            }

        //if (delay >= Random.Range(0, 0.3f))//Random.Range(0.8f, 1.2f))
        {

                /*
                if (Vector2.SqrMagnitude(_t.position - player.transform.position) < 30)
                {
                }
                else
                {
                    path = AI.setDestination(_t.position, player.transform.position + new Vector3(Random.Range(-2.1f, 2), Random.Range(-2.1f, 2), 0));
                }

                path = AI.setDestination(_t.position, playerRandomPoint);
                points = path.Count - 1;
                delay = 0;
                */
            }

            if (path != null)
                if (path.Count != 0)
                {
                    _t.position = Vector2.MoveTowards(_t.position, path[points], ms * Time.deltaTime);

                    if (Vector2.Distance(_t.position, path[points]) < 0.01f && points != 0)
                    {
                        points--;

                        if (points <= 0) return;

                        //Vector2 dirToPlayer = path[points] - (Vector2)transform.position;
                        //float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * 180 / Mathf.PI - 90;

                        //if (angle <= 41 && angle > -57) AI.setDirTo(ref dir, new Vector2(0, 1));
                        //else
                        //if (angle <= -57 && angle > -126) AI.setDirTo(ref dir, new Vector2(1, 0));
                        //else
                        //if (angle <= -126 && angle > -230) AI.setDirTo(ref dir, new Vector2(0, -1));
                        //else
                        //if (angle <= -230 || angle > 41) AI.setDirTo(ref dir, new Vector2(-1, 0));

                        _a.SetFloat("MoveX", dir.x);
                        _a.SetFloat("MoveY", dir.y);
                        _a.SetFloat("LastMoveX", dir.x);
                        _a.SetFloat("LastMoveY", dir.y);
                    }
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_a == null) _a = GetComponent<Animator>();

            player = collision.transform.gameObject;

            Invoke("startAttack", Random.Range(0.4f, 1));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && collision.transform.GetComponent<PlayerMovement>().isGlide)
            GetComponentInChildren<EnemyHP>().toDamage(Random.Range(4, 10), true);
    }

    void startAttack()
    {
        curState = State.attack;
    }

    void startWalking()
    {
        gotDir = false;
        isWalking = true;
        walkCounter = Random.Range(1, walkTime);
    }

    void stopWalking()
    {
        waitCounter = Random.Range(1, waitTime);
        isWalking = false;
    }
}
