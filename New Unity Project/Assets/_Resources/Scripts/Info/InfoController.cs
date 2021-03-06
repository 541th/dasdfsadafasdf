﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    [SerializeField] GameObject perksPanel, perksInfo, infoPanel, items, monetsText;
    static PlayerMovement pm;
    public static Perks[] perks = new Perks[15];
    public static bool[] skills = new bool[15];

    public static float[] sV = new float[3];

    private void OnLevelWasLoaded(int level)
    {
        if (PlayerPrefs.GetInt("Continue") == 0)
            saveData();
    }

    private void OnDestroy()
    {
        saveData();
    }

    void saveData()
    {
        if (perks != null)
        {
            for (int i = 0; i < 15; i++)
            {
                PlayerPrefs.SetInt("Skill_" + i, skills[i] ? 1 : 0);
                PlayerPrefs.SetInt("PerkLvl_" + i, perks[i].lvl);
                PlayerPrefs.SetFloat("PerkValue_" + i, perks[i].value);
            }

            for (int i = 0; i < 3; i++)
                PlayerPrefs.SetFloat("SV_" + i, sV[i]);

            PlayerPrefs.SetInt("CurSkill_0", curSkill_0);
            PlayerPrefs.SetInt("CurSkill_1", curSkill_1);

            PlayerPrefs.SetInt("CurSkillId_0", curSkill_0id);
            PlayerPrefs.SetInt("CurSkillId_1", curSkill_1id);
        }
    }

    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            skills[i] = PlayerPrefs.GetInt("Skill_" + i) == 1;
            perks[i] = new Perks(PlayerPrefs.GetInt("PerkLvl_" + i), PlayerPrefs.GetFloat("PerkValue_" + i));
        }

        for (int i = 0; i < 3; i++)
            sV[i] = PlayerPrefs.GetFloat("SV_" + i);

        curSkill_0 = PlayerPrefs.GetInt("CurSkill_0");
        curSkill_1 = PlayerPrefs.GetInt("CurSkill_1");

        curSkill_0id = PlayerPrefs.GetInt("CurSkillId_0");
        curSkill_1id = PlayerPrefs.GetInt("CurSkillId_1");

        //PerkContainer[] perk = FindObjectsOfType<PerkContainer>();

        if (curSkill_0 != 0)
        {
            Transform t = null;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < perksPanel.transform.GetChild(i).GetChild(2).GetChild(0).childCount; j++)
                {
                    if (perksPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetChild(j).GetComponent<PerkContainer>() != null &&
                        perksPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetChild(j).GetComponent<PerkContainer>().id == curSkill_0)
                    {
                        t = perksPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetChild(j).GetChild(0);
                        break;
                    }
                }

            FindObjectOfType<UIManager>().setSkillIcon_0(t.GetComponent<Image>());
        }

        if (curSkill_1 != 0)
        {
            Transform t = null;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < perksPanel.transform.GetChild(i).GetChild(2).GetChild(0).childCount; j++)
                {
                    if (perksPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetChild(j).GetComponent<PerkContainer>() != null &&
                        perksPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetChild(j).GetComponent<PerkContainer>().id == curSkill_1)
                    {
                        t = perksPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetChild(j).GetChild(0);
                        break;
                    }
                }

            FindObjectOfType<UIManager>().setSkillIcon_1(t.GetComponent<Image>());
        }
    }

    public void showPanel(int id)
    {
        for (int i = 0; i < 3; i++)
            items.transform.GetChild(i).gameObject.SetActive(false);

        items.transform.GetChild(id).gameObject.SetActive(true);

        GameObject player = GameObject.Find("Player");
        infoPanel.transform.GetChild(0).GetComponent<Text>().text = "HP: " + FindObjectOfType<PlayerHP>().getCurHP() + "/" + FindObjectOfType<PlayerHP>().getMaxHP();
        infoPanel.transform.GetChild(1).GetComponent<Text>().text = "LVL: " + FindObjectOfType<PlayerExp>().curLvl;
        infoPanel.transform.GetChild(2).GetComponent<Text>().text = "Points: " + PlayerExp.points;

        monetsText.GetComponent<Text>().text = LanguageLines.getLine(26) + " " + FindObjectOfType<UIManager>().monetsAmount;

        setExpSV(id);
        setExpSV(id);
        perksInfo.SetActive(false);
        float slider = perksPanel.transform.GetChild(id).GetChild(1).GetChild(0).GetComponent<Slider>().value;
        for (int i = 0; i < 3; i++)
            perksPanel.transform.GetChild(i).gameObject.SetActive(i == id);

        Transform _t = perksPanel.transform.GetChild(id).GetChild(2).GetChild(0);
        for (int i = 0; i < _t.childCount; i++)
        {
            if (_t.GetChild(i).name != "stub")
            {
                Image _m0 = _t.GetChild(i).GetComponent<Image>();
                Image _m1 = _t.GetChild(i).GetChild(0).GetComponent<Image>();

                _m0.color = new Color(_m0.color.r, _m0.color.g, _m0.color.b, 1);
                _m1.color = new Color(_m1.color.r, _m1.color.g, _m1.color.b, 1);

                if (_t.GetChild(i).GetComponent<PerkContainer>().getLvl() > slider)
                {
                    _m0.color = new Color(_m0.color.r, _m0.color.g, _m0.color.b, _m0.color.a / 4);
                    _m1.color = new Color(_m1.color.r, _m1.color.g, _m1.color.b, _m1.color.a / 4);
                }
            }
        }

        InventoryManager _im = FindObjectOfType<InventoryManager>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                items.transform.GetChild(i).GetChild(j).GetChild(0).GetComponent<Image>().sprite = _im.takedItems[i, j] != null ? _im.takedItems[i, j].sprite : empty;
            }
        }
    }

    [SerializeField] Sprite empty;

    public void setExpSV(int id)
    {
        perksPanel.transform.GetChild(id).GetChild(1).GetChild(0).GetComponent<Slider>().value = sV[id];
    }

    public void open()
    {
        showPanel(Mathf.Clamp(FindObjectOfType<PlayerMovement>().playerType - 1, 0, 4));
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
    public void showPerksInfo(string desc, string adding_0, string adding_1, bool isSkill, int id, Transform _t, float value, int lvl)
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
            perksInfo.transform.GetChild(1).GetChild(3).gameObject.SetActive(
                (FindObjectOfType<PlayerMovement>().playerType == getCurPanelId() + 1 || FindObjectOfType<PlayerMovement>().playerType == 0) && 
                PlayerExp.points > 0 && 
                lvl <= perksPanel.transform.GetChild(getCurPanelId()).GetChild(1).GetChild(0).GetComponent<Slider>().value &&
                perks[id - 16].lvl < 5);
            perksInfo.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
        }
        else
        {
            perksInfo.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);

            if (skills[id - 1] && lvl <= perksPanel.transform.GetChild(getCurPanelId()).GetChild(1).GetChild(0).GetComponent<Slider>().value)
            {
                perksInfo.transform.GetChild(1).GetChild(1).gameObject.SetActive((FindObjectOfType<PlayerMovement>().playerType == getCurPanelId() + 1
                || FindObjectOfType<PlayerMovement>().playerType == 0) && isSkill);
                perksInfo.transform.GetChild(1).GetChild(2).gameObject.SetActive((FindObjectOfType<PlayerMovement>().playerType == getCurPanelId() + 1
                || FindObjectOfType<PlayerMovement>().playerType == 0) && isSkill);
                perksInfo.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
            }
            else
            {
                perksInfo.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                perksInfo.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                perksInfo.transform.GetChild(1).GetChild(4).gameObject.SetActive((FindObjectOfType<PlayerMovement>().playerType == getCurPanelId() + 1
                || FindObjectOfType<PlayerMovement>().playerType == 0) && isSkill && lvl <= perksPanel.transform.GetChild(getCurPanelId()).GetChild(1).GetChild(0).GetComponent<Slider>().value);
            }

            if (PlayerExp.points == 0)
                perksInfo.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
            perksInfo.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        }

        perksInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        perksInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = adding_0 + "" + (isSkill ? "" : adding_1);
    }

    public int getCurPanelId()
    {
        for (int i = 0; i < 3; i++)
            if (perksPanel.transform.GetChild(i).gameObject.activeSelf)
                return i;

        return 0;
    }

    Transform _t;
    public static int curSkill_0 = 0, curSkill_1 = 0, curSkill_0id, curSkill_1id;

    public void chooseSkill_0()
    {
        FindObjectOfType<UIManager>().setSkillIcon_0(_t.GetComponent<Image>());
        perksInfo.SetActive(false);
        curSkill_0 = id;
        curSkill_0id = FindObjectOfType<PlayerMovement>().playerType;
        //PlayerPrefs.SetInt("CurSkill_0id", PlayerPrefs.GetInt("PlayerType"));

        if (curSkill_0 == curSkill_1) curSkill_1 = 0;
    }

    public void chooseSkill_1()
    {
        FindObjectOfType<UIManager>().setSkillIcon_1(_t.GetComponent<Image>());
        perksInfo.SetActive(false);
        curSkill_1 = id;
        curSkill_1id = FindObjectOfType<PlayerMovement>().playerType;
        //PlayerPrefs.SetInt("CurSkill_1id", PlayerPrefs.GetInt("PlayerType"));

        if (curSkill_0 == curSkill_1) curSkill_0 = 0;
    }

    public void upSkill()
    {
        if (PlayerExp.points > 0)
        {
            perksInfo.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            perksInfo.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            perksInfo.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
            skills[id - 1] = true;

            PlayerExp.points--;
            infoPanel.transform.GetChild(2).GetComponent<Text>().text = "Points: " + PlayerExp.points;
        }
    }

    public void upPerk()
    {
        int _id = id - 16;
        
        if (perks[_id].lvl < 5 && PlayerExp.points > 0)
        {
            perks[_id].lvl++;
            perks[_id].value = value * perks[_id].lvl;
            perksInfo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "lvl. " + perks[_id].lvl;
            perksInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = adding_0 + " " + (value * (perks[_id].lvl + 1));


            GameObject player = GameObject.Find("Player");
            if (_id == 0 && player.GetComponent<PlayerMovement>().playerType == 1) FindObjectOfType<PlayerHP>().updateMaxHP();
            else if (_id == 5 && player.GetComponent<PlayerMovement>().playerType == 2) player.GetComponent<PlayerMovement>().updadeMS();
            else if (_id == 6 && player.GetComponent<PlayerMovement>().playerType == 2) FindObjectOfType<CamFollow>().updateCamSize();
        }

        PlayerExp.points--;
        infoPanel.transform.GetChild(2).GetComponent<Text>().text = "Points: " + PlayerExp.points;

        if (PlayerExp.points == 0)
        {
            perksInfo.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        }

        if (perks[_id].lvl == 5)
        {
            perksInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = adding_0 + " " + (value * (perks[_id].lvl + 1));
            perksInfo.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        }
    }

    /*
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
    */

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
                if (Input.GetMouseButton(0))
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

        pm.dontMove = true;
        pm.dontAttack = true;
        pm.transform.GetChild(1).gameObject.SetActive(false);
        pm.transform.GetChild(0).GetComponent<Animator>().SetTrigger("skill");

        yield return new WaitForSeconds(.6f);

        pm.dontMove = false;
        pm.dontAttack = false;
        pm.transform.GetChild(1).gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

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

        pm.GetComponent<PlayerAttack_Archer>().canAttack = false;

        pm.dontMove = true;
        pm.dontAttack = true;
        pm.transform.GetChild(1).gameObject.SetActive(false);
        pm.transform.GetChild(0).GetComponent<Animator>().SetTrigger("PentagramFire");

        yield return new WaitForSeconds(1f);

        pm.dontMove = false;
        pm.dontAttack = false;
        pm.transform.GetChild(1).gameObject.SetActive(true);

        pm.GetComponent<PlayerAttack_Archer>().canAttack = true;

        yield return new WaitForSeconds(.3f);

        Destroy(pentagram);
    }

    IEnumerator pentagram_I()
    {
        if (pm == null)
            pm = FindObjectOfType<PlayerMovement>();

        GameObject pentagram = Instantiate(Resources.Load("Prefabs/Effects/Pentagram_Ice") as GameObject);
        pentagram.transform.position = pm.transform.position;

        pm.GetComponent<PlayerAttack_Archer>().canAttack = false;

        pm.dontMove = true;
        pm.dontAttack = true;
        pm.transform.GetChild(1).gameObject.SetActive(false);
        pm.transform.GetChild(0).GetComponent<Animator>().SetTrigger("PentagramIce");

        yield return new WaitForSeconds(1f);

        pm.dontMove = false;
        pm.dontAttack = false;
        pm.transform.GetChild(1).gameObject.SetActive(true);

        pm.GetComponent<PlayerAttack_Archer>().canAttack = false;

        yield return new WaitForSeconds(.3f);

        Destroy(pentagram);
    }

    IEnumerator skill_2()
    {
        if (pm == null)
            pm = FindObjectOfType<PlayerMovement>();

        GameObject pentagram = Instantiate(Resources.Load("Prefabs/Effects/MagArrowExpl_0") as GameObject);
        pentagram.transform.position = pm.transform.position;

        pm.dontMove = true;
        pm.dontAttack = true;
        pm.transform.GetChild(0).GetComponent<Animator>().SetTrigger("skill_2");

        yield return new WaitForSeconds(.4f);

        pm.dontMove = false;
        pm.dontAttack = false;

        yield return new WaitForSeconds(.3f);

        Destroy(pentagram);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(pentagram_F());
        }
    }

    public void useSkill_0()
    {
        if (FindObjectOfType<PlayerMovement>().dontMove) return;

        switch (curSkill_0)
        {
            case 1:
                FindObjectOfType<PlayerAttack_Warrior>().skill_0();
                FindObjectOfType<UIManager>().setSkillCD_0(4);
                break;
            case 2:
                FindObjectOfType<PlayerAttack_Warrior>().skill_1();
                FindObjectOfType<UIManager>().setSkillCD_0(10);
                break;
            case 3:
                StartCoroutine(skill_2());
                FindObjectOfType<UIManager>().setSkillCD_0(12);
                break;
            case 4:
                GameObject banner = Instantiate(Resources.Load("Prefabs/Banner") as GameObject);
                banner.transform.position = GameObject.Find("Player").transform.position;
                Destroy(banner, 10);
                //StartCoroutine(buildBanner());
                FindObjectOfType<UIManager>().setSkillCD_0(20);
                break;
            case 5:
                FindObjectOfType<PlayerMovement>().skill_4();
                FindObjectOfType<UIManager>().setSkillCD_0(16);
                break;
            case 6:
                FindObjectOfType<PlayerMovement>().skill_5();
                FindObjectOfType<UIManager>().setSkillCD_0(20);
                break;
            case 7:
                FindObjectOfType<PlayerAttack_Archer>().isHotShot = true;
                FindObjectOfType<UIManager>().setSkillCD_0(10);
                break;
            case 8:
                StartCoroutine(throwNet());
                FindObjectOfType<UIManager>().setSkillCD_0(14);
                break;
            case 9:
                StartCoroutine(roots());
                FindObjectOfType<UIManager>().setSkillCD_0(20);
                break;
            case 10:
                FindObjectOfType<PlayerAttack_Archer>().skill_8();
                FindObjectOfType<UIManager>().setSkillCD_0(14);
                break;
            case 11:
                StartCoroutine(pentagram_F());
                FindObjectOfType<UIManager>().setSkillCD_0(12);
                break;
            case 12:
                StartCoroutine(pentagram_I());
                FindObjectOfType<UIManager>().setSkillCD_0(12);
                break;
            case 13:
                FindObjectOfType<PlayerAttack_Archer>().isLightning = true;
                FindObjectOfType<UIManager>().setSkillCD_0(7);
                break;
            case 14:
                FindObjectOfType<PlayerHP>().createForceField();
                FindObjectOfType<UIManager>().setSkillCD_0(20);
                break;
            case 15:
                StartCoroutine(hurricane());
                FindObjectOfType<UIManager>().setSkillCD_0(16);
                break;
        }
    }

    public void useSkill_1()
    {
        if (FindObjectOfType<PlayerMovement>().dontMove) return;

        switch (curSkill_1)
        {
            case 1:
                FindObjectOfType<PlayerAttack_Warrior>().skill_0();
                FindObjectOfType<UIManager>().setSkillCD_1(4);
                break;
            case 2:
                FindObjectOfType<PlayerAttack_Warrior>().skill_1();
                FindObjectOfType<UIManager>().setSkillCD_1(10);
                break;
            case 3:
                StartCoroutine(skill_2());
                FindObjectOfType<UIManager>().setSkillCD_1(12);
                break;
            case 4:
                GameObject banner = Instantiate(Resources.Load("Prefabs/Banner") as GameObject);
                banner.transform.position = GameObject.Find("Player").transform.position;
                Destroy(banner, 10);
                //StartCoroutine(buildBanner());
                FindObjectOfType<UIManager>().setSkillCD_1(20);
                break;
            case 5:
                FindObjectOfType<PlayerMovement>().skill_4();
                FindObjectOfType<UIManager>().setSkillCD_1(16);
                break;
            case 6:
                FindObjectOfType<PlayerMovement>().skill_5();
                FindObjectOfType<UIManager>().setSkillCD_1(20);
                break;
            case 7:
                FindObjectOfType<PlayerAttack_Archer>().isHotShot = true;
                FindObjectOfType<UIManager>().setSkillCD_1(10);
                break;
            case 8:
                StartCoroutine(throwNet());
                FindObjectOfType<UIManager>().setSkillCD_1(14);
                break;
            case 9:
                StartCoroutine(roots());
                FindObjectOfType<UIManager>().setSkillCD_1(20);
                break;
            case 10:
                FindObjectOfType<PlayerAttack_Archer>().skill_8();
                FindObjectOfType<UIManager>().setSkillCD_1(14);
                break;
            case 11:
                StartCoroutine(pentagram_F());
                FindObjectOfType<UIManager>().setSkillCD_1(12);
                break;
            case 12:
                StartCoroutine(pentagram_I());
                FindObjectOfType<UIManager>().setSkillCD_1(12);
                break;
            case 13:
                FindObjectOfType<PlayerAttack_Archer>().isLightning = true;
                FindObjectOfType<UIManager>().setSkillCD_1(7);
                break;
            case 14:
                FindObjectOfType<PlayerHP>().createForceField();
                FindObjectOfType<UIManager>().setSkillCD_1(20);
                break;
            case 15:
                StartCoroutine(hurricane());
                FindObjectOfType<UIManager>().setSkillCD_1(16);
                break;
        }
    }

    public class Perks
    {
        public int lvl;
        public float value;

        public Perks(int lvl, float value)
        {
            this.lvl = lvl;
            this.value = value;
        }
    }
}
