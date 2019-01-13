using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderAtStart : MonoBehaviour
{
    [SerializeField] bool isGrass, isTorch, isSpikes, isParticle;
    [SerializeField] float startTimer;

    void Start()
    {
        if (isParticle)
        {
            GetComponent<ParticleSystemRenderer>().sortingOrder = -(int)(transform.position.y * SortingOrder.mult);
            return;
        }

        if (isSpikes)
        {
            GetComponent<Animator>().enabled = false;
            StartCoroutine(showSpikes());
            return;
        }

        if (isTorch)
        {
            GetComponent<Animator>().speed = Random.Range(0.8f, 1.6f);
            return;
        }

        if (isGrass)
            GetComponent<Animator>().speed = Random.Range(0.4f, 1.1f);
        GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * SortingOrder.mult);
    }

    IEnumerator showSpikes()
    {
        yield return new WaitForSeconds(startTimer);
        GetComponent<Animator>().enabled = true;
    }
}
