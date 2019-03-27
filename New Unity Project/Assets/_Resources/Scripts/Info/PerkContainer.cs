using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkContainer : MonoBehaviour
{
    [SerializeField] string key0, key1;
    public int id, lvl;
    [SerializeField] float mult;
    [SerializeField] bool skill;

    public void openPerksInfo()
    {
        FindObjectOfType<InfoController>().showPerksInfo(
            key0, 
            key1 + ((skill) ? "" : " + "),
            mult * ((!skill) ? InfoController.perks[id - 16].lvl + 1: 1) + "", 
            skill, 
            id, 
            transform.GetChild(0), 
            mult,
            lvl);
    }

    public int getLvl()
    {
        return lvl;
    }
}
