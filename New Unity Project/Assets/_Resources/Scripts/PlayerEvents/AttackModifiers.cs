using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackModifiers : MonoBehaviour
{
    static List<Modifier> modifiers = new List<Modifier>();

    public static void clear()
    {
        modifiers.Clear();
    }

    public static void addModifier(char _c, int _v)
    {
        modifiers.Add(new Modifier(_c, _v));
    }

    public static void removeModifier(char _c, int _v)
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

    public static float getModifiedValue(float startValue)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            switch (modifiers[i].sign)
            {
                case '+': startValue += modifiers[i].value; break;
                case '-': startValue -= modifiers[i].value; break;
                case '/': startValue /= modifiers[i].value; break;
                case '*': startValue *= modifiers[i].value; break;
            }
        }

        return startValue;
    }

    public class Modifier
    {
        public char sign;
        public int value;

        public Modifier(char _c, int _v)
        {
            sign = _c;
            value = _v;
        }
    }
}
