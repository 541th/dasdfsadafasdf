using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] float value;
    void Start()
    {
        GetComponent<Animator>().SetFloat("Blend", value);
    }
}
