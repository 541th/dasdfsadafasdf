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

        for (int i = 0; i < 3; i++)
            if (perksPanel.transform.GetChild(i).gameObject.activeSelf)
            {
                perksInfo.transform.GetChild(1).GetChild(1).gameObject.SetActive(FindObjectOfType<PlayerMovement>().playerType == i + 1 && isSkill);
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

    IEnumerator throwNet()
    {
        float timer = 4;
        yield return new WaitForSeconds(0.1f);

        if (pm == null)
            pm = FindObjectOfType<PlayerMovement>();

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (Time.timeScale == 1)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    GameObject net = Instantiate(Resources.Load("Prefabs/TrappingNet") as GameObject);
                    net.transform.position = pm.transform.position;

                    Vector2 to = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    while (Vector2.SqrMagnitude(net.transform.position - (Vector3)to) > 0.4f)
                    {
                        net.transform.position = Vector3.Lerp(net.transform.position, to, 4 * Time.deltaTime);
                        yield return null;
                    }

                    Destroy(net, 5);

                    yield break;
                }
            }

            yield return null;
        }
    }

    IEnumerator roots()
    {
        if (pm == null)
            pm = FindObjectOfType<PlayerMovement>();

        GameObject roots = Instantiate(Resources.Load("Prefabs/TrappingRoots") as GameObject);
        roots.transform.position = pm.transform.position;

        yield return new WaitForSeconds(6);

        roots.GetComponent<Animator>().SetTrigger("End");
        Destroy(roots, 1);
    }

    IEnumerator pentagram_F()
    {
        if (pm == null)
            pm = FindObjectOfType<PlayerMovement>();

        GameObject pentagram = Instantiate(Resources.Load("Prefabs/Effects/Pentagram_Fire") as GameObject);
        pentagram.transform.position = pm.transform.position;

        pm.dontMove = true;
        pm.dontAttack = true;

        yield return new WaitForSeconds(1f);

        pm.dontMove = false;
        pm.dontAttack = false;

        yield return new WaitForSeconds(.3f);

        Destroy(pentagram);
    }

    IEnumerator pentagram_I()
    {
        if (pm == null)
            pm = FindObjectOfType<PlayerMovement>();

        GameObject pentagram = Instantiate(Resources.Load("Prefabs/Effects/Pentagram_Ice") as GameObject);
        pentagram.transform.position = pm.transform.position;

        pm.dontMove = true;
        pm.dontAttack = true;

        yield return new WaitForSeconds(1f);

        pm.dontMove = false;
        pm.dontAttack = false;

        yield return new WaitForSeconds(.3f);

        Destroy(pentagram);
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
            case 6:
                FindObjectOfType<PlayerMovement>().skill_5();
                break;
            case 7:
                FindObjectOfType<PlayerAttack_Archer>().isHotShot = true;
                break;
            case 8:
                StartCoroutine(throwNet());
                break;
            case 9:
                StartCoroutine(roots());
                break;
            case 10:
                FindObjectOfType<PlayerAttack_Archer>().skill_8();
                break;
            case 11:
                StartCoroutine(pentagram_F());
                break;
            case 12:
                StartCoroutine(pentagram_I());
                break;
            case 13:
                FindObjectOfType<PlayerAttack_Archer>().isLightning = true;
                break;
        }
    }
}
