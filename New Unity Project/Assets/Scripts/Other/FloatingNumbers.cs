using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingNumbers : MonoBehaviour
{
    void Start()
    {
        transform.position += new Vector3(Random.Range(-0.4f, 0.4f), 0);
        Destroy(transform.parent.gameObject, 1);
    }

    public void setText(string value)
    {
        GetComponent<Text>().text = value;
    }

    void Update()
    {
        transform.parent.position += new Vector3(0, Time.deltaTime);
        GetComponent<Text>().color -= new Color(0, 0, 0, Time.deltaTime);
    }
}
