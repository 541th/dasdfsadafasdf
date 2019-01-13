using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByCircle : MonoBehaviour
{
    Transform _t;
    float timer, x, y;
    private void Start()
    {
        _t = transform;
        x = Random.Range(40, 70);
        y = Random.Range(40, 70);
        x *= Random.Range(0, 2) == 0 ? 1 : -1;
        y *= Random.Range(0, 2) == 0 ? 1 : -1;
    }

    void Update()
    {
        timer += Time.deltaTime;
        _t.position += new Vector3(Mathf.Cos(timer) / x, Mathf.Sin(timer) / y);
    }
}