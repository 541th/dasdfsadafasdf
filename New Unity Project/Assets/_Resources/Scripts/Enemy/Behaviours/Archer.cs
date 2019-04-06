using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    Transform _t;
    public LayerMask mask;

    Animator _a;
    AIMethods AI;
    GameObject player;

    public float walkTime = 2.5f;
    public float waitTime = 3f;
    public float shootSpeed = 4f;
    float shootTimer = 0;

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
    bool gotDir, shooted;

    Vector2 playerRandomPoint;

    [SerializeField] bool shootFromSpot;

    void Start()
    {
        shootFromSpot = Random.Range(0, 2) == 0;
        _t = transform;
        AI = GetComponent<AIMethods>();
        _a = GetComponent<Animator>();
        if (gon == null)
            gon = FindObjectOfType<GridOfNodes>();
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
                }

                RaycastHit2D ray = Physics2D.Raycast(_t.position, dir, 2, mask);

                if (ray.collider != null)
                {
                    dir = AI.chooseDirectionWithException(AI.getDirByVector(dir));
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
        else
        if (curState == State.attack)
        {
            _a.SetBool("Walk", false);

            delay += Time.deltaTime;
            delta -= Time.deltaTime;
            shootTimer -= Time.deltaTime;

            float _d = Vector2.SqrMagnitude(_t.position - player.transform.position);

            if (_d < 28)
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

                if (shootTimer < 0)
                {
                    if (!shooted)
                    { 
                        _a.SetTrigger("Attack");

                        shooted = true;
                        shootTimer = shootSpeed;
                        StartCoroutine(shot(_a.GetCurrentAnimatorStateInfo(0).length / 3));
                        return;
                    }
                }
            }
            else
            {
                if (delay >= .1f)
                {
                    path = AI.setDestination(transform.position, player.transform.position);
                    points = path.Count - 1;
                    delay = 0;
                }

                if (AI.netting) return;

                if (path != null)
                    if (path.Count != 0)
                    {
                        _a.SetBool("Walk", true);
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
                            _a.SetFloat("LastMoveX", dir.x);
                            _a.SetFloat("LastMoveY", dir.y);
                        }
                    }
                    else
                        _a.SetBool("Walk", false);
            }
        }
    }

    [SerializeField] GameObject arrow = null;
    IEnumerator shot(float wait)
    {
        yield return new WaitForSeconds(wait);

        if (player == null) player = GameObject.Find("Player");

        GameObject _arrow = Instantiate(arrow);
        _arrow.transform.position = transform.position + new Vector3(0, 1, 0);

        Vector3 target = player.transform.position - new Vector3(0, .2f);

        _arrow.GetComponent<EnemyArrowFly>().target = (player.transform.position - transform.position).normalized;
        _arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((target - transform.position).normalized.y, 
            (target - transform.position).normalized.x) * 180 / Mathf.PI);

        shooted = false;

        _a.SetBool("Walk", false);
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
