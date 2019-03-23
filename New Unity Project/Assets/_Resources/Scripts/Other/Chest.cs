using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] GameObject monetsPrefab;
    [SerializeField] GameObject equipPrefab;

    private void OnDestroy()
    {
        int chanse = Random.Range(0, 12);

        if (chanse < 6)
        {
            GameObject monets = Instantiate(monetsPrefab, transform.position + new Vector3(0, 1), Quaternion.identity);
            monets.GetComponent<MonetsEffect>().monets = true;
        }
        else if (chanse < 10)
        {
            Instantiate(monetsPrefab, transform.position + new Vector3(0, 1), Quaternion.identity);
        }
        else
        {
            GameObject equip = Instantiate(equipPrefab, transform.position, Quaternion.identity);

            if (Random.Range(0, 10) <= 6)
            {
                int id = Random.Range(0, 3) * 100 + Random.Range(0, 4) * 10 + Random.Range(0, 3);
                equip.transform.GetChild(1).GetComponent<ThrowedItem>().id = id;
            }
            else
            {
                int id = Random.Range(0, 3) * 100 + Random.Range(0, 4) * 10 + Random.Range(3, 5);
                equip.transform.GetChild(1).GetComponent<ThrowedItem>().id = id;
            }
        }
    }
}
