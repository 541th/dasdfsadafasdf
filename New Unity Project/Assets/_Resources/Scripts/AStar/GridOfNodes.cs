using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOfNodes : MonoBehaviour
{
    public GridsNode[,] nodes;

    public Vector2 mapSize;
    public bool drawGizmosW, drawGizmosNW;
    public bool nodeIsCreated;
    float halfMapSizeX, halfMapSizeY;

    public LayerMask mask;

    private void OnLevelWasLoaded(int level)
    {
        createGrid();
    }

    Vector2 mainMapPos;

    private void Start()
    {
        mainMapPos = transform.position;
        halfMapSizeX = mapSize.x / 2;
        halfMapSizeY = mapSize.y / 2;
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
        nodes = new GridsNode[(int)mapSize.x * 2, (int)mapSize.y * 2];

        float x = transform.position.x - (int)mapSize.x / 2;
        float y = transform.position.y - (int)mapSize.y / 2;
        int idX = 0, idY = 0;

        while (x < transform.position.x + (int)mapSize.x / 2 && idX < (int)mapSize.x * 2)
        {
            while (y < transform.position.y + (int)mapSize.y / 2 && idY < (int)mapSize.y * 2)
            {
                nodes[idX, idY] = 
                    new GridsNode();
                nodes[idX, idY].pos = new Vector2(x, y);
                nodes[idX, idY].idX = idX;
                nodes[idX, idY].idY = idY;

                nodes[idX, idY].walkable = isWalkable(new Vector2(x, y));

                idY++;
                y += 1;
            }

            x += 1;
            y = transform.position.y - (int)mapSize.y / 2;

            idX++;

            idMaxY = idY;
            idY = 0;
        }

        idMaxX = idX;

        nodeIsCreated = true;
    }

    public bool isWalkable(Vector2 point)
    {
        return !(Physics2D.BoxCast(new Vector2(point.x, point.y), new Vector2(0.1f, 0.1f), 0, new Vector2(0, 0), 1, mask).collider
                    && (!Physics2D.BoxCast(new Vector2(point.x, point.y), new Vector2(0.1f, 0.1f), 0, new Vector2(0, 0), 1, mask).collider.isTrigger));
    }

    public List<GridsNode> GetNeighboursOf(GridsNode node, string whoCalls)
    {
        List<GridsNode> n = new List<GridsNode>();
        int _i = 0, _j = 0;

        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                _i = i; _j = j;

                if (i < 0) _i = i * -1;
                if (j < 0) _j = j * -1;

                int x = node.idX + i;
                int y = node.idY + j;

                if (x >= 0 && x < idMaxX && y >= 0 && y < idMaxY)
                    n.Add(nodes[x, y]);
            }

        return n;
    }

    public GridsNode GetNodeByPos(Vector2 pos)
    {
        return nodes[(int)(pos.x + halfMapSizeX + 0.55f), (int)(pos.y + halfMapSizeY + 0.8f)];
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

