using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int HP, maxHP, startMax;
    [SerializeField] Slider HPslider;
    [SerializeField] GameObject floatingNumbers;

    private void Start()
    {
        HP = (int)HPslider.value;
        maxHP = HP;
        startMax = maxHP;

        if (GetComponent<PlayerMovement>().playerType == 1)
            updateMaxHP();
    }

    public int getCurHP()
    {
        return HP;
    }

    public int getMaxHP()
    {
        return maxHP;
    }

    public bool lessThan10()
    {
        return HP / maxHP < 0.1f;
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

        if (HP <= 0)
        {
            print("СМЭРТ");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Banner"))
        {
            startMax += 300;
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
            startMax -= 300;
            maxHP -= 300;
            HP -= 300;
            HPslider.value -= 300;
            HPslider.maxValue -= 300;
        }
    }

    public void updateMaxHP()
    {
        maxHP = startMax + (int)(InfoController.perks[0].value) + GetComponent<PlayerExp>().curLvl * 10;
        HP += 50;
        HPslider.maxValue = startMax + (int)(InfoController.perks[0].value) + GetComponent<PlayerExp>().curLvl * 10;
        HPslider.value += 50;
    }

    public void lvlup()
    {
        maxHP = startMax + (int)(InfoController.perks[0].value) + GetComponent<PlayerExp>().curLvl * 10;
        HP = maxHP;
        HPslider.maxValue = maxHP;
        HPslider.value = HP;
    }

    public void slowDown(float value)
    {
        GetComponent<PlayerMovement>().subMS(value);   
    }
}
