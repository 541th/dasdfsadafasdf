using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tongue : MonoBehaviour
{
    Transform player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.parent.GetComponent<Smoker_0>().tongued = true;
            StartCoroutine(pull());
        }
        else if (collision.CompareTag("Collisions"))
        {
            transform.parent.GetComponent<Smoker_0>().stopTongue();
        }
    }

    [SerializeField] GameObject buttonPrefab;
    IEnumerator pull()
    {
        player = GameObject.Find("Player").transform;

        if (player.GetComponent<PlayerMovement>().dontMove) yield break;

        Transform _t = transform;

        float blinkTimer = 0, height = 1.4f;

        GameObject button = Instantiate(buttonPrefab);
        button.transform.SetParent(_t.parent);
        button.transform.position = player.transform.position;
        FindObjectOfType<UIManager>().addItemToGameButtons(button);

        Vector2 target = -(_t.parent.position - player.position);

        player.GetComponent<PlayerMovement>().dontMove = true;

        if (player.GetComponent<PlayerAttack_Archer>() != null)
            player.GetComponent<PlayerAttack_Archer>().canAttack = false;

        PlayerExp _pe = FindObjectOfType<PlayerExp>();

        while (Vector3.SqrMagnitude(_t.parent.position - (_t.parent.position + (Vector3)target)) > 3f)
        {
            blinkTimer += Time.deltaTime;

            target -= target * Time.deltaTime * .1f;

            _t.position = _t.parent.position + (Vector3)target + new Vector3(0, height);

            GetComponent<LineRenderer>().SetPosition(0, _t.parent.position + new Vector3(0, height));
            GetComponent<LineRenderer>().SetPosition(1, _t.parent.position + (Vector3)target);

            player.position = GetComponent<LineRenderer>().GetPosition(1);
            button.transform.position = player.position + new Vector3(0, height);

            if (blinkTimer > .4f)
            {
                FindObjectOfType<PlayerHP>().toDamage(Random.Range(1, 10) + _pe.getKoefByLvl());
                blinkTimer = 0;
            }
            else
                if (blinkTimer > .2) button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            else
                button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

            yield return null;
        }

        while (true)
        {
            blinkTimer += Time.deltaTime;

            if (blinkTimer > .4f)
            {
                FindObjectOfType<PlayerHP>().toDamage(Random.Range(1, 10) + _pe.getKoefByLvl());
                blinkTimer = 0;
            }
            else
                if (blinkTimer > .2) button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            else
                button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

            yield return null;
        }
    }

    public void destr(GameObject button)
    {
        if (transform.parent.GetComponent<Smoker_0>() != null)
            transform.parent.GetComponent<Smoker_0>().stopTongue();

        FindObjectOfType<UIManager>().removeItemFromGameButtons(button);

        Destroy(gameObject);
        Destroy(button);
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().dontMove = false;

            if (player.GetComponent<PlayerAttack_Archer>() != null)
                player.GetComponent<PlayerAttack_Archer>().canAttack = true;
        }
    }
}
