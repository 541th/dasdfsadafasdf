using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherController : MonoBehaviour
{
    [SerializeField] float startTimer, start, end;
    Transform _t;

    void Start()
    {
        _t = transform;
        StartCoroutine(pusherEvent());
        start = _t.localPosition.y;
    }

    IEnumerator pusherEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(6 + startTimer);

            _t.GetChild(0).gameObject.SetActive(true);

            while (_t.localPosition.y < end)
            {
                _t.localPosition += new Vector3(0, Time.deltaTime * 30, 0);
                yield return null;
            }

            _t.localPosition = new Vector2(_t.localPosition.x, end);

            yield return new WaitForSeconds(1);

            _t.GetChild(0).gameObject.SetActive(false);

            while (_t.localPosition.y > start)
            {
                _t.localPosition -= new Vector3(0, Time.deltaTime * 4, 0);
                yield return null;
            }

            _t.localPosition = new Vector2(_t.localPosition.x, start);
        }
    }
}
