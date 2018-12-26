using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float ms;
    Transform _t;
    
    void Start()
    {
        _t = transform;
    }

    float h, v;
    void Update()
    {
        h = CnInputManager.GetAxis("Horizontal");
        v = CnInputManager.GetAxis("Vertical");
        _t.position += new Vector3(h * ms * Time.deltaTime, v * ms * Time.deltaTime, 0);
    }
}
