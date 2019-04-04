using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<GameObject> items;
    [SerializeField] GameObject glideButton, potions;
    public GameObject blackScreen;
    GameObject player;

    private void OnLevelWasLoaded(int level)
    {
        if (PlayerPrefs.GetInt("Continue") == 0)
        {
            saveData();
        }
    }

    private void OnDestroy()
    {
        saveData();
    }

    void saveData()
    {
        PlayerPrefs.SetInt("Monets", monetsAmount);
        PlayerPrefs.SetInt("Potions", potionsAmount);
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        updatePotionButtonValue();

        monetsAmount = PlayerPrefs.GetInt("Monets");
        potionsAmount = PlayerPrefs.GetInt("Potions");

        updateMonets();
        updatePotionButtonValue();
    }

    public void startAttack()
    {
        if (player == null) player = GameObject.Find("Player");
        player.GetComponent<PlayerAttack_Warrior>().startAttack();
    }

    public void stopAttack()
    {
        if (player == null) player = GameObject.Find("Player");
        player.GetComponent<PlayerAttack_Warrior>().stopAttack();
    }

    public void glide()
    {
        if (player == null) player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().glide();
    }

    [SerializeField] Sprite full, empty;
    [SerializeField] Text potionText;
    public int potionsAmount;
    void updatePotionButtonValue()
    {
        potionText.text = potionsAmount + "";

        potions.transform.GetChild(0).GetComponent<Image>().sprite = (potionsAmount > 0) ? full : empty;
    }

    public void potion()
    {
        if (potionsAmount > 0 && GameObject.Find("Player").GetComponent<PlayerMovement>().playerType != 0)
        {
            potionsAmount--;
            updatePotionButtonValue();
            FindObjectOfType<PlayerHP>().toHeal(30 + (10 * (int)(InfoController.perks[12].value)));
        }
    }

    [SerializeField] Text monetsText;
    public int monetsAmount;

    void updateMonets()
    {
        monetsText.text = "Монеты: " + monetsAmount;
    }

    public void addMonets(int value)
    {
        monetsAmount += value;

        updateMonets();
    }

    public bool subMonets(int value)
    {
        if (monetsAmount - value >= 0)
        {
            monetsAmount -= value;

            updateMonets();

            return true;
        }
        else
        {
            return false;
        }
    }

    public void addPotion(int value)
    {
        potionsAmount += value;

        updatePotionButtonValue();
    }

    public void glideFalse()
    {
        glideButton.GetComponent<Button>().interactable = false;
        glideButton.GetComponent<Image>().color = new Color(glideButton.GetComponent<Image>().color.r, glideButton.GetComponent<Image>().color.g, glideButton.GetComponent<Image>().color.b, 0.05f);
    }

    public void glideReturn()
    {
        StartCoroutine(returning());
    }

    IEnumerator returning()
    {
        while (glideButton.GetComponent<Image>().color.a <= 0.1f)
        {
            glideButton.GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        glideButton.GetComponent<Button>().interactable = true;
    }

    [SerializeField] GameObject gameMenu, infoCanvas;

    public void openMenu()
    {
        Time.timeScale = 0;
        gameMenu.SetActive(true);
        setAllItems(false);
    }

    bool tValue;
    public void setAllItems(bool value)
    {
        tValue = value;
        ThrowedItem[] _ti = FindObjectsOfType<ThrowedItem>();

        foreach (ThrowedItem item in _ti)
        {
            item.transform.GetChild(0).gameObject.SetActive(false);
        }

        Trading_0[] t = FindObjectsOfType<Trading_0>();

        foreach (Trading_0 item in t)
        {
            item.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (!value)
        {
            EndLevel[] el = FindObjectsOfType<EndLevel>();

            foreach (EndLevel item in el)
            {
                if (item.transform.childCount > 2)
                    item.allObjects.gameObject.SetActive(false);
            }
        }

        if (FindObjectOfType<Entry>() != null)
            FindObjectOfType<Entry>().transform.GetChild(0).gameObject.SetActive(value);

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == "Skill_0")
            {
                items[i].SetActive(tValue && InfoController.curSkill_0 != 0 && PlayerPrefs.GetInt("PlayerType") != 0 && PlayerPrefs.GetInt("PlayerType") == InfoController.curSkill_0id);
                continue;
            }

            if (items[i].name == "Skill_1")
            {
                items[i].SetActive(tValue && InfoController.curSkill_1 != 0 && PlayerPrefs.GetInt("PlayerType") != 0 && PlayerPrefs.GetInt("PlayerType") == InfoController.curSkill_1id);
                continue;
            }

            if (items[i].name == "Bars")
            {
                items[i].SetActive(tValue && PlayerPrefs.GetInt("PlayerType") != 0);
                continue;
            }

            items[i].SetActive(tValue);
        }

        setGameButtons(tValue);
    }

    public void closeMenu()
    {
        Time.timeScale = 1;
        gameMenu.SetActive(false);
        setAllItems(true);
    }

    public void openInfo()
    {
        gameMenu.SetActive(false);

        infoCanvas.GetComponent<InfoController>().open();
    }

    List<GameObject> gameButtons = new List<GameObject>();
    public void addItemToGameButtons(GameObject item)
    {
        gameButtons.Add(item);
    }

    public void removeItemFromGameButtons(GameObject item)
    {
        for (int i = 0; i < gameButtons.Count; i++)
            if (gameButtons[i].name == item.name)
                gameButtons.RemoveAt(i);
    }

    public void setGameButtons(bool value)
    {
        for (int i = 0; i < gameButtons.Count; i++)
            gameButtons[i].SetActive(value);
    }

    public void setSkillCD_0(float value)
    {
        StartCoroutine(skillCD_0(value));
    }

    public void setSkillCD_1(float value)
    {
        StartCoroutine(skillCD_1(value));
    }

    IEnumerator skillCD_0(float value)
    {
        GameObject skillButton = null;
        for (int i = 0; i < items.Count; i++)
            if (items[i].name == "Skill_0")
            {
                skillButton = items[i];
                break;
            }

        skillButton.GetComponent<Image>().color = new Color(skillButton.GetComponent<Image>().color.r, skillButton.GetComponent<Image>().color.g, skillButton.GetComponent<Image>().color.b, skillButton.GetComponent<Image>().color.a / 2);
        skillButton.GetComponent<Button>().interactable = false;

        yield return new WaitForSeconds(value);

        skillButton.GetComponent<Image>().color = new Color(skillButton.GetComponent<Image>().color.r, skillButton.GetComponent<Image>().color.g, skillButton.GetComponent<Image>().color.b, skillButton.GetComponent<Image>().color.a * 2);
        skillButton.GetComponent<Button>().interactable = true;
    }

    IEnumerator skillCD_1(float value)
    {
        GameObject skillButton = null;
        for (int i = 0; i < items.Count; i++)
            if (items[i].name == "Skill_1")
            {
                skillButton = items[i];
                break;
            }

        skillButton.GetComponent<Image>().color = new Color(skillButton.GetComponent<Image>().color.r, skillButton.GetComponent<Image>().color.g, skillButton.GetComponent<Image>().color.b, skillButton.GetComponent<Image>().color.a / 2);
        skillButton.GetComponent<Button>().interactable = false;

        yield return new WaitForSeconds(value);

        skillButton.GetComponent<Image>().color = new Color(skillButton.GetComponent<Image>().color.r, skillButton.GetComponent<Image>().color.g, skillButton.GetComponent<Image>().color.b, skillButton.GetComponent<Image>().color.a * 2);
        skillButton.GetComponent<Button>().interactable = true;
    }

    public void setSkillIcon_0(Image _i)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].name == "Skill_0")
            {
                items[i].transform.GetChild(0).GetComponent<Image>().sprite = _i.sprite;
                break;
            }
    }

    public void setSkillIcon_1(Image _i)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].name == "Skill_1")
            {
                items[i].transform.GetChild(0).GetComponent<Image>().sprite = _i.sprite;
                break;
            }
    }

    InfoController _ic;
    public void useSkill_0()
    {
        if (_ic == null) _ic = FindObjectOfType<InfoController>();

        _ic.useSkill_0();
    }

    public void useSkill_1()
    {
        if (_ic == null) _ic = FindObjectOfType<InfoController>();

        _ic.useSkill_1();
    }

    [SerializeField] GameObject bossHPBar;
    public void showBossHPBar(float value, float maxValue)
    {
        bossHPBar.GetComponent<Slider>().maxValue = maxValue;
        bossHPBar.GetComponent<Slider>().value = value;
        bossHPBar.SetActive(true);
    }

    public void goToMenu()
    {
        CamFollow.cameraExists = false;
        Time.timeScale = 1;
        Destroy(transform.parent.gameObject);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
