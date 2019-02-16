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

            if (value < 0) value = 1;

            if (isLightning)
                collision.GetComponent<EnemyHP>().toDamageLightning((value) * (skill_2 ? 3 : 1));
            else
            if (!slow)
                collision.GetComponent<EnemyHP>().toDamage((value) * (skill_2 ? 3 : 1), Random.Range(0, 3) == 0 && !isArrow, withStan, bleeding, expl, sub, explMag);
            else
                collision.GetComponent<EnemyHP>().toDamageSlow((value) * (skill_2 ? 3 : 1), withStan);

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
