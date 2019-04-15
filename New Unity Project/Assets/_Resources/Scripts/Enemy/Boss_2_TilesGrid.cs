using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_2_TilesGrid : MonoBehaviour
{
    GameObject[,] tiles = new GameObject[24, 20];

    private void Start()
    {
        Transform t = transform;
        for (int i = 0; i < t.childCount; i++)
        {
            tiles[i % 24, i / 24] = (t.GetChild(i).GetComponent<SpriteRenderer>() != null ? t.GetChild(i).gameObject : null);
        }
    }

    public void startEllipse(Vector2 pos)
    {
        int x = (int)Mathf.Abs((pos.x - .5f) / 1.3f);
        int y = (int)Mathf.Abs((pos.y - 30f) / 1.3f);

        StartCoroutine(ellipseEvent(x, y));
    }

    IEnumerator ellipseEvent(int startX, int startY)
    {
        yield return new WaitForSeconds(.5f);

        int a2, b2, x0, y0, x, y;
        for (int i = 0; i < 5; i++)
        {
            float X1 = Mathf.Clamp(((float)startX - 1 - i), 0, 23), X2 = Mathf.Clamp(((float)startX + 1 + i), 0, 23);
            float Y1 = Mathf.Clamp(((float)startY - 1 - i), 0, 19), Y2 = Mathf.Clamp(((float)startY + 1 + i), 0, 19);

            a2 = (int)(((X2 - X1) / 2) * ((X2 - X1) / 2));
            b2 = (int)(((Y2 - Y1) / 2) * ((Y2 - Y1) / 2));
            x0 = (int)((X2 + X1) / 2);
            y0 = (int)((Y2 + Y1) / 2);

            for (x = (int)X1; x <= (int)X2; x++)
            {
                for (y = (int)Y1; y <= (int)Y2; y++)
                {
                    if (((x - x0) * (x - x0) / a2 + (y - y0) * (y - y0) / b2) == 1 && tiles[x, y] != null)
                    {
                        tiles[x, y].gameObject.SetActive(true);
                        tiles[x, y].GetComponent<Animator>().SetTrigger("Event");
                    }
                }
            }

            yield return new WaitForSeconds(.5f);
        }

        Transform t = transform;

        for (int i = 0; i < t.childCount; i++)
            t.GetChild(i).gameObject.SetActive(false);
    }

    public void startLine(Vector2 startPos, Vector2 endPos)
    {
        int x1 = (int)Mathf.Abs((startPos.x - .5f) / 1.3f);
        int y1 = (int)Mathf.Abs((startPos.y - 30f) / 1.3f);

        int x2 = (int)Mathf.Abs((endPos.x - .5f) / 1.3f);
        int y2 = (int)Mathf.Abs((endPos.y - 30f) / 1.3f);
        
        StartCoroutine(lineEvent(x1, y1, x2, y2));
    }

    IEnumerator lineEvent(int X1, int Y1, int X2, int Y2)
    {
        yield return new WaitForSeconds(.5f);

        int dx, dy, x, y, xSign, ySign;

        dx = X2 - X1;
        dy = Y2 - Y1;

        xSign = (dx < 0) ? -1 : 1;
        ySign = (dy < 0) ? -1 : 1;

        dx = Mathf.Abs(dx);
        dy = Mathf.Abs(dy);

        x = X1 - ((xSign == -1) ? 1 : 0);
        y = Y1 - ((ySign == -1) ? 1 : 0);

        float e = 2 * dy - dx;
        int i = 1;

        do
        {
            yield return new WaitForSeconds(.1f);

            if (tiles[x, y] != null)
            {
                tiles[x, y].gameObject.SetActive(true);
                tiles[x, y].GetComponent<Animator>().SetTrigger("Event");
            }

            while (e >= 0)
            {
                y += ySign;
                e -= 2 * dx * -xSign;
                yield return null;
            }

            x += xSign;
            e += 2 * dy;
            i++;

            if (i > 40) break;
        } while (i <= dx);

        Transform t = transform;

        yield return new WaitForSeconds(2f);

        for (i = 0; i < t.childCount; i++)
            t.GetChild(i).gameObject.SetActive(false);
    }
}
