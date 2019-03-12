using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public int HP, maxHP;
    Transform player;
    [SerializeField] GameObject floatingNumbers, slider;
    public float expForKill;

    [SerializeField] Material defaultMaterial, blinkMaterial;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    [SerializeField] bool divide, createSwill, isGhost;

    public void toHealth(int value)
    {
        if (HP < maxHP) HP += value;
        else HP = maxHP;
        
        if (HP != maxHP)
        {
            slider.SetActive(true);

            slider.transform.GetChild(0).GetComponent<Slider>().maxValue = maxHP;
            slider.transform.GetChild(0).GetComponent<Slider>().value = HP;
            slider.transform.GetChild(0).GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, ((float)1 / maxHP) * HP);
        }
        else
        {
            slider.SetActive(false);
        }
    }

    public void toDamage(int damage, bool isFlying, bool stan, bool bleeding, bool expl, bool sub, bool explMag)
    {
        if (bleeding) StartCoroutine(bleedingEvent());
        if (expl) explEvent();
        if (sub)
        {
            transform.parent.GetComponent<AIMethods>().subDamage();
        }
        if (explMag) explMagEvent();

        string res;

        if (!isGhost)
        {
            HP -= damage;
            res = damage + "";
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                HP -= damage;
                res = damage + "";
            }
            else
            res = "промах";
        }

        InfoController.addExp(expForKill / 10);
        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(res);

        slider.SetActive(true);

        slider.transform.GetChild(0).GetComponent<Slider>().maxValue = maxHP;
        slider.transform.GetChild(0).GetComponent<Slider>().value = HP;
        slider.transform.GetChild(0).GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, ((float)1 / maxHP) * HP);

        if (isFlying)
        {
            StartCoroutine(flying(player.position - transform.parent.position));
        }

        checkDeath();

        if (stan) transform.parent.GetComponent<AIMethods>().startStan();
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

    IEnumerator bleedingEvent()
    {
        float bleedTimer = 4, damTimer = 0;

        GameObject bleed = Instantiate(Resources.Load("Prefabs/Effects/Bleeding") as GameObject, transform.position - new Vector3(0, 0.3f), Quaternion.identity);
        bleed.transform.SetParent(transform);

        while (bleedTimer >= 0)
        {
            damTimer += Time.deltaTime;
            bleedTimer -= Time.deltaTime;

            if (damTimer > 0.3f)
            {
                damTimer = 0;
                toDamage(1, false, false, false, false, false, false);
            }

            yield return null;
        }

        Destroy(bleed);
    }

    public void toDamageSlow(int damage, bool stan)
    {
        transform.parent.GetComponent<AIMethods>().ms /= 2;
        Invoke("setMSBack", 3);

        string res;

        if (!isGhost)
        {
            HP -= damage;
            res = damage + "";
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                HP -= damage;
                res = damage + "";
            }
            else
                res = "промах";
        }

        InfoController.addExp(expForKill / 10);
        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(res);

        slider.SetActive(true);

        slider.transform.GetChild(0).GetComponent<Slider>().maxValue = maxHP;
        slider.transform.GetChild(0).GetComponent<Slider>().value = HP;
        slider.transform.GetChild(0).GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, ((float)1 / maxHP) * HP);

        checkDeath();

        if (stan) transform.parent.GetComponent<AIMethods>().startStan();
    }

    public void toDamageLightning(int damage)
    {
        string res;

        if (!isGhost)
        {
            HP -= damage;
            res = damage + "";
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                HP -= damage;
                res = damage + "";
            }
            else
                res = "промах";
        }
        InfoController.addExp(expForKill / 10);

        GameObject _l = Instantiate(Resources.Load("Prefabs/Arrows/ChainLightningTrigger") as GameObject, transform.position, Quaternion.identity);
        _l.GetComponent<ChainLightning>()._e.Add(gameObject);

        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(res);

        slider.SetActive(true);

        slider.transform.GetChild(0).GetComponent<Slider>().maxValue = maxHP;
        slider.transform.GetChild(0).GetComponent<Slider>().value = HP;
        slider.transform.GetChild(0).GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, ((float)1 / maxHP) * HP);

        checkDeath();
    }

    public void toDamageLightning(int damage, List<GameObject> _e)
    {
        damage += (int)InfoController.perks[13].value;
        string res;

        if (!isGhost)
        {
            HP -= damage;
            res = damage + "";
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                HP -= damage;
                res = damage + "";
            }
            else
                res = "промах";
        }
        InfoController.addExp(expForKill / 10);

        GameObject _le = Instantiate(Resources.Load("Prefabs/Effects/Lightning") as GameObject, transform.position, Quaternion.identity);
        Destroy(_le, 0.4f);

        GameObject _l = Instantiate(Resources.Load("Prefabs/Arrows/ChainLightningTrigger") as GameObject, transform.position, Quaternion.identity);
        _l.GetComponent<ChainLightning>()._e = _e;
        _l.GetComponent<ChainLightning>()._e.Add(gameObject);

        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(res);

        slider.SetActive(true);

        slider.transform.GetChild(0).GetComponent<Slider>().maxValue = maxHP;
        slider.transform.GetChild(0).GetComponent<Slider>().value = HP;
        slider.transform.GetChild(0).GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, ((float)1 / maxHP) * HP);

        checkDeath();
    }

    IEnumerator hitBlink()
    {
        transform.parent.GetComponent<SpriteRenderer>().material = blinkMaterial;
        yield return new WaitForSeconds(0.04f);
        transform.parent.GetComponent<SpriteRenderer>().material = defaultMaterial;
    }

    void stopSleeping()
    {
        transform.parent.GetComponent<CapsuleCollider2D>().enabled = true;
        transform.parent.GetComponent<AIMethods>().stanned = false;
    }

    [SerializeField] bool isBoss, isBossSleeping, isFinalCutscene;
    void checkDeath()
    {
        if (isBossSleeping)
        {
            isBossSleeping = false;
            transform.parent.GetComponent<Animator>().SetTrigger("StopSleeping");
            Invoke("stopSleeping", 1);
        }

        if (isBoss)
        {
            FindObjectOfType<CamFollow>().startCameraRotating();

            Camera.main.GetComponent<CamFollow>().lightOnMap();

            FindObjectOfType<UIManager>().showBossHPBar(HP, maxHP);
        }

        StartCoroutine(hitBlink());

        if (HP <= 0)
        {
            if (isFinalCutscene)
            {
                StartCoroutine(FindObjectOfType<Boss_5_DeathCutscene>().finalCutscene());

                return;
            }

            if (isBoss)
            {
                FindObjectOfType<CamFollow>().stopCameraRotating();
                GameObject.Find("PortalEndObject").transform.GetChild(0).gameObject.SetActive(true);
            }

            if (divide)
                if (GetComponent<Divide>() != null)
                    GetComponent<Divide>().divide();
                else
                    GetComponent<DivideParts>().divide();

            if (player != null)
                FindObjectOfType<PlayerExp>().addExp(expForKill);

            showDeath();
        }
    }

    void showDeath()
    {
        if (isBoss)
        {
            transform.parent.GetComponent<Animator>().SetTrigger("Death");
        }
        else
            GetComponentInParent<AIMethods>().showDeath();
    }

    IEnumerator flying(Vector2 to)
    {
        float timer = 0.1f;
        float x = -to.x * Time.deltaTime * 1000, y = -to.y * Time.deltaTime * 1000;
        Rigidbody2D parent = transform.parent.GetComponent<Rigidbody2D>();
        
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            //x += (0 - x) * Time.deltaTime * 20;
            //y += (0 - y) * Time.deltaTime * 20;

            parent.velocity = new Vector3(x, y, 0);

            yield return null;
        }

        parent.velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Slow"))
        {
            if (transform.parent != null)
            {
                transform.parent.GetComponent<AIMethods>().ms /= 2;
            }
        }
        else if (collision.CompareTag("TrappingNet"))
        {
            if (transform.parent != null)
            {
                transform.parent.GetComponent<AIMethods>().netting = true;
                Invoke("setNettingFalse", 5);
            }
        }
    }

    void setNettingFalse()
    {
        transform.parent.GetComponent<AIMethods>().netting = false;
    }

    void setMSBack()
    {
        transform.parent.GetComponent<AIMethods>().ms *= 2;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Slow"))
        {
            if (transform.parent != null)
            {
                transform.parent.GetComponent<AIMethods>().ms *= 2;
            }
        }
    }

    private void OnDestroy()
    {
        if (createSwill)
            GetComponent<Swill>().show(player.transform.position);
    }
}
