using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static List<Item> DB = new List<Item>();

    void Start()
    {
        for (int characterType = 0; characterType < 3; characterType++)
            for (int itemType = 0; itemType < 4; itemType++)
                for (int item = 0; item < 5; item++)
                {
                    int mult = (itemType + item + 1) * 2;
                    DB.Add(new Item(mult, mult * 6, characterType.ToString(), itemType + "" + item, characterType * 100 + itemType * 10 + item));
                }
    }

    public static Item getItemById(int id)
    {
        int res = ((id / 100) * 20) + (((id / 10) % 10) * 5) + (id % 10);

        return DB[res];
    }

    public static int getCharacterType(int id)
    {
        return id / 100;
    }

    public static int getItemType(int id)
    {
        return (id / 10) % 10;
    }

    public static int getItemId(int id)
    {
        return id % 10;
    }

    public class Item
    {
        public int id;
        public int value;
        public int power;
        public Sprite sprite;

        public Item(int _val, int _pow, string spriteFolder, string spriteName, int _id)
        {
            id = _id;
            value = _val;
            power = _pow;
            sprite = Resources.Load<Sprite>("Items/" + spriteFolder + "/" + spriteName);
        }
    }
}
