using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject items, throwed;
    public ItemDatabase.Item[,] takedItems = new ItemDatabase.Item[3, 4];

    private void Start()
    {
    }

    public void takeItem(int id)
    {
        takedItems[ItemDatabase.getCharacterType(id), ItemDatabase.getItemType(id)] = ItemDatabase.getItemById(id);
    }

    public void putOn(int id)
    {
        if (takedItems[ItemDatabase.getCharacterType(id), ItemDatabase.getItemType(id)] != null)
        {
            GameObject _t = Instantiate(throwed, GameObject.Find("Player").transform.position, Quaternion.identity);
            _t.transform.GetChild(1).GetComponent<ThrowedItem>().id = takedItems[ItemDatabase.getCharacterType(id), ItemDatabase.getItemType(id)].id;
        }

        takeItem(id);
    }
}
