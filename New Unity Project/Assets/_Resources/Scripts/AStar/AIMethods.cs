using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMethods : MonoBehaviour {
    public bool dontShowOCI, stanned, hasDeathAnimation;
    string _tag = "";
    public float ms;
    public bool netting;

    private void Start()
    {
        _tag = tag;
    }

    public Vector2 chooseDirectionWithException(int exclucdedDirection)
    {
        int returnableDir = Random.Range(0, 4);

        if (exclucdedDirection == returnableDir) returnableDir++;
        if (returnableDir == 4) returnableDir = 0;

        return getVectorByDir(returnableDir);
    }

    public void startStan()
    {
        StartCoroutine(stannedEvent());
    }

    IEnumerator stannedEvent()
    {
        stanned = true;

        GameObject stannedEffect = Instantiate(Resources.Load("Prefabs/Effects/StannedEffect") as GameObject);
        stannedEffect.transform.SetParent(transform);
        stannedEffect.transform.localPosition = new Vector2(0, 0.04f);
        stannedEffect.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;

        float timer = 2 + InfoController.perks[4].value;
        print(InfoController.perks[4].value);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        
        stanned = false;
        Destroy(stannedEffect);
    }

    /*
    public void setDirTo(Vector2 value)
    {
        GetComponent<EnemyMovement>().dir = value;
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("MoveX", GetComponent<EnemyMovement>().dir.x);
        anim.SetFloat("MoveY", GetComponent<EnemyMovement>().dir.y);
        anim.SetFloat("LastMoveX", GetComponent<EnemyMovement>().dir.x);
        anim.SetFloat("LastMoveY", GetComponent<EnemyMovement>().dir.y);
    }*/
    
    public void setDirTo(ref Vector2 dir, Vector2 value)
    {
        dir = value;
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("MoveX", dir.x);
        anim.SetFloat("MoveY", dir.y);
        anim.SetFloat("LastMoveX", dir.x);
        anim.SetFloat("LastMoveY", dir.y);
    }

    public Vector2 chooseDir()
    {
        int dir = Random.Range(0, 4);

        return getVectorByDir(dir);
    }

    public Vector2 getVectorByDir(int dir)
    {
        switch (dir)
        {
            case 0: return new Vector2(-1, 0);
            case 1: return new Vector2(0, -1);
            case 2: return new Vector2(1, 0);
            case 3: return new Vector2(0, 1);
        }

        return Vector2.zero;
    }

    public int getDirByVector(Vector2 v)
    {
        if (v == new Vector2(-1, 0)) return 0;
        else if (v == new Vector2(0, -1)) return 1;
        else if (v == new Vector2(1, 0)) return 2;
        else if (v == new Vector2(0, 1)) return 3;

        return -1;
    }

    GridOfNodes gon;

    GridsNode startNode;
    GridsNode targetNode;
    HashSet<GridsNode> closedSet;
    public List<Vector2> setDestination(Vector2 from, Vector2 to)
    {
        to -= new Vector2(0, 0.2f);
            
        if (gon == null)
            gon = FindObjectOfType<GridOfNodes>();
        
        Heap<GridsNode> openSet = new Heap<GridsNode>(gon.MaxSize);
        closedSet = new HashSet<GridsNode>();

        startNode = gon.GetNodeByPos(from);
        targetNode = gon.GetNodeByPos(to);

        openSet.add(startNode);
        int iters = 0;

        GridsNode node;

        while (openSet.Count > 0)
        {
            GridsNode currentNode = openSet.removeFirst();

            closedSet.Add(currentNode);

            if (currentNode == targetNode)//(Vector2.Distance(currentNode.pos, to) < 0.5f)
            {
                closedSet.Add(currentNode);

                setPath(startNode, targetNode);

                return path;
            }

            List<GridsNode> nodes = gon.GetNeighboursOf(currentNode, _tag);

            for (int i = 0; i < nodes.Count; i++)
            {
                node = nodes[i];

                if (!node.walkable || closedSet.Contains(node)) continue;

                int newCost = (int)currentNode.G + (int)distance(currentNode, node);
                if (newCost < node.G || !openSet.contains(node))
                {
                    node.G = newCost;
                    node.H = (int)distance(node, targetNode);
                    node.cameFrom = currentNode;

                    if (!openSet.contains(node))
                        openSet.add(node);
                    else
                        openSet.updateItem(node);
                }
            }

            iters++;
            if (iters >= 100) break;
        }

        print("Path not found");
        path.Add(from);
        return path;
    }

    List<Vector2> path = new List<Vector2>();

    void setPath(GridsNode startNode, GridsNode endNode)
    {
        path.Clear();

        GridsNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.pos);
            currentNode = currentNode.cameFrom;
        }
    }

    float distance(GridsNode A, GridsNode B)
    {
        float x = A.idX - B.idX, y = A.idY - B.idY;

        if (x < 0) x *= -1;
        if (y < 0) y *= -1;

        return x*100 + y*100;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && collision.transform.GetComponent<PlayerMovement>().isGlide)
            GetComponentInChildren<EnemyHP>().toDamage(Random.Range(4, 10) + (int)InfoController.perks[1].value, true, false, false, false, false, false);
    }

    GameObject OCI;

    public void subDamage()
    {
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).GetComponent<EnemyAttack>() != null)
            {
                transform.GetChild(i).GetComponent<EnemyAttack>().sub = InfoController.perks[10].value;
                break;
            }
    }

    public void showDeath()
    {
        stanned = true;

        if (hasDeathAnimation)
        {
            GetComponent<Animator>().SetBool("Death", true);
            Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
        else
            Destroy(gameObject, 0);
        //GetComponent<SpriteRenderer>().color = (GetComponent<NPCMovement>() == null) ? new Color(1, 1, 1, 1) : GetComponent<NPCMovement>().skinColor;
    }
}