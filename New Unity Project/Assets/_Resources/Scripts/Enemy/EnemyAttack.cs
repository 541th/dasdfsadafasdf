using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;
    public int randL, randH;
    public float sub;
    [SerializeField] bool arrow, slowDown, isBoss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int _d = damage + Random.Range(randL, randH);
            _d = _d - (_d * (int)(sub / 100));

            if (_d <= 0) _d = 1;

            FindObjectOfType<PlayerHP>().toDamage(_d);

            if (slowDown) FindObjectOfType<PlayerHP>().slowDown(4);

            if (isBoss) FindObjectOfType<CamFollow>().punchShake();

            if (arrow && GetComponent<EnemyArrowFly>() != null) GetComponent<EnemyArrowFly>().stop();
        }
    }
}
