using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int HP;
    Transform player;

    public void toDamage(int damage, bool isFlying)
    {
        HP -= damage;

        if (isFlying)
        {
            if (player == null) player = GameObject.Find("Player").transform;
            StartCoroutine(flying(player.position - transform.parent.position));
        }

        if (HP <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    IEnumerator flying(Vector2 to)
    {
        float timer = 0.2f;
        float x = -to.x * Time.deltaTime * 1400, y = -to.y * Time.deltaTime * 1400;
        Rigidbody2D parent = transform.parent.GetComponent<Rigidbody2D>();

        //parent.AddForce(to * 100, ForceMode2D.Force);
        /*
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            parent.AddForce((Vector2)parent.transform.position - to, ForceMode2D.Impulse);

            yield return null;
        }*/

        
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            x += (0 - x) * Time.deltaTime * 20;
            y += (0 - y) * Time.deltaTime * 20;

            parent.velocity = new Vector3(x, y, 0);

            yield return null;
        }

        parent.velocity = Vector3.zero;
    }
}
