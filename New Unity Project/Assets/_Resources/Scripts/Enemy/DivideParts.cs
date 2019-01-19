using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideParts : MonoBehaviour
{
    [SerializeField] GameObject[] parts;
    [SerializeField] GameObject expl;

    public void divide()
    {
        if (expl != null)
        {
            GameObject go = Instantiate(expl, transform.position, Quaternion.identity);
            Destroy(go, .6f);
        }
        
        for (int i = 0; i < parts.Length; i++)
        {
            GameObject enemy = Instantiate(parts[i], transform.position, Quaternion.identity);
        }
    }
}