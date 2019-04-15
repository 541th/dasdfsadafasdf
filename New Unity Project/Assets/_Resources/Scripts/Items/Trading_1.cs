using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trading_1 : MonoBehaviour
{
    [SerializeField] GameObject panel, buttons, confirmation, monets;
    [SerializeField] Sprite empty;
    int[] items = new int[3];
    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            items[i] = Random.Range(0, 3) * 100 + Random.Range(0, 4) * 10 + Random.Range(3, 5);

            while (true)
            {
                if (!itemsContains(items[i], i))
                    break;
                else
                    items[i] = Random.Range(0, 3) * 100 + Random.Range(0, 4) * 10 + Random.Range(0, 5);
            }

            buttons.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = ItemDatabase.getItemById(items[i]).sprite;
        }
    }

    bool itemsContains(int id, int exc)
    {
        for (int i = 0; i < 3; i++)
            if (items[i] == id && i != exc) return true;

        return false;
    }

    int choosenItemId;
    public void showConfirmation(int id)
    {
        choosenItemId = id;

        if (items[choosenItemId] != -1)
        {
            confirmation.SetActive(true);

            confirmation.transform.GetChild(0).GetChild(ItemDatabase.getItemType(items[choosenItemId]) == 3 ? 1 : 0).gameObject.SetActive(true);
            confirmation.transform.GetChild(0).GetChild(ItemDatabase.getItemType(items[choosenItemId]) == 3 ? 0 : 1).gameObject.SetActive(false);

            int v1 = ItemDatabase.getItemById(items[choosenItemId]).value;

            ItemDatabase.Item taked = FindObjectOfType<InventoryManager>().takedItems[ItemDatabase.getCharacterType(items[choosenItemId]), ItemDatabase.getItemType(items[choosenItemId])];

            int v2 = (taked != null) ? taked.value : 0;

            confirmation.transform.GetChild(1).GetComponent<Text>().text = ((v1 - v2) > 0 ? "+" : "-") + (Mathf.Abs(v1 - v2));

            confirmation.transform.GetChild(1).GetComponent<Text>().color = new Color((v1 - v2) > 0 ? 0 : 250, (v1 - v2) > 0 ? 250 : 0, 0);

            monets.transform.GetChild(2).GetComponent<Text>().text = LanguageLines.getLine(20) + "\n\n" + v1 * 120;
            monets.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            monets.transform.GetChild(2).gameObject.SetActive(false);
            confirmation.SetActive(false);
        }
    }

    public void buy()
    {
        if (FindObjectOfType<UIManager>().subMonets(ItemDatabase.getItemById(items[choosenItemId]).value * 120))
        {
            monets.transform.GetChild(1).GetComponent<Text>().text = FindObjectOfType<UIManager>().monetsAmount + "";

            FindObjectOfType<InventoryManager>().takeItem(items[choosenItemId]);
            items[choosenItemId] = -1;

            buttons.transform.GetChild(choosenItemId).GetChild(0).GetComponent<Image>().sprite = empty;

            monets.transform.GetChild(2).gameObject.SetActive(false);
            confirmation.SetActive(false);
        }
    }

    public void showPanel()
    {
        FindObjectOfType<UIManager>().setAllItems(false);
        panel.SetActive(true);
        monets.transform.GetChild(1).GetComponent<Text>().text = FindObjectOfType<UIManager>().monetsAmount + "";

        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void hidePanel()
    {
        FindObjectOfType<UIManager>().setAllItems(true);
        panel.SetActive(false);
        confirmation.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        monets.transform.GetChild(2).gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
