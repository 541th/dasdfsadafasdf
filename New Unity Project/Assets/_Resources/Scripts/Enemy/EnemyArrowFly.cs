﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrowFly : MonoBehaviour
{
    public Vector2 target, ms;
    [SerializeField]
    bool dontStop;
    bool fly;
    Rigidbody2D _rb;

    private void Start()
    {
        fly = true;
        StartCoroutine(StartDeath());
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (fly)
            _rb.velocity = (Vector3)target * Time.deltaTime * 1000;
        //transform.position += (Vector3)target * Time.deltaTime * 40;
        else
            _rb.velocity = Vector3.zero;
    }

    public void showDeath()
    {
        if (dontStop) return;
        StartCoroutine(InstantDeath());
    }

    IEnumerator StartDeath()
    {
        yield return new WaitForSeconds(3);
        if (dontStop)
        {
            Destroy(gameObject);
            yield break;
        }

        if (fly)
            StartCoroutine(InstantDeath());
    }

    IEnumerator InstantDeath()
    {
        fly = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Death");
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }

    public void stop()
    {
        if (fly)
        {
            if (dontStop) return;

            fly = false;
            StartCoroutine(InstantDeath());
            if (FindObjectOfType<PlayerMovement>().playerType == 3) FindObjectOfType<CamFollow>().startShakeArrow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (fly)
        {
            if (dontStop) return;

            fly = false;
            StartCoroutine(InstantDeath());
            if (FindObjectOfType<PlayerMovement>().playerType == 3) FindObjectOfType<CamFollow>().startShakeArrow();
        }
    }
}
