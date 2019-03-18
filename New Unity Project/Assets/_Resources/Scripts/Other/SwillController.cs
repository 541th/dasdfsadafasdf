using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwillController : MonoBehaviour
{
    public Vector2 target;
    [SerializeField] bool dontDestroy;

    private void Start()
    {
        StartCoroutine(mv());
    }

    IEnumerator mv()
    {
        float timer = Random.Range(0.1f, 0.2f);

        if (!dontDestroy)
            target = target.normalized + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (!dontDestroy)
                transform.position -= (Vector3)target * Time.deltaTime * 14;

            yield return null;
        }

        if (!dontDestroy)
            Destroy(gameObject, 10);
    }

    PlayerHP _php;
    PlayerExp _pe;
    bool stopDamage;
    IEnumerator damage()
    {
        if (_php == null) _php = FindObjectOfType<PlayerHP>();
        if (_pe == null) _pe = FindObjectOfType<PlayerExp>();

        while (!stopDamage)
        {
            _php.toDamage(1 + _pe.getKoefByLvl());
            yield return new WaitForSeconds(0.4f);
        }

        stopDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(damage());
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
