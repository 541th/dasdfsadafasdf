using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    SpriteRenderer _sr;
    ParticleSystemRenderer _psr;
    Transform _t;
    public static int mult = 111;
    [SerializeField] float delta;
    [SerializeField] bool isParticle, dontSubParticle;
    float timer = 40;

    public bool dontChangeSortingOrger;

    //КТО-НИБУДЬ ИЗ БУДУЩЕГО, ПОСЛУШАЙ МЕНЯ - МНЕ ОЧЕНЬ ЖАЛЬ ЗА ЭТОТ КОД! ЗДЕСЬ ЭТОГО НЕ ДОЛЖНО БЫТЬ! Я НЕ ХОЧУ ЭТОГО ДЕЛАТЬ, НО МНЕ ЛЕНЬ СОЗДАВАТЬ НОВЫЙ СКРИПТ ТОЛЬКО РАДИ ОДНОЙ ФУНКЦИИ!
    //ДА И ВООБЩЕ СЕЙЧАС ШЕСТЬ УТРА, ПРОСТИ МЕНЯ
    //ЭТИМ Я ХОЧУ ПОКАЗАТЬ КАК МНЕ ДЕЙСТВИТЕЛЬНО ЖАЛЬ
    public void getPunchShake()
    {
        FindObjectOfType<CamFollow>().startSwordShake();
    }

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
        if (!dontChangeSortingOrger)
        {
            if (isParticle)
            {
                if (!dontSubParticle)
                {
                    timer -= Time.deltaTime * 4;
                    _psr.GetComponent<ParticleSystem>().emissionRate = timer;
                }

                _psr.sortingOrder = -(int)(_t.position.y * mult);
            }
            else
                _sr.sortingOrder = -(int)(_t.position.y * mult + delta);
        }
    }
}
