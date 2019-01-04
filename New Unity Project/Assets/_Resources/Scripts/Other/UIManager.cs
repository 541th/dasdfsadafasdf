﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<GameObject> items;
    [SerializeField] GameObject glideButton;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void startAttack()
    {
        player.GetComponent<PlayerAttack_Warrior>().startAttack();
    }

    public void stopAttack()
    {
        player.GetComponent<PlayerAttack_Warrior>().stopAttack();
    }

    public void glide()
    {
        player.GetComponent<PlayerMovement>().glide();
    }

    public void glideFalse()
    {
        glideButton.GetComponent<Button>().interactable = false;
        glideButton.GetComponent<Image>().color = new Color(glideButton.GetComponent<Image>().color.r, glideButton.GetComponent<Image>().color.g, glideButton.GetComponent<Image>().color.b, 0.05f);
    }

    public void glideReturn()
    {
        StartCoroutine(returning());
    }

    IEnumerator returning()
    {
        while (glideButton.GetComponent<Image>().color.a <= 0.1f)
        {
            glideButton.GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        glideButton.GetComponent<Button>().interactable = true;
    }

    [SerializeField] GameObject gameMenu, infoCanvas;

    public void openMenu()
    {
        gameMenu.SetActive(true);
        setAllItems(false);
    }

    public void setAllItems(bool value)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(value);
        }
    }

    public void closeMenu()
    {
        gameMenu.SetActive(false);
        setAllItems(true);
    }

    public void openInfo()
    {
        gameMenu.SetActive(false);

        infoCanvas.SetActive(true);
    }
}
