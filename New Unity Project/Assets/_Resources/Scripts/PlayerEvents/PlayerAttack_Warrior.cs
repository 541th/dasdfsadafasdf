using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Warrior : MonoBehaviour
{
    bool isAttacking;
    Animator _aUp, _aDown;
    PlayerMovement _pm;
    CamFollow cam;

    private void Start()
    {
        cam = FindObjectOfType<CamFollow>();
        _aUp = transform.GetChild(0).GetComponent<Animator>();
        _aDown = transform.GetChild(1).GetComponent<Animator>();
        _pm = GetComponent<PlayerMovement>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) startAttack();

        if (Input.GetKeyUp(KeyCode.Space)) stopAttack();
    }

    public void startAttack()
    {
        if (!GetComponent<PlayerMovement>().isGlide && !GetComponent<PlayerMovement>().dontMove)
        {
            isAttacking = true;
            _aUp.SetBool("attack", true);
        }
    }

    public void stopAttack()
    {
        //if (!GetComponent<PlayerMovement>().isGlide)
        {
            isAttacking = false;
            _aUp.SetBool("attack", false);
        }
    }

    public void shakeCam()
    {
        cam.startShakeArrow();
    }

    public void skill_0()
    {
        _aUp.SetTrigger("skill_0");
        _aDown.SetTrigger("skill_0");
    }

    public void skill_1()
    {
        _aUp.SetTrigger("skill_1");
        _aDown.SetTrigger("skill_1");
    }

    public void skill_2()
    {
        _aUp.SetTrigger("skill_1");
        _aDown.SetTrigger("skill_2");
    }
}
