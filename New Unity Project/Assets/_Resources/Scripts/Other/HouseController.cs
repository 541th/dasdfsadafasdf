using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    bool hide;
    GameObject _b, _r, _d; 

    void Start()
    {
        _b = transform.GetChild(0).gameObject;
        _r = transform.GetChild(1).gameObject;
        _d = transform.GetChild(2).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(_d);
        StartCoroutine(hideRoof());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(showRoof());
    }

    IEnumerator showRoof()
    {
        SpriteRenderer _sr = _r.GetComponent<SpriteRenderer>();

        while (_sr.color.a <= 1)
        {
            if (!hide)
                _sr.color += new Color(0, 0, 0, Time.deltaTime * 4);

            yield return null;
        }

        _sr.color = new Color(1, 1, 1, 1);

        yield return null;
    }

    IEnumerator hideRoof()
    {
        SpriteRenderer _sr = _r.GetComponent<SpriteRenderer>();
        hide = true;
        while(_sr.color.a >= 0)
        {
            _sr.color -= new Color(0, 0, 0, Time.deltaTime * 4);
            yield return null;
        }
        hide = false;
        _sr.color = new Color(1, 1, 1, 0);
    }
}
