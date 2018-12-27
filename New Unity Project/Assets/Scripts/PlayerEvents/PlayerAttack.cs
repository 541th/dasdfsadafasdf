using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    bool isAttacking;
    Animator _a;
    PlayerMovement _pm;

    private void Start()
    {
        _a = transform.GetChild(0).GetComponent<Animator>();
        _pm = GetComponent<PlayerMovement>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) startAttack();

        if (Input.GetKeyUp(KeyCode.Space)) stopAttack();

        _a.SetFloat("lastMoveX", _pm.lastMove.x);
        _a.SetFloat("lastMoveY", _pm.lastMove.y);
    }

    public void startAttack()
    {
        isAttacking = true;
        _a.SetBool("isAttacking", true);
    }

    public void stopAttack()
    {
        isAttacking = false;
        _a.SetBool("isAttacking", false);
    }
}
