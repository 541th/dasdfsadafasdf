using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    SpriteRenderer _sr;
    Transform _t;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _t = transform;
    }

    void Update()
    {
        _sr.sortingOrder = -(int)(_t.position.y * 111);
    }
}
