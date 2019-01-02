using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pass : MonoBehaviour
{
    [SerializeField] Transform _0, _1;
    public void pass0()
    {
        GameObject.Find("Player").transform.position = _1.position;
    }

    public void pass1()
    {
        GameObject.Find("Player").transform.position = _0.position;
    }
}
