using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int HP;
    [SerializeField] Slider HPslider;
    [SerializeField] GameObject floatingNumbers;

    private void Start()
    {
        HP = (int)HPslider.value;
    }

    public void toDamage(int damage)
    {
        StartCoroutine(sub(damage));

        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
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
}
