using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkContainer : MonoBehaviour
{
    [SerializeField] string key0, key1;
    [SerializeField] int id;
    [SerializeField] float mult;
    [SerializeField] bool skill;

    public void openPerksInfo()
    {
        //string text1; 
        //if (showDiff)
        //    text1 = key1 + ": " + startValue + " + " + mult;// тут надо умножать на уровень, ну или где-то в другом месте
        //else/

        FindObjectOfType<InfoController>().showPerksInfo(
            key0, 
            key1 + ((skill) ? "" : " + "),
            mult * ((!skill) ? InfoController.perks[id - 16].lvl + 1: 1) + "", 
            skill, 
            id, 
            transform.GetChild(0), 
            mult);
    }
}
