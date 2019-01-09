using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    [SerializeField] GameObject perksPanel, perksInfo;
    static PlayerMovement pm;

    public static float[] sV = new float[3];

    private void Start()
    {
        showPanel(0);
        setExpSV(0);
    }

    public void showPanel(int id)
    {
        setExpSV(id);

        for (int i = 0; i < 3; i++)
            perksPanel.transform.GetChild(i).gameObject.SetActive(i == id);
    }

    public void setExpSV(int id)
    {
        perksPanel.transform.GetChild(id).GetChild(1).GetChild(0).GetComponent<Slider>().value = sV[id];
    }

    public void close()
    {
        gameObject.SetActive(false);
        FindObjectOfType<UIManager>().setAllItems(true);
    }

    public static void addExp(float value)
    {
        if (pm == null)
            pm = FindObjectOfType<PlayerMovement>();

        sV[pm.playerType - 1] += value;
    }

    public void showPerksInfo(string desc, string adding)
    {
        perksInfo.SetActive(true);
        perksInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        perksInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = adding;
    }
}
