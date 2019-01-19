﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divide : MonoBehaviour
{
    [SerializeField] GameObject expl;
    [SerializeField] GameObject part;
    [SerializeField] int min_count, max_count;
    [SerializeField] float minSize, div;

    public void divide()
    {
        if (transform.parent.localScale.x < minSize) return;

        if (expl != null)
        {
            GameObject go = Instantiate(expl, transform.position, Quaternion.identity);
            Destroy(go, .6f);
        }

        int count = Random.Range(min_count, max_count);

        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(part, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Quaternion.identity);
            enemy.transform.GetChild(0).GetComponent<EnemyAttack>().damage /= 2;
            enemy.transform.GetChild(1).GetComponent<EnemyHP>().expForKill /= 2;
            enemy.transform.GetChild(1).GetComponent<EnemyHP>().HP = GetComponent<EnemyHP>().maxHP / 2;
            enemy.transform.GetChild(1).GetComponent<EnemyHP>().maxHP = GetComponent<EnemyHP>().maxHP / 2;
            enemy.transform.localScale /= div;
        }
    }
}