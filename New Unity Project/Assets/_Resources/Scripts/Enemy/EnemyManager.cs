using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    int enemyCount;

    public void addCount()
    {
        enemyCount++;
    }

    public void subCount()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            GameObject.Find("PortalEndObject").transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
