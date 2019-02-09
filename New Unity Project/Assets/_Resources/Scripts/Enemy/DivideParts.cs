using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideParts : MonoBehaviour
{
    [SerializeField] GameObject[] parts;
    [SerializeField] GameObject expl;
    [SerializeField] bool isRandom;

    public void divide()
    {
        if (expl != null)
        {
            GameObject go = Instantiate(expl, transform.position, Quaternion.identity);
            Destroy(go, .6f);
        }
        
        for (int i = 0; i < parts.Length; i++)
        {
            if (isRandom && Random.Range(0, 2) == 0) continue;

            GameObject enemy = Instantiate(parts[i], transform.position, Quaternion.identity);
        }
    }
}