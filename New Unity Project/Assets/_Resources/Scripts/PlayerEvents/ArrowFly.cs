using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFly : MonoBehaviour
{
    public Vector2 target, ms;
    public float colliderEnabledTime;
    [SerializeField] bool dontStop, hasParticle;
    public bool fly;
    Rigidbody2D _rb;

    private void Start()
    {
        fly = true;
        StartCoroutine(StartDeath());
        _rb = GetComponent<Rigidbody2D>();
    }

    public bool isMagArrow()
    {
        return hasParticle;
    }

    private void Update()
    {
        if (colliderEnabledTime > 0)
        {
            colliderEnabledTime -= Time.deltaTime;

            if (colliderEnabledTime <= 0)
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        if (fly)
        {
            if (hasParticle) transform.GetChild(0).eulerAngles = Vector3.zero;
            _rb.velocity = (Vector3)target * Time.deltaTime * 1000;
        }
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
            if (FindObjectOfType<PlayerMovement>() != null && FindObjectOfType<PlayerMovement>().playerType == 3) FindObjectOfType<CamFollow>().startShakeArrow();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (fly)
        {
            if (dontStop) return;

            fly = false;
            StartCoroutine(InstantDeath());
            if (FindObjectOfType<PlayerMovement>() != null && FindObjectOfType<PlayerMovement>().playerType == 3) FindObjectOfType<CamFollow>().startShakeArrow();
        }
    }
}
