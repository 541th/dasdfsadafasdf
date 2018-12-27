using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    public int damage;
    public int randL, randH;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHP"))
        {
            collision.GetComponent<EnemyHP>().toDamage(damage + Random.Range(randL, randH), true);
        }
    }
}
