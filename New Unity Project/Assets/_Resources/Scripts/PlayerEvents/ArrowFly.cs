using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFly : MonoBehaviour
{
    public Vector2 target, ms;
    bool fly;

    private void Start()
    {
        fly = true;
        StartCoroutine(StartDeath());
    }

    private void Update()
    {
        if (fly)
            transform.position += (Vector3)target * Time.deltaTime * 40;
    }

    public void showDeath()
    {
        StartCoroutine(InstantDeath());
    }

    IEnumerator StartDeath()
    {
        yield return new WaitForSeconds(3);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (fly)
        {
            if (FindObjectOfType<PlayerMovement>().playerType == 3) FindObjectOfType<CamFollow>().startShakeArrow();
            StartCoroutine(InstantDeath());
        }
    }
}
