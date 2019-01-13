using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    SpriteRenderer _sr;
    ParticleSystemRenderer _psr;
    Transform _t;
    public static int mult = 111;
    [SerializeField] bool isParticle;
    float timer = 40;

    private void Start()
    {
        if (!isParticle)
            _sr = GetComponent<SpriteRenderer>();
        else
            _psr = GetComponent<ParticleSystemRenderer>();

        _t = transform;
    }

    void Update()
    {
        if (isParticle)
        {
            timer -= Time.deltaTime * 4;
            _psr.GetComponent<ParticleSystem>().emissionRate = timer;
            _psr.sortingOrder = -(int)(_t.position.y * mult);
        }
        else
            _sr.sortingOrder = -(int)(_t.position.y * mult);
    }
}
