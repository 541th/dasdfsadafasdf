using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderAtStart : MonoBehaviour
{
    [SerializeField] bool isGrass;

    void Start()
    {
        if (isGrass)
            GetComponent<Animator>().speed = Random.Range(0.4f, 1.1f);
        GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * SortingOrder.mult);
    }
}
