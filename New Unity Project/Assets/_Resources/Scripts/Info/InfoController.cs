using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    [SerializeField] GameObject perksPanel, perksInfo;
    static PlayerMovement pm;
    public static Perks[] perks = new Perks[15];

    public static float[] sV = new float[3];

    public void showPanel(int id)
    {
        setExpSV(id);

        for (int i = 0; i < 15; i++)
            perks[i] = new Perks();

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
    string adding_0;
    float value;
    public void showPerksInfo(string desc, string adding_0, string adding_1, bool isSkill, int id, Transform _t, float value)
    {
        this.id = id;
        this.adding_0 = adding_0;
        this.value = value;
        this._t = _t;
        perksInfo.SetActive(true);

        if (!isSkill)
        {
            perksInfo.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            perksInfo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "lvl. " + perks[id - 16].lvl;
            perksInfo.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            perksInfo.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            perksInfo.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        }
        else
        for (int i = 0; i < 3; i++)
            if (perksPanel.transform.GetChild(i).gameObject.activeSelf)
            {
                perksInfo.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                perksInfo.transform.GetChild(1).GetChild(1).gameObject.SetActive(FindObjectOfType<PlayerMovement>().playerType == i + 1 && isSkill);
                perksInfo.transform.GetChild(1).GetChild(2).gameObject.SetActive(FindObjectOfType<PlayerMovement>().playerType == i + 1 && isSkill);
                perksInfo.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
                break;
            }

        perksInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        perksInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = adding_0 + "" + (isSkill ? "" : adding_1);
    }

    Transform _t;
    public static int curSkill_0 = 0, curSkill_1 = 0;

    public void chooseSkill_0()
    {
        FindObjectOfType<UIManager>().setSkillIcon_0(_t.GetComponent<Image>());
        perksInfo.SetActive(false);
        curSkill_0 = id;

        if (curSkill_0 == curSkill_1) curSkill_1 = 0;
    }

    public void chooseSkill_1()
    {
        FindObjectOfType<UIManager>().setSkillIcon_1(_t.GetComponent<Image>());
        perksInfo.SetActive(false);
        curSkill_1 = id;

        if (curSkill_0 == curSkill_1) curSkill_0 = 0;
    }

    public void upPerk()
    {
        if (perks[id - 16].lvl < 5)
        {
            perks[id - 16].lvl++;
            perksInfo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "lvl. " + perks[id - 16].lvl;
            perks[id - 16].value = value * perks[id - 16].lvl;
            perksInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = adding_0 + " " + (value * (perks[id - 16].lvl + 1));
        }
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

    IEnumerator hurricane()
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
                    GameObject hurricane = Instantiate(Resources.Load("Prefabs/Hurricane_0_Obj") as GameObject);
                    hurricane.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

                    int count = Random.Range(10, 20);

                    for (int i = 0; i < count; i++)
                    {
                        GameObject hurricane_0 = Instantiate(Resources.Load("Prefabs/Effects/Hurricane_0_Effect") as GameObject);
                        hurricane_0.transform.SetParent(hurricane.transform);
                        hurricane_0.transform.position = new Vector3(hurricane.transform.position.x + Random.Range(-4f, 4f), hurricane.transform.position.y + Random.Range(-4f, 4f), -0.15f);
                    }

                    Destroy(hurricane, 10);
                    yield break;
                }
            }

            yield return null;
        }
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

    public void useSkill_0()
    {
        switch (curSkill_0)
        {
            case 1:
                FindObjectOfType<PlayerAttack_Warrior>().skill_0();
                FindObjectOfType<UIManager>().setSkillCD_0(4);
                break;
            case 2:
                FindObjectOfType<PlayerAttack_Warrior>().skill_1();
                FindObjectOfType<UIManager>().setSkillCD_0(14);
                break;
            case 3:
                FindObjectOfType<PlayerAttack_Warrior>().skill_2();
                FindObjectOfType<UIManager>().setSkillCD_0(12);
                break;
            case 4:
                StartCoroutine(buildBanner());
                FindObjectOfType<UIManager>().setSkillCD_0(38);
                break;
            case 5:
                FindObjectOfType<PlayerMovement>().skill_4();
                FindObjectOfType<UIManager>().setSkillCD_0(18);
                break;
            case 6:
                FindObjectOfType<PlayerMovement>().skill_5();
                FindObjectOfType<UIManager>().setSkillCD_0(30);
                break;
            case 7:
                FindObjectOfType<PlayerAttack_Archer>().isHotShot = true;
                FindObjectOfType<UIManager>().setSkillCD_0(10);
                break;
            case 8:
                StartCoroutine(throwNet());
                FindObjectOfType<UIManager>().setSkillCD_0(16);
                break;
            case 9:
                StartCoroutine(roots());
                FindObjectOfType<UIManager>().setSkillCD_0(28);
                break;
            case 10:
                FindObjectOfType<PlayerAttack_Archer>().skill_8();
                FindObjectOfType<UIManager>().setSkillCD_0(18);
                break;
            case 11:
                StartCoroutine(pentagram_F());
                FindObjectOfType<UIManager>().setSkillCD_0(17);
                break;
            case 12:
                StartCoroutine(pentagram_I());
                FindObjectOfType<UIManager>().setSkillCD_0(14);
                break;
            case 13:
                FindObjectOfType<PlayerAttack_Archer>().isLightning = true;
                FindObjectOfType<UIManager>().setSkillCD_0(13);
                break;
            case 14:
                FindObjectOfType<PlayerHP>().createForceField();
                FindObjectOfType<UIManager>().setSkillCD_0(28);
                break;
            case 15:
                StartCoroutine(hurricane());
                FindObjectOfType<UIManager>().setSkillCD_0(30);
                break;
        }
    }

    public void useSkill_1()
    {
        switch (curSkill_1)
        {
            case 1:
                FindObjectOfType<PlayerAttack_Warrior>().skill_0();
                FindObjectOfType<UIManager>().setSkillCD_1(4);
                break;
            case 2:
                FindObjectOfType<PlayerAttack_Warrior>().skill_1();
                FindObjectOfType<UIManager>().setSkillCD_1(14);
                break;
            case 3:
                FindObjectOfType<PlayerAttack_Warrior>().skill_2();
                FindObjectOfType<UIManager>().setSkillCD_1(12);
                break;
            case 4:
                StartCoroutine(buildBanner());
                FindObjectOfType<UIManager>().setSkillCD_1(38);
                break;
            case 5:
                FindObjectOfType<PlayerMovement>().skill_4();
                FindObjectOfType<UIManager>().setSkillCD_1(18);
                break;
            case 6:
                FindObjectOfType<PlayerMovement>().skill_5();
                FindObjectOfType<UIManager>().setSkillCD_1(30);
                break;
            case 7:
                FindObjectOfType<PlayerAttack_Archer>().isHotShot = true;
                FindObjectOfType<UIManager>().setSkillCD_1(10);
                break;
            case 8:
                StartCoroutine(throwNet());
                FindObjectOfType<UIManager>().setSkillCD_1(16);
                break;
            case 9:
                StartCoroutine(roots());
                FindObjectOfType<UIManager>().setSkillCD_1(28);
                break;
            case 10:
                FindObjectOfType<PlayerAttack_Archer>().skill_8();
                FindObjectOfType<UIManager>().setSkillCD_1(18);
                break;
            case 11:
                StartCoroutine(pentagram_F());
                FindObjectOfType<UIManager>().setSkillCD_1(17);
                break;
            case 12:
                StartCoroutine(pentagram_I());
                FindObjectOfType<UIManager>().setSkillCD_1(14);
                break;
            case 13:
                FindObjectOfType<PlayerAttack_Archer>().isLightning = true;
                FindObjectOfType<UIManager>().setSkillCD_1(13);
                break;
            case 14:
                FindObjectOfType<PlayerHP>().createForceField();
                FindObjectOfType<UIManager>().setSkillCD_1(28);
                break;
            case 15:
                StartCoroutine(hurricane());
                FindObjectOfType<UIManager>().setSkillCD_1(30);
                break;
        }
    }

    public class Perks
    {
        public int lvl;
        public float value;
    }
}
