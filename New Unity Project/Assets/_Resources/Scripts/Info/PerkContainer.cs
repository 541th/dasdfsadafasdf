using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkContainer : MonoBehaviour
{
    [SerializeField] string key0, key1;
    [SerializeField] float mult, startValue;
    [SerializeField] bool showDiff, skill;

    public void openPerksInfo()
    {
        string text1; 
        if (showDiff)
        {
            text1 = key1 + ": " + startValue + " + " + mult;// тут надо умножать на уровень, ну или где-то в другом месте
        }
        else
        {
            text1 = key1 + " + " + (startValue + mult);// тут надо умножать на уровень, ну или где-то в другом месте
        }

        if (skill) text1 = key1;

        FindObjectOfType<InfoController>().showPerksInfo(key0, text1);
    }
}
