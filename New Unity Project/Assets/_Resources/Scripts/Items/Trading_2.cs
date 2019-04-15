using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trading_2 : MonoBehaviour
{
    [SerializeField] GameObject panel, buttons, buyButton, monets;
    int amount;
    private void Start()
    {
        amount = Random.Range(6, 20);
        buttons.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "1";
    }

    public void OnValueChanged()
    {
        monets.transform.GetChild(2).GetComponent<Text>().text = LanguageLines.getLine(20) + "\n\n" + buttons.transform.GetChild(0).GetComponent<Slider>().value * 80;
        buttons.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = buttons.transform.GetChild(0).GetComponent<Slider>().value + "";
    }

    public void buy()
    {
        if (FindObjectOfType<UIManager>().subMonets((int)buttons.transform.GetChild(0).GetComponent<Slider>().value * 80))
        {
            amount -= (int)buttons.transform.GetChild(0).GetComponent<Slider>().value;
            monets.transform.GetChild(1).GetComponent<Text>().text = FindObjectOfType<UIManager>().monetsAmount + "";

            FindObjectOfType<UIManager>().addPotion((int)buttons.transform.GetChild(0).GetComponent<Slider>().value);
            
            buttons.transform.GetChild(0).GetComponent<Slider>().maxValue = amount;

            monets.transform.GetChild(2).GetComponent<Text>().text = LanguageLines.getLine(20) + "\n\n" + (buttons.transform.GetChild(0).GetComponent<Slider>().value * 80);
            buttons.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = buttons.transform.GetChild(0).GetComponent<Slider>().value +  "";
        }

        if (amount <= 0)
        {
            buttons.transform.GetChild(0).GetComponent<Slider>().interactable = false;
            buyButton.SetActive(false);
        }
    }

    public void showPanel()
    {
        buttons.transform.GetChild(0).GetComponent<Slider>().value = 1;
        buttons.transform.GetChild(0).GetComponent<Slider>().maxValue = amount;

        FindObjectOfType<UIManager>().setAllItems(false);
        panel.SetActive(true);
        monets.transform.GetChild(1).GetComponent<Text>().text = FindObjectOfType<UIManager>().monetsAmount + "";
        monets.transform.GetChild(2).GetComponent<Text>().text = LanguageLines.getLine(20) + "\n\n" + 80;

        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void hidePanel()
    {
        FindObjectOfType<UIManager>().setAllItems(true);
        panel.SetActive(false);

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
