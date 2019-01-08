using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int randL, randH;
    [SerializeField] bool isArrow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHP"))
        {
            collision.GetComponent<EnemyHP>().toDamage(damage + Random.Range(randL, randH), true && !isArrow);

            if (isArrow)
            {

            }
        }
    }
}
