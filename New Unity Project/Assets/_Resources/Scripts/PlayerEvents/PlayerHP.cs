using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int HP, maxHP, startMax;
    [SerializeField] Slider HPslider;
    [SerializeField] GameObject floatingNumbers;

    [SerializeField] Material defaultMaterial, blinkMaterial;
    InventoryManager _im;
    
    public void setAllFull()
    {
        created = false;
        HPslider.maxValue = 200 + (FindObjectOfType<PlayerMovement>().playerType == 1 ? (int)(InfoController.perks[0].value) : 0) + FindObjectOfType<PlayerExp>().curLvl * 10;
        HPslider.value = HPslider.maxValue;
        HP = (int)HPslider.value;
        maxHP = HP;
        startMax = maxHP;
    }

    private void Start()
    {
        HPslider.value = HPslider.maxValue;

        _im = FindObjectOfType<InventoryManager>();
        HP = (int)HPslider.value;
        maxHP = HP;
        startMax = maxHP;

        if (FindObjectOfType<PlayerMovement>().playerType == 1)
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
        return (HP / (float)maxHP) <= 0.3f;
    }

    public void createForceField()
    {
        StartCoroutine(fieldEvent());
    }

    bool isField;
    IEnumerator fieldEvent()
    {
        GameObject field = Instantiate(Resources.Load("Prefabs/Effects/ForceField_0") as GameObject);
        field.transform.localScale = new Vector3(2, 2, 2);
        isField = true;

        Transform t = GameObject.Find("Player").transform;
        Transform other = null;

        for (int i = 0; i < t.childCount; i++)
            if (t.GetChild(i).name == "Other")
            {
                other = t.GetChild(i);
                break;
            }

        field.transform.SetParent(other);
        field.transform.localPosition = Vector2.zero;

        yield return new WaitForSeconds(6);

        isField = false;
        Destroy(field);
    }

    PlayerMovement _pm;
    public void toDamage(int damage)
    {
        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);

        if (FindObjectOfType<PlayerMovement>().dontAttack)
        {
            //GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
            fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(LanguageLines.getLine(12));
            fn.transform.localScale /= 2;
            return;
        }

        if (FindObjectOfType<PlayerMovement>().inRage) damage *= 2;

        if (isField) damage /= 2;

        if (_pm == null) _pm = FindObjectOfType<PlayerMovement>();

        int def = 0;
        if (_pm.playerType != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (_im.takedItems[_pm.playerType - 1, i] != null)
                    def += _im.takedItems[_pm.playerType - 1, i].value;
            }
        }

        damage -= def / 3;

        if (damage <= 0) damage = 1;

        GameObject.Find("Player").transform.GetChild(0).GetComponent<SpriteRenderer>().material = blinkMaterial;
        GameObject.Find("Player").transform.GetChild(1).GetComponent<SpriteRenderer>().material = blinkMaterial;
        StartCoroutine(sub(damage));

        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(damage + "");
    }

    public void toHeal(int value)   
    {
        if (HP < maxHP)
        {
            HP += value;
            HPslider.value += value;
        }
    }

    IEnumerator sub(int value)
    {
        yield return new WaitForSeconds(0.04f);

        GameObject.Find("Player").transform.GetChild(0).GetComponent<SpriteRenderer>().material = defaultMaterial;
        GameObject.Find("Player").transform.GetChild(1).GetComponent<SpriteRenderer>().material = defaultMaterial;

        while (value > 0)
        {
            HP--;
            HPslider.value--;
            value--;
            yield return null;
        }

        if (HP <= 0 && !created)
        {
            FindObjectOfType<CamFollow>().stopCameraRotating();
            created = true;

            foreach(CnControls.SimpleJoystick item in FindObjectsOfType<CnControls.SimpleJoystick>())
            {
                item.pointerUp();
            }

            FindObjectOfType<UIManager>().setAllItems(false);
            GameObject deathEffect = Instantiate(deathEffectPrefab);
        }
    }

    bool isLessThan10, isJagging;
    public bool dam;
    private void Update()
    {
        if (dam)
        {
            toDamage(100);
            dam = false;
        }

        if (lessThan10() && !isJagging)
        {
            isJagging = true;

            StartCoroutine(jaggingEvent());
        }
        else
        {
            if (!lessThan10() && isJagging)
            {
                isJagging = false;
            }
        }
    }

    IEnumerator jaggingEvent()
    {
        Vector3 startPos = HPslider.transform.GetChild(1).localPosition;

        while (isJagging)
        {
            HPslider.transform.GetChild(1).position = HPslider.transform.GetChild(1).position + new Vector3(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));

            yield return new WaitForSeconds(Random.Range(.06f, .1f));

            HPslider.transform.GetChild(1).localPosition = startPos;
        }

        HPslider.transform.GetChild(1).localPosition = startPos;
    }

    bool created;
    [SerializeField] GameObject deathEffectPrefab;

    public void startBanner()
    {
        startMax += 300;
        maxHP += 300;
        HP += 300;
        HPslider.maxValue += 300;
        HPslider.value += 300;
    }

    public void stopBanner()
    {
        startMax -= 300;
        maxHP -= 300;
        HP -= 300;
        HPslider.value -= 300;
        HPslider.maxValue -= 300;
    }

    public void updateMaxHP()
    {
        maxHP = 200 + (FindObjectOfType<PlayerMovement>().playerType == 1 ? (int)(InfoController.perks[0].value) : 0) + FindObjectOfType<PlayerExp>().curLvl * 10;
        startMax = maxHP;
        HP += 50;
        HPslider.maxValue = maxHP;
        HPslider.value += 50;
    }

    public void lvlup()
    {
        maxHP = 200 + (FindObjectOfType<PlayerMovement>().playerType == 1 ? (int)(InfoController.perks[0].value) : 0) + FindObjectOfType<PlayerExp>().curLvl * 10;
        startMax = maxHP;
        HP = maxHP;
        HPslider.maxValue = maxHP;
        HPslider.value = HP;
    }

    public void slowDown(float value)
    {
        FindObjectOfType<PlayerMovement>().subMS(value);   
    }
}
