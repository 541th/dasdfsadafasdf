using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    public List<GameObject> _e = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(expand());
    }

    IEnumerator expand()
    {
        CircleCollider2D _cc = GetComponent<CircleCollider2D>();
        while (_cc.radius < 4)
        {
            _cc.radius += Time.deltaTime * 10;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyHP")
            if (!_e.Contains(collision.gameObject))
            {
                if (_e.Count < 7)
                {
                    if (collision.GetComponent<EnemyHP>() != null)
                        collision.GetComponent<EnemyHP>().toDamageLightning(10, _e);
                    else
                        collision.GetComponent<PartsHP>().toDamageLightning(10, _e);
                    _e.Add(collision.gameObject);
                }
                Destroy(gameObject);
            }
    }
}
