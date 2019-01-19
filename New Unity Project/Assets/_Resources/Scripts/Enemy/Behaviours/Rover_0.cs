using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover_0 : MonoBehaviour
{
    Transform _t;
    public LayerMask mask;

    Animator _a;
    AIMethods AI;
    GameObject player;

    public float walkTime = 2.5f;
    public float waitTime = 3f;
    
    int points;

    float delay, delta;
    GridOfNodes gon;

    public enum State { walk, attack, rove };
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
        if (AI.stanned) return;

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

                _t.position = Vector2.MoveTowards(_t.position, (Vector2)_t.position + dir, AI.ms * Time.deltaTime / 2);

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
            float _d = Vector2.SqrMagnitude(_t.position - player.transform.position);

            GetComponent<BoxCollider2D>().enabled = _d < 20;

            if (delta <= 0)
            {
                //if (Vector2.Distance(transform.position, ((targets.Count == 0) ? player.transform.position : targets[0].transform.position)) > 2)
                {
                    playerRandomPoint = new Vector2(player.transform.position.x + Random.Range(-1.1f, 1.1f),
                        player.transform.position.y + Random.Range(-1.1f, 1.1f));

                    if (gon.GetNodeByPos(playerRandomPoint) != null)
                        while (!gon.GetNodeByPos(playerRandomPoint).walkable)
                        {
                            playerRandomPoint = new Vector2(player.transform.position.x + Random.Range(-1.1f, 1.1f),
                                player.transform.position.y + Random.Range(-1.1f, 1.1f));
                        }
                    else
                    {
                        playerRandomPoint = player.transform.position;
                    }

                    playerRandomPoint = gon.GetNodeByPos(playerRandomPoint).pos;
                }

                delta = 2;
            }

            if (delay >= Random.Range(0, 0.3f))//Random.Range(0.8f, 1.2f))
            {
                path = AI.setDestination(transform.position, playerRandomPoint);
                points = path.Count - 1;
                delay = 0;
            }

            if (Vector2.SqrMagnitude(_t.position - player.transform.position) < 24)
            {
                curState = State.rove;
                return;
            }

            if (AI.netting) return;

            if (path != null)
                if (path.Count != 0)
                {
                    _t.position = Vector2.MoveTowards(_t.position, path[points], AI.ms * Time.deltaTime);

                    if (Vector2.Distance(_t.position, path[points]) < 0.1f && points != 0)
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
        else if (curState == State.rove && !isRoving)
        {
            StartCoroutine(roving());
        }
    }

    bool isRoving;
    IEnumerator roving()
    {
        isRoving = true;

        yield return new WaitForSeconds(2);

        float rovingTimer = 4;
        Vector2 target = -(_t.position - player.transform.position).normalized * 20;

        GetComponent<BoxCollider2D>().enabled = true;

        while (rovingTimer > 0)
        {
            if (!isRoving) yield break;

            rovingTimer -= Time.deltaTime;

            _a.SetBool("Attack", true);
            GetComponent<Rigidbody2D>().velocity = target;

            yield return null;
        }

        _a.SetBool("Attack", false);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        curState = State.attack;

        GetComponent<BoxCollider2D>().enabled = false;

        isRoving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Collisions") || collision.transform.CompareTag("Player"))
        {
            isRoving = false;
            GetComponent<BoxCollider2D>().enabled = false;
            _a.SetBool("Attack", false);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            curState = State.attack;
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
