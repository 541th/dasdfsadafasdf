using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public enum Totem_type { Exp, Heal, Damage };
    public Totem_type type;
    [SerializeField] GameObject button, particle_0, particle_1;
    bool used, playerInTrigger;

    public void use()
    {
        playerInTrigger = true;
        used = true;

        if (type == Totem_type.Exp)
        {
            Destroy(GetComponent<CircleCollider2D>());
            FindObjectOfType<PlayerExp>().doubleExp();
        }
        else if (type == Totem_type.Heal)
        {
            StartCoroutine(healEvent());
        }
        else if (type == Totem_type.Damage)
        {
            Destroy(GetComponent<CircleCollider2D>());
            AttackModifiers.addModifier('*', 2);
            StartCoroutine(removeModifier());
        }

        Destroy(button.transform.parent.gameObject);

        StartCoroutine(endParticle());
    }

    IEnumerator removeModifier()
    {
        float timer = 10;
        yield return new WaitForSeconds(10);
        AttackModifiers.removeModifier('*', 2);
    }

    IEnumerator healEvent()
    {
        PlayerHP _php = FindObjectOfType<PlayerHP>();

        float timer = 10;

        ParticleSystem particle = particle_1.GetComponent<ParticleSystem>();
        particle.emissionRate = 0;
        particle_1.SetActive(true);

        while (particle.emissionRate < 20)
        {
            particle.emissionRate++;
            yield return new WaitForSeconds(0.05f);
        }

        float healTimer = 0;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            healTimer += Time.deltaTime;

            if (healTimer > 0.2f)
            {
                if (playerInTrigger) _php.toHeal(_php.getMaxHP() / 200);

                healTimer = 0;
            }

            yield return null;
        }

        while (particle.emissionRate > 0)
        {
            particle.emissionRate--;
            yield return new WaitForSeconds(0.05f);
        }

        particle_1.SetActive(false);
    }

    IEnumerator endParticle()
    {
        float timer = 0.3f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            particle_0.transform.localScale += new Vector3(Time.deltaTime * 1, Time.deltaTime * 1);
            yield return null;
        }

        Destroy(particle_0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!used)
                button.SetActive(true);
            else
            {
                playerInTrigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!used)
            {
                button.SetActive(false);
            }
            else
            {
                playerInTrigger = false;
            }
        }
    }
}
