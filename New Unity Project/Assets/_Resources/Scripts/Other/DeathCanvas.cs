using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCanvas : MonoBehaviour
{
    public void start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void startAgain()
    {
        StartCoroutine(startAgainEvent());
    }

    IEnumerator startAgainEvent()
    {
        GameObject blackScreen = FindObjectOfType<UIManager>().blackScreen;
        Image blackScreenImage = blackScreen.GetComponent<Image>();

        blackScreen.SetActive(true);

        transform.GetChild(0).gameObject.SetActive(false);

        {
            string levelToLoad = int.Parse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) + "";
            int levelType = 0;

            if (levelToLoad == "5")
            {
                levelType = 1;
            }
            else
            if (levelToLoad == "10")
            {
                levelType = 2;
            }
            else
            if (levelToLoad == "15")
            {
                levelType = 3;
            }
            else
            if (levelToLoad == "20")
            {
                levelType = 4;
            }
            else
            if (levelToLoad == "25")
            {
                levelType = 5;
            }

            PlayerPrefs.SetInt("LevelType", levelType);

            FindObjectOfType<PlayerHP>().setAllFull();

            UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad + "");
        }

        yield return null;
    }

    public void toCity()
    {
        StartCoroutine(toCityEvent());
    }

    IEnumerator toCityEvent()
    {
        GameObject blackScreen = FindObjectOfType<UIManager>().blackScreen;
        Image blackScreenImage = blackScreen.GetComponent<Image>();

        transform.GetChild(0).gameObject.SetActive(false);

        blackScreen.SetActive(true);

        while (blackScreenImage.color.a <= 1)
        {
            blackScreenImage.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        PlayerPrefs.SetInt("PlayerType", 0);

        UnityEngine.SceneManagement.SceneManager.LoadScene("City");
    }
}
