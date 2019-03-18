using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemies;
    public int[] randAmount;

    void Start()
    {
        int indx = Random.Range(0, enemies.Length);

        if (randAmount[indx] < 0)
        {
            for (int i = 0; i < -randAmount[indx]; i++)
            {
                transform.parent.GetComponent<EnemyManager>().addCount();
                Instantiate(enemies[indx], transform.position, Quaternion.identity);
            }
        }
        else
            for (int i = 0; i < 3 + Random.Range(0, randAmount[indx]); i++)
            {
                transform.parent.GetComponent<EnemyManager>().addCount();
                Instantiate(enemies[indx], transform.position, Quaternion.identity);
            }
    }
}
