using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int HP;
    [SerializeField] Slider HPslider;

    private void Start()
    {
        HP = (int)HPslider.value;
    }

    public void toDamage(int damage)
    {
        HP -= damage;

        HPslider.value -= damage;

        if (HP <= 0)
        {
            print("СМЭРТ");
        }
    }
}
