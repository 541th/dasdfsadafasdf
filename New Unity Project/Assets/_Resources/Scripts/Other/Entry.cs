using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entry : MonoBehaviour
{
    [SerializeField] GameObject BG, chars, slider, curLvl;
    int curChar = 1;
    public int levelType;

    public void showPanel()
    {
        StartCoroutine(showing());
    }

    public void switchCharacter(int value)
    {
        if (value == curChar || changing) return;

        StartCoroutine(changeChar(value));
    }

    bool changing;
    IEnumerator changeChar(int value)
    {
        changing = true;
        float _a0 = 141f / 256f, _a1 = 30f / 256f;

        Image char0 = chars.transform.GetChild(value - 1).GetComponent<Image>();
        Image char1 = chars.transform.GetChild(curChar - 1).GetComponent<Image>();

        while (char1.color.a > _a1)
        {
            char0.color += new Color(0, 0, 0, Time.deltaTime);

            char1.color -= new Color(0, 0, 0, Time.deltaTime);

            if (char0.transform.GetChild(0).localScale.x < 1)
            {
                char0.transform.GetChild(0).localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
                char1.transform.GetChild(0).localScale -= new Vector3(Time.deltaTime, Time.deltaTime, 0);
            }

            if (char1.transform.GetChild(0).GetComponent<Image>().color.a > 128f / 256f)
            {
                char0.transform.GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime);
                char1.transform.GetChild(0).GetComponent<Image>().color -= new Color(0, 0, 0, Time.deltaTime);
            }

            yield return null;
        }

        char0.color = new Color(char0.color.r, char0.color.g, char0.color.b, _a0);
        char1.color = new Color(char1.color.r, char1.color.g, char1.color.b, _a1);
        char0.transform.GetChild(0).localScale = new Vector3(1, 1, 0);
        char1.transform.GetChild(0).localScale = new Vector3(0.7f, 0.7f, 0);
        char0.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        char1.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 128f / 256f);

        curChar = value;
        changing = false;
    }

    public void enterToTower()
    {
        StartCoroutine(loadingEvent());
    }

    IEnumerator loadingEvent()
    {
        GameObject blackScreen = FindObjectOfType<UIManager>().blackScreen;
        Image blackScreenImage = blackScreen.GetComponent<Image>();

        blackScreen.SetActive(true);

        while (blackScreenImage.color.a <= 1)
        {
            blackScreenImage.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        int levelToLoad = (int)slider.GetComponent<Slider>().value;

        if (levelToLoad == 5)
            levelType = 1;
        else if (levelToLoad == 10)
            levelType = 2;
        else if (levelToLoad == 15)
            levelType = 3;
        else if (levelToLoad == 20)
            levelType = 4;
        else if (levelToLoad == 25)
            levelType = 5;
        else 
            levelType = 0;

        PlayerPrefs.SetInt("LevelType", levelType);
        PlayerPrefs.SetInt("PlayerType", curChar);

        UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad + "");
    }

    IEnumerator showing()
    {
        changing = true;
        FindObjectOfType<UIManager>().setAllItems(false);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        float value = 200f / 256f;

        while (BG.GetComponent<Image>().color.a < value)
        {
            BG.GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        {
            float _a0 = 68f / 256f, _a1 = 109f / 256f, _a2 = 141f / 256f;

            GameObject tower = slider.transform.parent.gameObject;

            Image char00 = tower.transform.GetChild(0).GetComponent<Image>();
            Image char01 = tower.transform.GetChild(1).GetComponent<Image>();

            while (char00.color.a < _a0)
            {
                char00.color += new Color(0, 0, 0, Time.deltaTime * 4);
                char01.color += new Color(0, 0, 0, Time.deltaTime * 4);

                yield return null;
            }

            char00.color = new Color(1, 1, 1, _a0);
            char01.color = new Color(1, 1, 1, _a0);

            Image char10 = slider.transform.GetChild(0).GetComponent<Image>();
            Image char11 = slider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            Image char12 = slider.transform.GetChild(2).GetComponent<Image>();

            while (char10.color.a < _a1)
            {
                char10.color += new Color(0, 0, 0, Time.deltaTime * 4);
                char12.color += new Color(0, 0, 0, Time.deltaTime * 4);

                if (char11.color.a < _a2)
                    char11.color += new Color(0, 0, 0, Time.deltaTime * 4);

                yield return null;
            }

            char10.color = new Color(char10.color.r, char10.color.g, char10.color.b, _a1);
            char12.color = new Color(char10.color.r, char10.color.g, char10.color.b, _a1);
            char11.color = new Color(1, 1, 1, _a2);

            while (tower.transform.GetChild(3).GetComponent<Text>().color.a < 1)
            {
                tower.transform.GetChild(3).GetComponent<Text>().color += new Color(0, 0, 0, Time.deltaTime * 4);

                yield return null;
            }

            tower.transform.GetChild(3).GetComponent<Text>().color = new Color(1, 1, 1, 1);

            while (tower.transform.GetChild(4).GetChild(0).GetComponent<Image>().color.a < 1)
            {
                tower.transform.GetChild(4).GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime * 4);
                tower.transform.GetChild(4).GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime * 2);

                yield return null;
            }

            tower.transform.GetChild(4).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            tower.transform.GetChild(4).GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 1);
        }

        {
            float _a0 = 141f / 256f, _a1 = 30f / 256f, _a2 = 128f / 256f;

            Image char0 = chars.transform.GetChild(0).GetComponent<Image>();
            Image char1 = chars.transform.GetChild(1).GetComponent<Image>();
            Image char2 = chars.transform.GetChild(2).GetComponent<Image>();

            while (char0.color.a < _a0)
            {
                char0.color += new Color(0, 0, 0, Time.deltaTime * 4);

                if (char0.transform.GetChild(0).GetComponent<Image>().color.a < 1)
                    char0.transform.GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime * 4);

                yield return null;
            }

            char0.color = new Color(char0.color.r, char0.color.g, char0.color.b, _a0);
            char0.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);

            while (char1.color.a < _a1)
            {
                char1.color += new Color(0, 0, 0, Time.deltaTime);

                if (char1.transform.GetChild(0).GetComponent<Image>().color.a < _a2)
                    char1.transform.GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime);

                yield return null;
            }

            char1.color = new Color(char1.color.r, char1.color.g, char1.color.b, _a1);
            char1.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, _a2);

            while (char2.color.a < _a1)
            {
                char2.color += new Color(0, 0, 0, Time.deltaTime);

                if (char2.transform.GetChild(0).GetComponent<Image>().color.a < _a2)
                    char2.transform.GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime);

                yield return null;
            }

            char2.color = new Color(char2.color.r, char2.color.g, char2.color.b, _a1);
            char2.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, _a2);
        }

        changing = false;
    }

    public void onValueChanged()
    {
        curLvl.GetComponent<Text>().text = slider.GetComponent<Slider>().value.ToString();
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
