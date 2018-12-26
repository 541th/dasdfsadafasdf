using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> items;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void startAttack()
    {
        player.GetComponent<PlayerAttack>().startAttack();
    }

    public void stopAttack()
    {
        player.GetComponent<PlayerAttack>().stopAttack();
    }
}
