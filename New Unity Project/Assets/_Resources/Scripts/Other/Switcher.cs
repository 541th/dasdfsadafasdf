using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.transform.GetChild(0).gameObject.SetActive(false);
    }
}
