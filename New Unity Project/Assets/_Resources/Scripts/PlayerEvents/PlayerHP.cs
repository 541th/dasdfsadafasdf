﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int HP, maxHP;
    [SerializeField] Slider HPslider;
    [SerializeField] GameObject floatingNumbers;

    private void Start()
    {
        HP = (int)HPslider.value;
        maxHP = HP;
    }

    public void createForceField()
    {
        StartCoroutine(fieldEvent());
    }

    bool isField;
    IEnumerator fieldEvent()
    {
        GameObject field = Instantiate(Resources.Load("Prefabs/Effects/ForceField_0") as GameObject, transform.position, Quaternion.identity);
        field.transform.localScale = new Vector3(2, 2, 2);
        isField = true;

        Transform other = null;
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).name == "Other")
            {
                other = transform.GetChild(i);
                break;
            }
        field.transform.SetParent(other);

        yield return new WaitForSeconds(6);

        isField = false;
        Destroy(field);
    }

    public void toDamage(int damage)
    {
        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
        if (GetComponent<PlayerMovement>().dontAttack)
        {
            //GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
            fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText("промах");
            fn.transform.localScale /= 2;
            return;
        }

        if (GetComponent<PlayerMovement>().inRage) damage *= 2;

        if (isField) damage /= 2;

        StartCoroutine(sub(damage));

        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(damage + "");

        if (HP <= 0)
        {
            print("СМЭРТ");
        }
    }

    IEnumerator sub(int value)
    {
        while (value > 0)
        {
            HP--;
            HPslider.value--;
            value--;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Banner"))
        {
            maxHP += 300;
            HP += 300;
            HPslider.maxValue += 300;
            HPslider.value += 300;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Banner"))
        {
            maxHP -= 300;
            HP -= 300;
            HPslider.value -= 300;
            HPslider.maxValue -= 300;
        }
    }
}
