using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class PlayerAttack_Archer : MonoBehaviour
{
    bool isAttacking;
    int curArrow = 0;
    [SerializeField] float attackSpeed, attackTimer;
    [SerializeField] GameObject[] arrows;
    [SerializeField] GameObject[] mag_arrows;
    Animator _a;
    PlayerMovement _pm;

    private void Start()
    {
        _a = transform.GetChild(0).GetComponent<Animator>();
        _pm = GetComponent<PlayerMovement>();
    }
    
    float h, v;
    private void Update()
    {
        h = CnInputManager.GetAxis("Attack_H");
        v = CnInputManager.GetAxis("Attack_V");

        if (h != 0 || v != 0)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackSpeed)
            {
                attackTimer = 0;

                createArrow();
            }
        }
        
        // _a.SetFloat("lastMoveX", _pm.lastMove.x);
        // _a.SetFloat("lastMoveY", _pm.lastMove.y);
    }

    [SerializeField] CamFollow cam;
    void createArrow()
    {
        GameObject toInst;
        if (GetComponent<PlayerMovement>().playerType == 2) toInst = arrows[curArrow];
        else toInst = mag_arrows[curArrow];

        GameObject arrow = Instantiate(toInst);
        arrow.transform.position = transform.position;

        arrow.GetComponent<ArrowFly>().target = new Vector2(h, v).normalized;
        arrow.transform.position = transform.position + new Vector3(0, 0.5f);
        arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(v, h) * 180 / Mathf.PI);

        //SHAKING THE CAMERA
        if (GetComponent<PlayerMovement>().playerType == 2)
        {
            if (cam == null) cam = GameObject.Find("Main Camera").GetComponent<CamFollow>();
            cam.startShakeArrow();
        }
    }

    public void startAttack()
    {
        if (!GetComponent<PlayerMovement>().isGlide)
        {
            //isAttacking = true;
            //_a.SetBool("isAttacking", true);
        }
    }

    public void stopAttack()
    {
        //if (!GetComponent<PlayerMovement>().isGlide)
        {
            //isAttacking = false;
            //_a.SetBool("isAttacking", false);
        }
    }
}
