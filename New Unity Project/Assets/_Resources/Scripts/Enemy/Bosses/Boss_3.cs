using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_3 : MonoBehaviour
{
    Transform _t;

    Animator _aUp, _aDown;
    AIMethods AI;
    GameObject player;

    int points;

    float delay, delta, shootTimer;

    GridOfNodes gon;

    int bossType;

    public Vector2 dir, playerRandomPoint;

    List<Vector2> path = new List<Vector2>();

    void Start()
    {
        bossType = Random.Range(0, 3);

        transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Player animators/" + (bossType + 1) + "_Torso") as RuntimeAnimatorController;
        transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Player animators/" + (bossType + 1) + "_Legs") as RuntimeAnimatorController;

        _t = transform;
        AI = GetComponent<AIMethods>();
        _aUp = transform.GetChild(0).GetComponent<Animator>();
        _aDown = transform.GetChild(1).GetComponent<Animator>();
        player = GameObject.Find("Player");
        if (gon == null)
            gon = FindObjectOfType<GridOfNodes>();
    }

    void Update()
    {
        if (AI.stanned) return;

        if (bossType == 0)
        {
            delay += Time.deltaTime;
            float _d = Vector2.SqrMagnitude(_t.position - player.transform.position);
            GetComponent<CapsuleCollider2D>().enabled = _d < 20;

            if (delay >= Random.Range(0, 0.3f))
            {
                path = AI.setDestination(transform.position, player.transform.position);
                points = path.Count - 1;
                delay = 0;
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

                        Vector2 dirToPlayer = path[points - 1] - (Vector2)_t.position;
                        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * 180 / Mathf.PI - 90;

                        if (angle <= 41 && angle > -57) AI.setDirTo(ref dir, new Vector2(0, 1));
                        else
                        if (angle <= -57 && angle > -126) AI.setDirTo(ref dir, new Vector2(1, 0));
                        else
                        if (angle <= -126 && angle > -230) AI.setDirTo(ref dir, new Vector2(0, -1));
                        else
                        if (angle <= -230 || angle > 41) AI.setDirTo(ref dir, new Vector2(-1, 0));

                        _aDown.SetFloat("mx", dir.x);
                        _aDown.SetFloat("my", dir.y);
                        _aDown.SetBool("run", true);
                        _aUp.SetFloat("mx", dir.x);
                        _aUp.SetFloat("my", dir.y);
                        _aUp.SetBool("attack", true);
                    }
                }
        }
        else 
        {
            shootTimer += Time.deltaTime;
            delay += Time.deltaTime;
            delta -= Time.deltaTime;

            float _d = Vector2.SqrMagnitude(_t.position - player.transform.position);

            GetComponent<CapsuleCollider2D>().enabled = _d < 6;

            if (delta <= 0)
            {
                //if (Vector2.Distance(transform.position, ((targets.Count == 0) ? player.transform.position : targets[0].transform.position)) > 2)
                {
                    playerRandomPoint = new Vector2(player.transform.position.x + Random.Range(-6f, 6f),
                        player.transform.position.y + Random.Range(-6f, 6f));

                    if (gon.GetNodeByPos(playerRandomPoint) != null)
                        while (!gon.GetNodeByPos(playerRandomPoint).walkable)
                        {
                            playerRandomPoint = new Vector2(player.transform.position.x + Random.Range(-6f, 6f),
                                player.transform.position.y + Random.Range(-6f, 6f));
                        }
                    else
                    {
                        playerRandomPoint = player.transform.position;
                    }

                    playerRandomPoint = gon.GetNodeByPos(playerRandomPoint).pos;
                }

                delta = 1;
            }

            if (delay >= Random.Range(0, 0.3f))
            {
                path = AI.setDestination(transform.position, playerRandomPoint);
                points = path.Count - 1;
                delay = 0;
            }

            if (shootTimer > 1f)
            {
                StartCoroutine(shoot());

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

                        if (points <= 0)
                        {
                            _aDown.SetBool("run", false);
                            _aUp.SetBool("run", false);
                            return;
                        }

                        Vector2 dirToPlayer = path[points - 1] - (Vector2)_t.position;
                        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * 180 / Mathf.PI - 90;

                        if (angle <= 41 && angle > -57) AI.setDirTo(ref dir, new Vector2(0, 1));
                        else
                        if (angle <= -57 && angle > -126) AI.setDirTo(ref dir, new Vector2(1, 0));
                        else
                        if (angle <= -126 && angle > -230) AI.setDirTo(ref dir, new Vector2(0, -1));
                        else
                        if (angle <= -230 || angle > 41) AI.setDirTo(ref dir, new Vector2(-1, 0));

                        _aDown.SetFloat("mx", dir.x);
                        _aDown.SetFloat("my", dir.y);
                        _aDown.SetBool("run", true);
                        _aUp.SetBool("run", true);
                        _aUp.SetFloat("mx", dir.x);
                        _aUp.SetFloat("my", dir.y);
                    }
                }
        }
    }

    [SerializeField] GameObject magArrow, archerArrow;
    IEnumerator shoot()
    {
        _aUp.SetBool("attack", true);
        shootTimer = 0;

        yield return new WaitForSeconds(_aUp.GetCurrentAnimatorStateInfo(0).length / 2);

        Vector2 dirToPlayer = player.transform.position - _t.position;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * 180 / Mathf.PI - 90;

        if (angle <= 41 && angle > -57) AI.setDirTo(ref dir, new Vector2(0, 1));
        else
        if (angle <= -57 && angle > -126) AI.setDirTo(ref dir, new Vector2(1, 0));
        else
        if (angle <= -126 && angle > -230) AI.setDirTo(ref dir, new Vector2(0, -1));
        else
        if (angle <= -230 || angle > 41) AI.setDirTo(ref dir, new Vector2(-1, 0));

        if (player == null) player = GameObject.Find("Player");

        _aDown.SetFloat("mx", dir.x);
        _aDown.SetFloat("my", dir.y);
        _aUp.SetFloat("mx", dir.x);
        _aUp.SetFloat("my", dir.y);

        GameObject _arrow = Instantiate(bossType == 1 ? archerArrow : magArrow);
        _arrow.transform.position = transform.position + new Vector3(0, 1, 0);

        _arrow.GetComponent<EnemyArrowFly>().target = (player.transform.position - transform.position).normalized;
        _arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((player.transform.position - transform.position).normalized.y,
            (player.transform.position - transform.position).normalized.x) * 180 / Mathf.PI);

        _aUp.SetBool("attack", false);
    }
}