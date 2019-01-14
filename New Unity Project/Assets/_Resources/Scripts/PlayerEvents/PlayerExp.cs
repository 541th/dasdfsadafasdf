using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExp : MonoBehaviour
{
    public int curLvl;
    [SerializeField] float curExp, expToLvlUp;
    [SerializeField] Slider slider;
    public int points;

    private void Start()
    {
        curExp = (int)slider.value;
    }

    [SerializeField] bool toadd;
    [SerializeField] float toaddvalue;
    private void Update()
    {
        if (toadd)
        {
            addExp(toaddvalue);
            toadd = false;
        }
    }

    public void addExp(float value)
    {
        StartCoroutine(addEvent(value));
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
            deltaTime = Time.deltaTime;
            timer += deltaTime;
            newLvl.transform.localPosition += new Vector3(0, deltaTime * 10);
             
            newLvl.GetComponent<Text>().color += new Color(0, 0, 0, deltaTime);

            yield return null;
        }

        timer = 0;
        while (timer < 3)
        {
            deltaTime = Time.deltaTime;
            timer += deltaTime;
            newLvl.transform.localPosition += new Vector3(0, deltaTime * 10);

            yield return null;
        }

        timer = 0;
        while (timer < 1)
        {
            deltaTime = Time.deltaTime;
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
        }

        curExp += value;
        slider.value += value;
    }
}
