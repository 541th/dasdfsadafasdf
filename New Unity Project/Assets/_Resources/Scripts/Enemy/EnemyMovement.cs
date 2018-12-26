using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
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

    public enum State { walk, startAttack, attack };
    public State curState;

    public Vector2 dir;

    List<Vector2> path = new List<Vector2>();

    float walkCounter;
    float waitCounter;
    float attackCounter = 5;

    bool isWalking;
    bool gotDir;

    void Start()
    {
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
        if (player != null && curState == State.walk)
        {
            setSA();
        }

        if (curState == State.walk)
        {
            _a.SetBool("Attack", false);
            if (isWalking)
            {
                if (!gotDir)
                {
                    dir = AI.chooseDir();
                    gotDir = true;

                    //setCollider(false);
                }

                RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, 10, mask);

                if (ray.collider != null && !ray.collider.isTrigger)
                {
                    dir = AI.chooseDirectionWithException(AI.getDirByVector(dir));
                }

                walkCounter -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + dir, ms * Time.deltaTime / 2);

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
        else if (curState == State.startAttack)
        {
            Vector2 dir = player.transform.position - transform.position;
            /*
            float angle = Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI - 90;

            if (angle <= 41 && angle > -57) AI.setDirTo(ref this.dir, new Vector2(0, 1));
            else
            if (angle <= -57 && angle > -126) AI.setDirTo(ref this.dir, new Vector2(1, 0));
            else
            if (angle <= -126 && angle > -230) AI.setDirTo(ref this.dir, new Vector2(0, -1));
            else
            if (angle <= -230 || angle > 41) AI.setDirTo(ref this.dir, new Vector2(-1, 0));
            */
            attackCounter -= Time.deltaTime;

            if (attackCounter < 0) curState = State.attack;
        }
        else if (curState == State.attack)
        {
            _a.SetBool("Run", true);
            delay += Time.deltaTime;

            if (delay >= Random.Range(0, 0.3f))//Random.Range(0.8f, 1.2f))
            {
                path = AI.setDestination(transform.position, player.transform.position);
                points = path.Count - 1;
                delay = 0;
            }

            if (delay >= Random.Range(0, 0.3f))//Random.Range(0.8f, 1.2f))
            {
                path = AI.setDestination(transform.position, player.transform.position);
                points = path.Count - 1;
                delay = 0;
            }

            if ((Vector2.Distance(transform.position, player.transform.position) < 2) && !_a.GetBool("Attack"))
            {
                attack();
                //setCollider(true);
                return;
            }

            if (path != null)
                if (path.Count != 0)
                {
                    if (points < 0)
                    {
                        attack();
                        return;
                    }

                    transform.position = Vector2.MoveTowards(transform.position, path[points], ms * Time.deltaTime);

                    if (Vector2.Distance(transform.position, path[points]) < 0.01f && points != 0)
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

    void attack()
    {
        path.Clear();
        _a.SetBool("Attack", true);

        /*
        Vector2 dirToPlayer = (Vector2)player.transform.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * 180 / Mathf.PI - 90;

        if (angle <= 41 && angle > -57) AI.setDirTo(ref dir, new Vector2(0, 1));
        else
        if (angle <= -57 && angle > -126) AI.setDirTo(ref dir, new Vector2(1, 0));
        else
        if (angle <= -126 && angle > -230) AI.setDirTo(ref dir, new Vector2(0, -1));
        else
        if (angle <= -230 || angle > 41) AI.setDirTo(ref dir, new Vector2(-1, 0));
        */
        Invoke("setSA", 1f);
    }

    void setSA()
    {
        if (attackCounter <= 0) attackCounter = Random.Range(0, 2);
        curState = State.startAttack;
        _a.SetBool("Walk", false);
        _a.SetBool("Run", false);
        _a.SetBool("Attack", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Player" || collision.transform.name == "Ally")
        {
            _a.SetBool("Attack", true);
            _a.SetBool("Run", false);
            attack();
            return;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (!_a.GetBool("Attack") && curState == State.attack && _a.GetBool("Run"))
            {
                attack();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (_a == null) _a = GetComponent<Animator>();

            player = collision.transform.gameObject;
            attackCounter = Random.Range(2, 5);
            curState = State.startAttack;
            _a.SetBool("Walk", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.tag == "Player") && Vector2.Distance(collision.transform.position, transform.position) > 12)
        {
            player = null;
            if (_a == null) _a = GetComponent<Animator>();
            
            curState = State.walk;
            _a.SetBool("Run", false);
            //setCollider(false);
        }
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