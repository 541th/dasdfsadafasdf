using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderAtStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * SortingOrder.mult);
    }
}
