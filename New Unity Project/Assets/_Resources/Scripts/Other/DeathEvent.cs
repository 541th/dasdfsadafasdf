using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEvent : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        transform.position = player.transform.position;
        player.SetActive(false);

        StartCoroutine(soul());

        //FindObjectOfType<PlayerHP>().setAllFull();
    }

    IEnumerator soul()
    {
        GameObject soulObj = transform.GetChild(1).gameObject;
        float timer = 0;

        while (timer < 1)
        {
            timer += Time.deltaTime;
            soulObj.transform.position += new Vector3(0, Time.deltaTime * 2);

            yield return null;
        }

        FindObjectOfType<DeathCanvas>().start();

        while (true)
        {
            soulObj.transform.position += new Vector3(0, Time.deltaTime * 2);

            yield return null;
        }
    }
}
