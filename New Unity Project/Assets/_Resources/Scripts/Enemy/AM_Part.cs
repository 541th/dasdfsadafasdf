using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AM_Part : MonoBehaviour
{
    Vector2 target;
    [SerializeField] GameObject particle;

    private void Start()
    {
        target = GameObject.Find("Player").transform.position - transform.position;
        StartCoroutine(mv());
    }

    IEnumerator mv()
    {
        float timer = Random.Range(.4f, .6f);
        float startRot = Random.Range(0, 10);
        float rotSpeed = Random.Range(10, 100);
        target = target.normalized + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        while (timer > 0)
        {
            float deltaTime = Time.deltaTime;

            if (startRot > 0)
                startRot -= deltaTime * rotSpeed;

            transform.eulerAngles += new Vector3(0, 0, startRot);
            if (particle != null)
            {
                particle.transform.eulerAngles -= new Vector3(0, 0, startRot);
                particle.GetComponentInChildren<ParticleSystem>().emissionRate = timer * 60;
            }

            timer -= deltaTime;
            target /= (1 + deltaTime);
            transform.position -= (Vector3)target * deltaTime * 10;
            yield return null;
        }

        Destroy(gameObject, 10);
    }
}
