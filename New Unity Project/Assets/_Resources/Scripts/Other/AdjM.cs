using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjM : MonoBehaviour
{
    SpriteRenderer _sr;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_sr.color.a > 0)
        {
            _sr.color -= new Color(0, 0, 0, Time.deltaTime);
        }
    }
}
