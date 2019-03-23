using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowedItem : MonoBehaviour
{
    public int id;

    public void take()
    {
        FindObjectOfType<InventoryManager>().putOn(id);

        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = ItemDatabase.getItemById(id).sprite;

        int v1 = ItemDatabase.getItemById(id).value;

        ItemDatabase.Item taked = FindObjectOfType<InventoryManager>().takedItems[ItemDatabase.getCharacterType(id), ItemDatabase.getItemType(id)];

        int v2 = (taked != null) ? taked.value : 0;

        transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = 
            ((v1 - v2) > 0 ? "+" : "-") + (Mathf.Abs(v1 - v2));

        transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().color = new Color((v1 - v2) > 0 ? 0 : 250, (v1 - v2) > 0 ? 250 : 0, 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
