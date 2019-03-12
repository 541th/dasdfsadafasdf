using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsHP : MonoBehaviour
{
    public int HP, maxHP;
    Transform player;

    [SerializeField] Material defaultMaterial, blinkMaterial;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    public void toDamage(int damage, bool isFlying, bool stan, bool bleeding, bool expl, bool sub, bool explMag)
    {
        if (expl) explEvent();

        if (explMag) explMagEvent();

        HP -= damage;
        transform.parent.parent.GetComponentInChildren<EnemyHP>().toDamage(damage, false, false, false, false, false, false);

        InfoController.addExp(10);

        checkDeath();
    }

    void explEvent()
    {
        GameObject exlpParticle = Instantiate(Resources.Load("Prefabs/Effects/ArrowExpl") as GameObject, transform.position - new Vector3(0, 0.3f), Quaternion.identity);
        Destroy(exlpParticle, 0.33f);
        GameObject arrowPrefab = Resources.Load("Prefabs/Arrows/Arrow_0") as GameObject;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;

                GameObject arrow = Instantiate(arrowPrefab);
                arrow.transform.position = transform.position;

                arrow.GetComponent<ArrowFly>().target = new Vector2(i, j).normalized;
                arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(i, j) * 180 / Mathf.PI);
            }
        }
    }

    void explMagEvent()
    {
        GameObject exlpParticle = Instantiate(Resources.Load("Prefabs/Effects/MagArrowExpl") as GameObject, transform.position - new Vector3(0, 0.3f), Quaternion.identity);
        Destroy(exlpParticle, 0.33f);
    }

    public void toDamageSlow(int damage, bool stan)
    {
        HP -= damage;
        transform.parent.parent.GetComponentInChildren<EnemyHP>().toDamage(damage, false, false, false, false, false, false);

        InfoController.addExp(10);

        checkDeath();
    }

    public void toDamageLightning(int damage)
    {
        HP -= damage;
        transform.parent.parent.GetComponentInChildren<EnemyHP>().toDamage(damage, false, false, false, false, false, false);

        InfoController.addExp(10);

        GameObject _l = Instantiate(Resources.Load("Prefabs/Arrows/ChainLightningTrigger") as GameObject, transform.position, Quaternion.identity);
        _l.GetComponent<ChainLightning>()._e.Add(gameObject);

        checkDeath();
    }

    public void toDamageLightning(int damage, List<GameObject> _e)
    {
        damage += (int)InfoController.perks[13].value;
        HP -= damage;
        transform.parent.parent.GetComponentInChildren<EnemyHP>().toDamage(damage, false, false, false, false, false, false);
        InfoController.addExp(10);

        GameObject _le = Instantiate(Resources.Load("Prefabs/Effects/Lightning") as GameObject, transform.position, Quaternion.identity);
        Destroy(_le, 0.4f);

        GameObject _l = Instantiate(Resources.Load("Prefabs/Arrows/ChainLightningTrigger") as GameObject, transform.position, Quaternion.identity);
        _l.GetComponent<ChainLightning>()._e = _e;
        _l.GetComponent<ChainLightning>()._e.Add(gameObject);

        checkDeath();
    }

    IEnumerator hitBlink()
    {
        transform.GetComponent<SpriteRenderer>().material = blinkMaterial;
        yield return new WaitForSeconds(0.04f);
        transform.GetComponent<SpriteRenderer>().material = defaultMaterial;
    }

    void stopSleeping()
    {
        transform.parent.parent.GetComponent<CapsuleCollider2D>().enabled = true;
        transform.parent.parent.GetComponent<AIMethods>().stanned = false;
    }

    [SerializeField] bool isBoss, isBossSleeping;
    void checkDeath()
    {
        if (isBossSleeping)
        {
            isBossSleeping = false;
            Invoke("stopSleeping", 1);
        }

        StartCoroutine(hitBlink());

        if (HP <= 0)
        {
            GameObject _le = Instantiate(Resources.Load("Prefabs/Effects/Pentargam_Fire_Explosion") as GameObject, transform.position, Quaternion.identity);
            Destroy(_le, 0.4f); 
            gameObject.SetActive(false);
        }
    }
}