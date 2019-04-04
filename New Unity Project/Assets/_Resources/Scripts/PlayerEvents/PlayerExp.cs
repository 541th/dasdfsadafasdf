using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExp : MonoBehaviour
{
    public int curLvl = 1;
    [SerializeField] float curExp, expToLvlUp;
    int expDelta = 1;
    [SerializeField] Slider slider;
    public static int points;

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
        PlayerPrefs.SetFloat("EXP", curExp);
        PlayerPrefs.SetFloat("EXPToLvlUp", expToLvlUp);
        PlayerPrefs.SetInt("Points", points);
        PlayerPrefs.SetInt("CurLvl", curLvl);
    }

    private void Start()
    {
        if (PlayerPrefs.GetFloat("EXPToLvlUp") != 0)
        {
            expToLvlUp = PlayerPrefs.GetFloat("EXPToLvlUp");
            slider.maxValue = PlayerPrefs.GetFloat("EXPToLvlUp");
        }
        if (PlayerPrefs.GetInt("CurLvl") != 0) curLvl = PlayerPrefs.GetInt("CurLvl");

        slider.value = PlayerPrefs.GetFloat("EXP");
        points = PlayerPrefs.GetInt("Points");

        curExp = (int)slider.value;
    }

    public void doubleExp()
    {
        StartCoroutine(doubleExpEvent());
    }

    public int getKoefByLvl()
    {
        return 4 / curLvl;
    }

    IEnumerator doubleExpEvent()
    {
        float timer = 10;
        expDelta = 2;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        expDelta = 1;
    }

    [SerializeField] bool toadd;
    [SerializeField] float toaddvalue;
    private void Update()
    {
        if (toadd)
        {
            addExp(toaddvalue * expDelta);
            toadd = false;
        }
    }

    public void addExp(float value)
    {
        StartCoroutine(addEvent(value * expDelta));
    }

    IEnumerator showNewLvl()
    {
        GameObject newLvl = Instantiate(Resources.Load("Prefabs/NewLvl") as GameObject, GameObject.Find("Main Camera").transform.GetChild(0));
        newLvl.transform.localPosition = new Vector2(0, 120);
        newLvl.GetComponent<Text>().color = new Color(1, 1, 1, 0);
        newLvl.GetComponent<Text>().text = "Новый уровень\n" + curLvl;

        float timer = 0, deltaTime = 0;
        while (timer < 1)
        {
            deltaTime = Time.unscaledDeltaTime;
            timer += deltaTime;
            newLvl.transform.localPosition += new Vector3(0, deltaTime * 10);
             
            newLvl.GetComponent<Text>().color += new Color(0, 0, 0, deltaTime);

            yield return null;
        }

        timer = 0;
        while (timer < 3)
        {
            deltaTime = Time.unscaledDeltaTime;
            timer += deltaTime;
            newLvl.transform.localPosition += new Vector3(0, deltaTime * 10);

            yield return null;
        }

        timer = 0;
        while (timer < 1)
        {
            deltaTime = Time.unscaledDeltaTime;
            timer += deltaTime;
            newLvl.transform.localPosition += new Vector3(0, deltaTime * 10);
            
            newLvl.GetComponent<Text>().color -= new Color(0, 0, 0, deltaTime);

            yield return null;
        }

        Destroy(newLvl);
    }

    IEnumerator addEvent(float value)
    {
        int mult = (int)value / 100;
        mult++;
        while (value > 1)
        {
            curExp += mult;
            slider.value += mult;
            value -= mult;
            yield return null;

            if (curExp >= expToLvlUp)
            {
                slider.value = 0;
                curExp = 0;
                curLvl++;
                points += 2;
                StartCoroutine(showNewLvl());
                expToLvlUp += 100 * curLvl;
                slider.maxValue = expToLvlUp;
                GetComponent<PlayerHP>().lvlup();
            }
        }

        if (curExp >= expToLvlUp)
        {
            slider.value = 0;
            curExp = 0;
            curLvl++;
            points += 2;
            StartCoroutine(showNewLvl());
            expToLvlUp += 100 * curLvl;
            slider.maxValue = expToLvlUp;
            GetComponent<PlayerHP>().lvlup();
        }

        curExp += value;
        slider.value += value;
    }
}
