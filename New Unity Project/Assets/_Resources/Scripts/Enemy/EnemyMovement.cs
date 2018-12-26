using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Animator _a;
    AIMethods AI;
    GameObject player;

    public float ms;
    int points;

    float delay, delta;
    GridOfNodes gon;

    public Vector2 dir;

    List<Vector2> path = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        AI = GetComponent<AIMethods>();
        _a = GetComponent<Animator>();
        if (gon == null)
            gon = FindObjectOfType<GridOfNodes>();
        player = GameObject.Find("Player");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        foreach (Vector2 item in path)
        {
            Gizmos.DrawCube(item, new Vector3(0.5f, 0.5f, 0.5f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        delay += Time.deltaTime;
        delta -= Time.deltaTime;

        if (delay >= Random.Range(0, 0.3f))//Random.Range(0.8f, 1.2f))
        {
            path = AI.setDestination(transform.position, player.transform.position);
            points = path.Count - 1;
            delay = 0;
        }

        if ((Vector2.Distance(transform.position, player.transform.position) < 2f))// && !_a.GetBool("Attack"))
        {
            path.Clear();
            print("attack");
            //attack();
            //setCollider(true);
            return;
        }

        if (path != null)
            if (path.Count != 0)
            {
                if (points < 0)
                {
                    path.Clear();
                    print("attack");
                    //attack();
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

                    //anim.SetFloat("MoveX", dir.x);
                    //anim.SetFloat("MoveY", dir.y);
                    //anim.SetFloat("LastMoveX", dir.x);
                    //anim.SetFloat("LastMoveY", dir.y);
                }
            }
    }
}
