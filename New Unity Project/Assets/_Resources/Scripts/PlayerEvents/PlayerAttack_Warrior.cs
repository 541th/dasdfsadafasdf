using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Warrior : MonoBehaviour
{
    bool isAttacking;
    Animator _a;
    PlayerMovement _pm;

    private void Start()
    {
        _a = transform.GetComponent<Animator>();
        _pm = GetComponent<PlayerMovement>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) startAttack();

        if (Input.GetKeyUp(KeyCode.Space)) stopAttack();

        //_a.SetFloat("lastMoveX", _pm.lastMove.x);
        //_a.SetFloat("lastMoveY", _pm.lastMove.y);
    }

    public void startAttack()
    {
        if (!GetComponent<PlayerMovement>().isGlide && GetComponent<PlayerMovement>().dontMove)
        {
            isAttacking = true;
            _a.SetBool("attack", true);
        }
    }

    public void stopAttack()
    {
        //if (!GetComponent<PlayerMovement>().isGlide)
        {
            isAttacking = false;
            _a.SetBool("attack", false);
        }
    }

    public void skill_0()
    {
        _a.SetTrigger("skill_0");
    }

    public void skill_1()
    {
        _a.SetTrigger("skill_1");
    }

    public void skill_2()
    {
        _a.SetTrigger("skill_2");
    }
}
