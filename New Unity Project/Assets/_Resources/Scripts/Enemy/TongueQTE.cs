using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TongueQTE : MonoBehaviour
{
    int count;

    public void tap()
    {
        count++;
        if (count >= 4) transform.parent.GetComponentInChildren<Tongue>().destr(gameObject);
    }
}
