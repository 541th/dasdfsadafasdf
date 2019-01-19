using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwillController : MonoBehaviour
{
    public Vector2 target;

    private void Start()
    {
        StartCoroutine(mv());
    }

    IEnumerator mv()
    {
        float timer = Random.Range(0.1f, 0.2f);
        target = target.normalized + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position -= (Vector3)target * Time.deltaTime * 14;
            yield return null;
        }

        Destroy(gameObject, 10);
    }

    bool stopDamage;
    IEnumerator damage(PlayerHP _php)
    {
        while (!stopDamage)
        {
            _php.toDamage(1);
            yield return new WaitForSeconds(0.4f);
        }

        stopDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(damage(collision.GetComponent<PlayerHP>()));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stopDamage = true;
        }
    }

}
