using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideSmoke : MonoBehaviour
{
    float mult1, mult2;
    void Start()
    {
        mult1 = Random.Range(0.6f, 1.6f);
        mult2 = Random.Range(0.6f, 1.6f);
        Destroy(gameObject, 1);
    }

    void Update()
    {
        transform.position += new Vector3(0, Time.deltaTime * mult1);
        transform.localScale += new Vector3(Time.deltaTime * 4 * mult2, Time.deltaTime * 4 * mult2);
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, Time.deltaTime);
    }
}
