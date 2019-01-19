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
    }

    [SerializeField] GameObject buttonPrefab;
    IEnumerator pull()
    {
        player = GameObject.Find("Player").transform;

        if (player.GetComponent<PlayerMovement>().dontMove) yield break;

        Transform _t = transform, child = _t.GetChild(0);

        float blinkTimer = 0;

        GameObject button = Instantiate(buttonPrefab);
        button.transform.SetParent(transform.parent);
    
        Vector2 target = -(transform.parent.position - (player.position + new Vector3(0, 1, 0)));

        player.GetComponent<PlayerMovement>().dontMove = true;

        if (player.GetComponent<PlayerAttack_Archer>() != null)
            player.GetComponent<PlayerAttack_Archer>().canAttack = false;

        //player.GetComponent<PlayerMovement>(). = true;
        float time;
        while (_t.localScale.y > .4f)
        {
            time = Time.deltaTime;
            blinkTimer += time;

            _t.localScale -= new Vector3(0, time, 0);
            _t.position -= (Vector3)target.normalized * time;

            player.position = child.position;
            button.transform.position = child.position + new Vector3(0, 1);

            if (blinkTimer > .4f) blinkTimer = 0;
            else
                if (blinkTimer > .2) button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            else
                button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

            yield return null;
        }

        while (_t.localScale.y > .4f)
        {
            if (blinkTimer > .4f) blinkTimer = 0;
            else
                if (blinkTimer > .2) button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            else
                button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

            yield return null;
        }
    }

    public void destr(GameObject button)
    {
        Destroy(gameObject);
        Destroy(button);
    }

    private void OnDestroy()
    {
        if (player != null)
            player.GetComponent<PlayerMovement>().dontMove = false;

        if (player.GetComponent<PlayerAttack_Archer>() != null)
            player.GetComponent<PlayerAttack_Archer>().canAttack = true;
    }
}
