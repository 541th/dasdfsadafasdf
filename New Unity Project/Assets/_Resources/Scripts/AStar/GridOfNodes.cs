using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOfNodes : MonoBehaviour
{
    public GridsNode[,] nodes;

    public Vector2 mapSize;
    public bool drawGizmosW, drawGizmosNW;
    public bool nodeIsCreated;

    private void OnLevelWasLoaded(int level)
    {
        createGrid();
    }

    Vector2 mainMapPos;

    private void Start()
    {
        mainMapPos = transform.position;
        //mainMapPos = FindObjectOfType<Tiled2Unity.TiledMap>().transform.position;
        createGrid();
    }

    private void Update()
    {
        //mainMapPos = FindObjectOfType<Tiled2Unity.TiledMap>().transform.position;
        createGrid();
    }

    public int MaxSize
    {
        get
        {
            return (int)mapSize.x * (int)mapSize.y;
        }
    }

    int idMaxX, idMaxY;
    void createGrid()
    {
        mapSize = GetComponent<BoxCollider2D>().size;
        nodes = new GridsNode[(int)mapSize.x * 2, (int)mapSize.y * 2];

        float x = GetComponent<BoxCollider2D>().transform.position.x - (int)mapSize.x / 2;
        float y = GetComponent<BoxCollider2D>().transform.position.y - (int)mapSize.y / 2;
        int idX = 0, idY = 0;

        while (x < GetComponent<BoxCollider2D>().transform.position.x + (int)mapSize.x / 2)
        {
            while (y < GetComponent<BoxCollider2D>().transform.position.y + (int)mapSize.y / 2)
            {
                nodes[idX, idY] = new GridsNode();
                nodes[idX, idY].pos = new Vector2(x, y);
                nodes[idX, idY].idX = idX;
                nodes[idX, idY].idY = idY;

                nodes[idX, idY].walkable = isWalkable(new Vector2(x, y));

                idY++;
                y += 0.5f;
            }

            x += 0.5f;
            y = GetComponent<BoxCollider2D>().transform.position.y - (int)mapSize.y / 2;

            idX++;

            idMaxY = idY;
            idY = 0;
        }

        idMaxX = idX;

        nodeIsCreated = true;
    }

    public bool isWalkable(Vector2 point)
    {
        return !(Physics2D.BoxCast(new Vector2(point.x, point.y + 0.1f), new Vector2(0.1f, 0.1f), 0, new Vector2(0, 0)).collider
                    && (!Physics2D.BoxCast(new Vector2(point.x, point.y + 0.1f), new Vector2(0.1f, 0.1f), 0, new Vector2(0, 0)).collider.isTrigger));
    }

    public List<GridsNode> GetNeighboursOf(GridsNode node, string whoCalls)
    {
        List<GridsNode> n = new List<GridsNode>();
        int _i = 0, _j = 0;

        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                _i = i; _j = j;

                if (whoCalls == "Enemy")
                    if (i == 0 && j == 0) continue;

                if (i < 0) _i = i * -1;
                if (j < 0) _j = j * -1;

                if (_i == _j && whoCalls != "Enemy") continue;

                int x = node.idX + i;
                int y = node.idY + j;

                if (x >= 0 && x < idMaxX && y >= 0 && y < idMaxY)
                    n.Add(nodes[x, y]);
            }

        return n;
    }

    public GridsNode GetNodeByPos(Vector2 pos)
    {
        pos -= mainMapPos;

        int x = (int)(pos.x * 2);
        int y = idMaxY - (int)(pos.y * 2) * -1;

        return nodes[x, y];
    }

    private void OnDrawGizmos()
    {
        if (nodeIsCreated && (drawGizmosNW || drawGizmosW))
        {
            for (int x = 0; x < idMaxX; x++)
                for (int y = 0; y < idMaxY; y++)
                {
                    if (!nodes[x, y].walkable && drawGizmosNW)
                    {
                        Gizmos.color = Color.red;

                        Gizmos.DrawCube(new Vector3(nodes[x, y].pos.x, nodes[x, y].pos.y, 0), new Vector3(0.25f, 0.25f, 0.25f));
                    }

                    if (nodes[x, y].walkable && drawGizmosW)
                    {
                        Gizmos.color = Color.green;

                        Gizmos.DrawCube(new Vector3(nodes[x, y].pos.x, nodes[x, y].pos.y, 0), new Vector3(0.25f, 0.25f, 0.25f));
                    }
                }
        }
    }
}

public class GridsNode : IHeapItem<GridsNode>
{
    public int idX, idY;
    public Vector2 pos;
    public bool walkable;
    public GridsNode cameFrom;
    int heapIndex;

    public int H, G;

    public int F
    {
        get
        {
            return H + G;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(GridsNode nodeToCompare)
    {
        int compare = F.CompareTo(nodeToCompare.F);

        if (compare == 0) compare = H.CompareTo(nodeToCompare.H);

        return -compare;
    }
}

