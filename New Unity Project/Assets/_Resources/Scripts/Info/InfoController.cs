using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    [SerializeField] GameObject perksPanel, perksInfo;
    static PlayerMovement pm;

    public static float[] sV = new float[3];

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

    public void open()
    {
        showPanel(0);
        setExpSV(0);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void close()
    {
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
        FindObjectOfType<UIManager>().setAllItems(true);
    }

    public static void addExp(float value)
    {
        if (pm == null)
            pm = FindObjectOfType<PlayerMovement>();

        sV[pm.playerType - 1] += value;
    }

    int id;
    public void showPerksInfo(string desc, string adding, bool isSkill, int id)
    {
        this.id = id;
        perksInfo.SetActive(true);
        perksInfo.transform.GetChild(1).GetChild(1).gameObject.SetActive(isSkill);

        for (int i = 0; i < 3; i++)
            if (perksPanel.transform.GetChild(i).gameObject.activeSelf)
            {
                perksInfo.transform.GetChild(1).GetChild(1).gameObject.SetActive(FindObjectOfType<PlayerMovement>().playerType == i + 1);
                break;
            }

        perksInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        perksInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = adding;
    }

    public static int curSkill = 0;

    public void chooseSkill()
    {
        perksInfo.SetActive(false);
        curSkill = id;
    }
    
    IEnumerator buildBanner()
    {
        float timer = 4;
        yield return new WaitForSeconds(0.1f);

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (Time.timeScale == 1)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    GameObject banner = Instantiate(Resources.Load("Prefabs/Banner") as GameObject);
                    banner.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
                    Destroy(banner, 10);
                    yield break;
                }
            }

            yield return null;
        }
    }

    public void useSkill()
    {
        switch (curSkill)
        {
            case 1:
                FindObjectOfType<PlayerAttack_Warrior>().skill_0();
                break;
            case 2:
                FindObjectOfType<PlayerAttack_Warrior>().skill_1();
                break;
            case 3:
                FindObjectOfType<PlayerAttack_Warrior>().skill_2();
                break;
            case 4:
                StartCoroutine(buildBanner());
                break;
            case 5:
                FindObjectOfType<PlayerMovement>().skill_4();
                break;
        }
    }
}
