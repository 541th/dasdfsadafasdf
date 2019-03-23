using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonetsEffect : MonoBehaviour
{
    public bool monets, enemy;
    public int amount;
    [SerializeField] Sprite bottle;

    void Start()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();

        if (monets)
        {
            if (!enemy)
            {
                int lvl = uiManager.GetComponent<PlayerExp>().curLvl + Random.Range(0, uiManager.GetComponent<PlayerExp>().curLvl - uiManager.GetComponent<PlayerExp>().curLvl / 2);
                int scene = (int.Parse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));

                int monetsValue = Random.Range(50, 100) + lvl * scene;

                uiManager.addMonets(monetsValue);

                transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + monetsValue;
            }
            else
            {
                int monetsValue = Random.Range(1, amount);

                uiManager.addMonets(monetsValue);

                transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + monetsValue;
            }
        }
        else
        {
            transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = bottle;
            int bottlesValue = Random.Range(1, 10);

            uiManager.addPotion(bottlesValue);

            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + bottlesValue;
        }

        StartCoroutine(Event());
    }

    IEnumerator Event()
    {
        float timer = .6f;

        float deltaTime;

        while (timer > 0)
        {
            deltaTime = Time.deltaTime;
            timer -= deltaTime;

            transform.position += new Vector3(0, deltaTime);

            yield return null;
        }

        timer = .6f;

        while (timer > 0)
        {
            deltaTime = Time.deltaTime;
            timer -= deltaTime;

            transform.position += new Vector3(0, deltaTime);

            transform.GetChild(0).GetChild(0).GetComponent<Text>().color -= new Color(0, 0, 0, deltaTime * 1.6f);
            transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color -= new Color(0, 0, 0, deltaTime * 1.6f);

            yield return null;
        }

        Destroy(gameObject);
    }
}
