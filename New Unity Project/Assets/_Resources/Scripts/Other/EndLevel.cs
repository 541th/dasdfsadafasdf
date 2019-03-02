using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
    [SerializeField] GameObject BG, buttons;

    public void showPanel()
    {
        StartCoroutine(showing());
    }

    IEnumerator showing()
    {
        FindObjectOfType<UIManager>().setAllItems(false);
        transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).GetChild(1).gameObject.SetActive(true);

        float value = 100f / 256f;

        while (BG.GetComponent<Image>().color.a < value)
        {
            BG.GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        buttons.SetActive(true);
    }

    IEnumerator closing()
    {
        buttons.SetActive(false);

        while (BG.GetComponent<Image>().color.a > 0)
        {
            BG.GetComponent<Image>().color -= new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        FindObjectOfType<UIManager>().setAllItems(true);
        transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
    }

    IEnumerator loadCityEvent()
    {
        buttons.SetActive(false);

        GameObject blackScreen = FindObjectOfType<UIManager>().blackScreen;
        Image blackScreenImage = blackScreen.GetComponent<Image>();

        blackScreen.SetActive(true);

        while (blackScreenImage.color.a <= 1)
        {
            blackScreenImage.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        PlayerPrefs.SetInt("PlayerType", 0);

        UnityEngine.SceneManagement.SceneManager.LoadScene("City");
    }

    IEnumerator loadNextSceneEvent()
    {
        buttons.SetActive(false);

        GameObject blackScreen = FindObjectOfType<UIManager>().blackScreen;
        Image blackScreenImage = blackScreen.GetComponent<Image>();

        blackScreen.SetActive(true);

        while (blackScreenImage.color.a <= 1)
        {
            blackScreenImage.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        {
            string levelToLoad = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            int levelType = 0;

            if (levelToLoad == "5")
            {
                levelType = 1;
            }
            else
                levelType = 0;

            PlayerPrefs.SetInt("LevelType", levelType);

            UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad + "");
        }
    }

    public void nextLevel()
    {
        StartCoroutine(loadNextSceneEvent());

    }

    public void toCity()
    {
        StartCoroutine(loadCityEvent());
    }

    public void back()
    {
        StartCoroutine(closing());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        }
    }
}
