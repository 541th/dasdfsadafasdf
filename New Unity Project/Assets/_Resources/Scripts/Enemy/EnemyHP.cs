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

    [SerializeField] GameObject monetsPrefab;
    [SerializeField] bool isMonets, chest, dontBleeding;
    [SerializeField] int maxAmountMonets;

    [SerializeField] Material defaultMaterial, blinkMaterial;

    private void Start()
    {
        if (isBoss && PlayerPrefs.GetInt(transform.parent.name) == 1)
        {
            if (transform.parent.name == "Boss_5")
            {
                Destroy(transform.parent.gameObject);
                return;
            }

            GameObject.Find("PortalEndObject").transform.GetChild(0).gameObject.SetActive(true);

            transform.parent.position = new Vector3(PlayerPrefs.GetFloat(transform.parent.name + "x"), PlayerPrefs.GetFloat(transform.parent.name + "y"));

            if (transform.parent.GetComponent<Animator>() != null)
                transform.parent.GetComponent<Animator>().SetTrigger("Death");
            else
                Destroy(transform.parent.gameObject);

            return;
        }

        maxAmountMonets += FindObjectOfType<PlayerExp>().curLvl * 2;
        player = GameObject.Find("Player").transform;

        maxHP = maxHP + (maxHP / 10) * FindObjectOfType<PlayerExp>().curLvl;
        expForKill = !dontAddExp ? maxHP : 0;
        HP = maxHP;
    }

    [SerializeField] bool divide, createSwill, isGhost, dontAddExp;

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

    [SerializeField] bool dontFly;

    public void toDamage(int damage, bool isFlying, bool stan, bool bleeding, bool expl, bool sub, bool explMag)
    {
        if (bleeding && !dontBleeding) StartCoroutine(bleedingEvent());
        if (expl) explEvent();
        if (sub)
        {
            if (transform.parent.GetComponent<AIMethods>() != null)
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
            res = LanguageLines.getLine(12);
        }

        InfoController.addExp(damage / 2);
        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(res);

        slider.SetActive(true);

        slider.transform.GetChild(0).GetComponent<Slider>().maxValue = maxHP;
        slider.transform.GetChild(0).GetComponent<Slider>().value = HP;
        slider.transform.GetChild(0).GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, ((float)1 / maxHP) * HP);

        if (isFlying && !dontFly)
        {
            StartCoroutine(flying(player.position - transform.parent.position));
        }

        checkDeath();

        if (stan && transform.parent.GetComponent<AIMethods>() != null) transform.parent.GetComponent<AIMethods>().startStan();
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
                arrow.GetComponent<BoxCollider2D>().enabled = false;
                arrow.GetComponent<ArrowFly>().colliderEnabledTime = 0.16f;
                arrow.transform.position = transform.position;

                arrow.GetComponent<ArrowFly>().target = new Vector2(-i, j).normalized;
                arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(i, j) * 180 / Mathf.PI+ 90);
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
                res = LanguageLines.getLine(12);
        }

        InfoController.addExp(damage/2);
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
                res = LanguageLines.getLine(12);
        }
        InfoController.addExp(damage/2);

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
                res = LanguageLines.getLine(12);
        }
        InfoController.addExp(damage/2);

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

    public IEnumerator hitBlink()
    {
        if (transform.parent.GetComponent<SpriteRenderer>() != null)
            transform.parent.GetComponent<SpriteRenderer>().material = blinkMaterial;
        else
        {
            transform.parent.GetChild(0).GetComponent<SpriteRenderer>().material = blinkMaterial;
            transform.parent.GetChild(1).GetComponent<SpriteRenderer>().material = blinkMaterial;
        }
        
        yield return new WaitForSeconds(0.04f);

        if (transform.parent.GetComponent<SpriteRenderer>() != null)
            transform.parent.GetComponent<SpriteRenderer>().material = defaultMaterial;
        else
        {
            transform.parent.GetChild(0).GetComponent<SpriteRenderer>().material = defaultMaterial;
            transform.parent.GetChild(1).GetComponent<SpriteRenderer>().material = defaultMaterial;
        }
    }

    void stopSleeping()
    {
        if (transform.parent.GetComponent<AIMethods>() != null)
        {
            transform.parent.GetComponent<CapsuleCollider2D>().enabled = true;
            transform.parent.GetComponent<AIMethods>().stanned = false;
        }
    }

    [SerializeField] bool isBoss, isBossSleeping, isFinalCutscene;
    public bool subbed;
    void checkDeath()
    {
        if (isBossSleeping)
        {
            isBossSleeping = false;
            if (transform.parent.GetComponent<Animator>() != null)
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
                PlayerPrefs.SetInt(transform.parent.name, 1);
                PlayerPrefs.SetFloat(transform.parent.name + "x", transform.parent.position.x);
                PlayerPrefs.SetFloat(transform.parent.name + "y", transform.parent.position.y);

                FindObjectOfType<CamFollow>().stopCameraRotating();
                GameObject.Find("PortalEndObject").transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                if (!subbed && FindObjectOfType<EnemyManager>() != null)
                {
                    subbed = true;
                    FindObjectOfType<EnemyManager>().subCount();
                }
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

    [SerializeField] bool justDestr;
    void showDeath()
    {
        if (isMonets)
        {
            GameObject monets = Instantiate(monetsPrefab, transform.position + new Vector3(0, 1), Quaternion.identity);
            monets.GetComponent<MonetsEffect>().monets = true;
            monets.GetComponent<MonetsEffect>().enemy = true;
            monets.GetComponent<MonetsEffect>().amount = maxAmountMonets;
        }

        if (chest)
        {
            transform.parent.GetComponent<Chest>().destrEvent();
        }

        if (justDestr)
        {
            Destroy(transform.parent.gameObject);
            return;
        }

        if (isBoss)
        {
            FindObjectOfType<UIManager>().hideBossHPBar();

            PlayerPrefs.SetInt("level_" + (int.Parse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) + 1), 1);

            if (transform.parent.GetComponent<Animator>() != null)
                transform.parent.GetComponent<Animator>().SetTrigger("Death");
            else
                Destroy(transform.parent.gameObject);
        }
        else
            GetComponentInParent<AIMethods>().showDeath();

        Destroy(gameObject);
    }

    IEnumerator flying(Vector2 to)
    {
        float timer = 0.1f;
        float x = -to.x * Time.deltaTime * 1000, y = -to.y * Time.deltaTime * 1000;
        Rigidbody2D parent = transform.parent.GetComponent<Rigidbody2D>();

        if (parent == null) yield break;

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
        if (transform.parent.GetComponent<AIMethods>() != null)
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
            if (transform.parent != null && transform.parent.GetComponent<AIMethods>() != null)
            {
                transform.parent.GetComponent<AIMethods>().ms *= 2;
            }
        }
    }

    private void OnDestroy()
    {
        if (transform.parent.GetComponent<SpriteRenderer>() != null)
            transform.parent.GetComponent<SpriteRenderer>().material = defaultMaterial;
        else
        {
            transform.parent.GetChild(0).GetComponent<SpriteRenderer>().material = defaultMaterial;
            transform.parent.GetChild(1).GetComponent<SpriteRenderer>().material = defaultMaterial;
        }

        if (createSwill)
            GetComponent<Swill>().show(player.transform.position);
    }

    private void OnDisable()
    {
        if (transform.parent.GetComponent<SpriteRenderer>() != null)
            transform.parent.GetComponent<SpriteRenderer>().material = defaultMaterial;
        else
        {
            transform.parent.GetChild(0).GetComponent<SpriteRenderer>().material = defaultMaterial;
            transform.parent.GetChild(1).GetComponent<SpriteRenderer>().material = defaultMaterial;
        }
    }
}
