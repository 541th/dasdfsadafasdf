using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<GameObject> items;
    [SerializeField] GameObject glideButton;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void startAttack()
    {
        player.GetComponent<PlayerAttack_Warrior>().startAttack();
    }

    public void stopAttack()
    {
        player.GetComponent<PlayerAttack_Warrior>().stopAttack();
    }

    public void glide()
    {
        player.GetComponent<PlayerMovement>().glide();
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

    public void setAllItems(bool value)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == "Skill_0")
            {
                items[i].SetActive(value && InfoController.curSkill_0 != 0);
                continue;
            }

            if (items[i].name == "Skill_1")
            {
                items[i].SetActive(value && InfoController.curSkill_1 != 0);
                continue;
            }

            items[i].SetActive(value);
        }

        setGameButtons(value);
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
}
