using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int randL, randH;
    [SerializeField] bool isArrow, withStan, skill_2, slow;
    public bool isLightning;
    List<Modifiers> modifiers = new List<Modifiers>();

    public void addModifier(char _c, int _v)
    {
        modifiers.Add(new Modifiers(_c, _v));
    }

    public void removeModifier(char _c, int _v)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            if (modifiers[i].value == _v)
                if (modifiers[i].sign == _c)
                {
                    modifiers.RemoveAt(i);
                    break;
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHP"))
        {
            int value = damage + Random.Range(randL, randH);

            for (int i = 0; i < modifiers.Count; i++)
            {
                switch (modifiers[i].sign)
                {
                    case '+': value += modifiers[i].value; break;
                    case '-': value -= modifiers[i].value; break;
                    case '/': value /= modifiers[i].value; break;
                    case '*': value *= modifiers[i].value; break;
                }
            }

            if (isLightning)
                collision.GetComponent<EnemyHP>().toDamageLightning((value) * (skill_2 ? 3 : 1));
            if (!slow)
                collision.GetComponent<EnemyHP>().toDamage((value) * (skill_2 ? 3 : 1), true && !isArrow, withStan);
            else
                collision.GetComponent<EnemyHP>().toDamageSlow((value) * (skill_2 ? 3 : 1), withStan);

            withStan = false;
        }
    }

    class Modifiers
    {
        public char sign;
        public int value;

        public Modifiers(char _c, int _v)
        {
            sign = _c;
            value = _v;
        }
    }
}
