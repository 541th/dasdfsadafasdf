using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swill : MonoBehaviour
{
    [SerializeField] GameObject swillPrefab, explPrefab;
    [SerializeField] int amount;

    public void show(Vector2 player)
    {
        GameObject expl = Instantiate(explPrefab, transform.position, Quaternion.identity);
        Destroy(expl, 0.4f);

        for (int i = 0; i < amount; i++)
        {
            GameObject swill = Instantiate(swillPrefab, transform.position, Quaternion.identity);
            swill.GetComponent<SwillController>().target = (Vector3)player - transform.position;
        }
    }
}
