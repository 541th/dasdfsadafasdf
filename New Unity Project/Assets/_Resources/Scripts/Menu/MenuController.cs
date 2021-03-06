﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject newGameAsking, continueButton, BG, buttons, pressOnScreen, languageButton;

    void Start()
    {
        continueButton.SetActive(PlayerPrefs.GetInt("Started") == 1);
        buttons.SetActive(false);
        pressOnScreen.SetActive(true);

        if (Application.systemLanguage.ToString() == "Russian" && PlayerPrefs.GetInt("LanguageSetted") == 0)
            PlayerPrefs.SetInt("CurLanguage", 1);

        PlayerPrefs.SetInt("LanguageSetted", 1);

        languageButton.GetComponent<Image>().sprite = PlayerPrefs.GetInt("CurLanguage") == 0 ? flag_eng : flag_ru;

        foreach (TextLanguageChanger item in FindObjectsOfType<TextLanguageChanger>())
        {
            item.change();
        }
    }

    [SerializeField] Sprite flag_ru, flag_eng;
    public void changeLanguage()
    {
        PlayerPrefs.SetInt("CurLanguage", PlayerPrefs.GetInt("CurLanguage") == 0 ? 1 : 0);

        languageButton.GetComponent<Image>().sprite = PlayerPrefs.GetInt("CurLanguage") == 0 ? flag_eng : flag_ru;

        foreach (TextLanguageChanger item in FindObjectsOfType<TextLanguageChanger>())
        {
            item.change();
        }
    }

    public void continueGame()
    {
        PlayerPrefs.SetInt("Continue", 1);
        PlayerPrefs.SetInt("PlayerType", 0);

        StartCoroutine(loadCity());
    }

    public void pressed()
    {
        StartCoroutine(HidePressOnScreen());
    }

    IEnumerator HidePressOnScreen()
    {
        pressOnScreen.transform.GetChild(1).gameObject.SetActive(false);

        while (pressOnScreen.transform.GetChild(0).GetComponent<Image>().color.a > 0)
        {
            pressOnScreen.transform.GetChild(0).GetComponent<Image>().color -= new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        buttons.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void startNewGame()
    {
        int language = PlayerPrefs.GetInt("CurLanguage");

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("CurLanguage", language);
        PlayerPrefs.SetInt("LanguageSetted", 1);

        PlayerPrefs.SetInt("CutScene", 1);
        PlayerPrefs.SetInt("Started", 1);
        PlayerPrefs.SetInt("PlayerType", 0);

        newGameAsking.SetActive(false);

        StartCoroutine(loadCity());
    }

    public void startNewGameAsking()
    {
        if (PlayerPrefs.GetInt("Started") == 1)
            newGameAsking.SetActive(true);
        else
            startNewGame();
    }

    IEnumerator loadCity()
    {
        BG.SetActive(true);

        while (BG.GetComponent<Image>().color.a < 1)
        {
            BG.GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime / 2);

            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("City");
    }
}
