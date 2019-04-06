using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] totemType;
    [SerializeField] Sprite[] sprites;
    [SerializeField] int levelType;

    void Start()
    {
        GameObject totem = Instantiate(totemType[Random.Range(0, totemType.Length)], transform);
        totem.transform.position = transform.position;

        totem.GetComponent<SpriteRenderer>().sprite = sprites[levelType];
    }
}
