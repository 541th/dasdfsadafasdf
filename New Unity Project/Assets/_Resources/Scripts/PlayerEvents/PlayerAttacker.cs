using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int randL, randH;
    [SerializeField] bool isArrow, withStan, skill_2, slow, modifyWithMagPerk;
    public bool isLightning, bleeding, expl, explMag, sub;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHP"))
        {
            int value = damage + Random.Range(randL, randH);

            value = (int)AttackModifiers.getModifiedValue(value);

            value += (int)InfoController.perks[3].value;
            if (modifyWithMagPerk) value += (int)InfoController.perks[13].value;

            if (FindObjectOfType<PlayerHP>().lessThan10()) value += (int)InfoController.perks[2].value;

            value += FindObjectOfType<InventoryManager>().takedItems[FindObjectOfType<PlayerMovement>().playerType - 1, 4].value / 2;

            if (value < 0) value = 1;

            if (isLightning)
            {
                if (collision.GetComponent<EnemyHP>() != null)
                    collision.GetComponent<EnemyHP>().toDamageLightning((value) * (skill_2 ? 3 : 1));
                else
                    collision.GetComponent<PartsHP>().toDamageLightning((value) * (skill_2 ? 3 : 1));
            }
            else
            if (!slow)
            {
                if (collision.GetComponent<EnemyHP>() != null)  
                    collision.GetComponent<EnemyHP>().toDamage((value) * (skill_2 ? 3 : 1), Random.Range(0, 3) == 0 && !isArrow, withStan, bleeding, expl, sub, explMag);
                else
                    collision.GetComponent<PartsHP>().toDamage((value) * (skill_2 ? 3 : 1), Random.Range(0, 3) == 0 && !isArrow, withStan, bleeding, expl, sub, explMag);
            }
            else
            {
                if (collision.GetComponent<EnemyHP>() != null)
                    collision.GetComponent<EnemyHP>().toDamageSlow((value) * (skill_2 ? 3 : 1), withStan);
                else
                    collision.GetComponent<PartsHP>().toDamageSlow((value) * (skill_2 ? 3 : 1), withStan);
            }

            expl = false;
            sub = false;
            explMag = false;
            Invoke("returnWithStan", 0.2f);

            if (isArrow && GetComponent<ArrowFly>() != null) GetComponent<ArrowFly>().stop();
        }
    }

    void returnWithStan()
    {
        withStan = false;
        bleeding = false;
    }
}
