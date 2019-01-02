    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideSmoke : MonoBehaviour
{
    float mult1, mult2, timer;
    [SerializeField] Sprite[] _ss;
    SpriteRenderer _sr;
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        mult1 = Random.Range(0.6f, 1.6f);
        mult2 = Random.Range(0.6f, 1.6f);
        Destroy(gameObject, 1);
    }

    void Update()
    {
        timer += Time.deltaTime;
        transform.position += new Vector3(0, Time.deltaTime * mult1);
        transform.localScale += new Vector3(Time.deltaTime * 4 * mult2, Time.deltaTime * 4 * mult2);
        _sr.color -= new Color(0, 0, 0, Time.deltaTime);
        if (timer < 0.2f)
            _sr.sprite = _ss[0];
        else if (timer < 0.4f && timer >= 0.2f)
            _sr.sprite = _ss[1];
        else if (timer < 0.6f && timer >= 0.4f)
            _sr.sprite = _ss[2];
        else if (timer < 0.8f && timer >= 0.6f)
            _sr.sprite = _ss[3];
        else if (timer < 1f && timer >= 0.8f)
            _sr.sprite = _ss[4];
    }
}
