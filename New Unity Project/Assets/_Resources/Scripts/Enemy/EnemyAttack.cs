using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;
    public int randL, randH;
    public float sub;
    [SerializeField] bool arrow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int _d = damage + Random.Range(randL, randH);
            _d = _d - (_d * (int)(sub / 100));

            if (_d <= 0) _d = 1;

            collision.GetComponent<PlayerHP>().toDamage(_d);

            if (arrow && GetComponent<EnemyArrowFly>() != null) GetComponent<EnemyArrowFly>().stop();
        }
    }
}
