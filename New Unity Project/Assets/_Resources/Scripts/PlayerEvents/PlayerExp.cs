using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExp : MonoBehaviour
{
    int curLvl;
    [SerializeField] float curExp, expToLvlUp;
    [SerializeField] Slider slider;

    private void Start()
    {
        curExp = (int)slider.value;
    }

    public void addExp(float value)
    {
        StartCoroutine(addEvent(value));
        
        if (curExp >= expToLvlUp)
        {
            slider.value = 0;
            curExp = 0;
            curLvl++;
            expToLvlUp += 160 * curLvl;
            slider.maxValue = expToLvlUp;
        }
    }

    IEnumerator addEvent(float value)
    {
        while (value > 1)
        {
            curExp++;
            slider.value++;
            value--;
            yield return null;
        }

        curExp += value;
    }
}
