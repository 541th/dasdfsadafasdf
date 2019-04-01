using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject newGameAsking, continueButton, BG, buttons, pressOnScreen;

    void Start()
    {
        continueButton.SetActive(PlayerPrefs.GetInt("Started") == 1);
        buttons.SetActive(false);
        pressOnScreen.SetActive(true);
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
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("CutScene", 1);
        PlayerPrefs.SetInt("Started", 1);
        PlayerPrefs.SetInt("PlayerType", 0);

        newGameAsking.SetActive(false);

        StartCoroutine(loadCity());
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
