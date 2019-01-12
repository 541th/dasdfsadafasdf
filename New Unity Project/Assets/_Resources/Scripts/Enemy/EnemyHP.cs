using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public int HP, maxHP;
    Transform player;
    [SerializeField] GameObject floatingNumbers, slider;
    [SerializeField] float expForKill;

    public bool damage;
    private void Update()
    {
        if (damage)
        {
            toDamageLightning(10);
            damage = false;
        }
    }

    public void toDamage(int damage, bool isFlying, bool stan)
    {
        HP -= damage;
        InfoController.addExp(expForKill / 10);
        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(damage + "");

        slider.SetActive(true);

        slider.transform.GetChild(0).GetComponent<Slider>().value = HP;
        slider.transform.GetChild(0).GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, ((float)1 / maxHP) * HP);

        if (isFlying)
        {
            if (player == null) player = GameObject.Find("Player").transform;
            StartCoroutine(flying(player.position - transform.parent.position));
        }

        if (HP <= 0)
        {
            if (player != null)
                player.GetComponent<PlayerExp>().addExp(expForKill);
            Destroy(transform.parent.gameObject);
        }

        if (stan) transform.parent.GetComponent<AIMethods>().startStan();
    }

    public void toDamageSlow(int damage, bool stan)
    {
        transform.parent.GetComponent<AIMethods>().ms /= 2;
        Invoke("setMSBack", 3);

        HP -= damage;
        InfoController.addExp(expForKill / 10);
        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(damage + "");

        slider.SetActive(true);

        slider.transform.GetChild(0).GetComponent<Slider>().value = HP;
        slider.transform.GetChild(0).GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, ((float)1 / maxHP) * HP);

        if (HP <= 0)
        {
            if (player != null)
                player.GetComponent<PlayerExp>().addExp(expForKill);
            Destroy(transform.parent.gameObject);
        }

        if (stan) transform.parent.GetComponent<AIMethods>().startStan();
    }

    public void toDamageLightning(int damage)
    {
        HP -= damage;
        InfoController.addExp(expForKill / 10);

        GameObject _l = Instantiate(Resources.Load("Prefabs/Arrows/ChainLightningTrigger") as GameObject, transform.position, Quaternion.identity);
        _l.GetComponent<ChainLightning>()._e.Add(gameObject);

        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(damage + "");

        slider.SetActive(true);

        slider.transform.GetChild(0).GetComponent<Slider>().value = HP;
        slider.transform.GetChild(0).GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, ((float)1 / maxHP) * HP);

        if (HP <= 0)
        {
            if (player != null)
                player.GetComponent<PlayerExp>().addExp(expForKill);
            Destroy(transform.parent.gameObject);
        }
    }

    public void toDamageLightning(int damage, List<GameObject> _e)
    {
        HP -= damage;
        InfoController.addExp(expForKill / 10);

        GameObject _le = Instantiate(Resources.Load("Prefabs/Effects/Lightning") as GameObject, transform.position, Quaternion.identity);
        Destroy(_le, 0.4f);

        GameObject _l = Instantiate(Resources.Load("Prefabs/Arrows/ChainLightningTrigger") as GameObject, transform.position, Quaternion.identity);
        _l.GetComponent<ChainLightning>()._e = _e;
        _l.GetComponent<ChainLightning>()._e.Add(gameObject);

        GameObject fn = Instantiate(floatingNumbers, transform.position + new Vector3(0, 0.4f), Quaternion.identity);
        fn.transform.GetChild(0).GetComponent<FloatingNumbers>().setText(damage + "");

        slider.SetActive(true);

        slider.transform.GetChild(0).GetComponent<Slider>().value = HP;
        slider.transform.GetChild(0).GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, ((float)1 / maxHP) * HP);

        if (HP <= 0)
        {
            if (player != null)
                player.GetComponent<PlayerExp>().addExp(expForKill);
            Destroy(transform.parent.gameObject);
        }
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
}
