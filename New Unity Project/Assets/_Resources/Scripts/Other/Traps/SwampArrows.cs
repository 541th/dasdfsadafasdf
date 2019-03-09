using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampArrows : MonoBehaviour
{
    [SerializeField] int type;
    [SerializeField] GameObject arrow;
    [SerializeField] Sprite _0, _1, _2;

    private void Start()
    {
        if (type == 0)
        {
            GetComponent<BoxCollider2D>().offset = new Vector2(2, 0);
            GetComponent<BoxCollider2D>().size = new Vector2(4, 1);
            GetComponent<SpriteRenderer>().sprite = _0;
        }
        else if (type == 1)
        {
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -2);
            GetComponent<BoxCollider2D>().size = new Vector2(1, 4);
            GetComponent<SpriteRenderer>().sprite = _1;
        }
        else
        {
            GetComponent<BoxCollider2D>().offset = new Vector2(-2, 0);
            GetComponent<BoxCollider2D>().size = new Vector2(4, 1);
            GetComponent<SpriteRenderer>().sprite = _2;
        }

        GetComponent<Animator>().SetFloat("type", type);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GetComponent<Animator>().SetTrigger("Start");
    }

    GameObject _arrow;
    void shot()
    {
        _arrow = Instantiate(arrow);
        _arrow.transform.position = transform.position;

        if (type == 0)
        {
            _arrow.GetComponent<EnemyArrowFly>().target = new Vector2(1, 0);
            _arrow.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (type == 1)
        {
            _arrow.GetComponent<BoxCollider2D>().enabled = false;
            Invoke("setArrowColliderTrue", 0.1f);
            _arrow.GetComponent<EnemyArrowFly>().target = new Vector2(0, -1);
            _arrow.transform.eulerAngles = new Vector3(0, 0, -90);
        }
        else
        {
            _arrow.GetComponent<EnemyArrowFly>().target = new Vector2(-1, 0);
            _arrow.transform.eulerAngles = new Vector3(0, 0, -180);
        }

    }

    void setArrowColliderTrue()
    {
        _arrow.GetComponent<BoxCollider2D>().enabled = true;
    }
}