using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class PlayerAttack_Archer : MonoBehaviour
{
    bool isAttacking;
    public bool isHotShot, isLightning;
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
        canAttack = true;
    }

    public bool canAttack;

    float h, v;
    private void Update()
    {
        if (!canAttack) return;

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
        else attackTimer = 0;

        // _a.SetFloat("lastMoveX", _pm.lastMove.x);
        // _a.SetFloat("lastMoveY", _pm.lastMove.y);
    }

    [SerializeField] CamFollow cam;
    void createArrow()
    {
        GameObject toInst;
        if (!isHotShot)
        {
            if (GetComponent<PlayerMovement>().playerType == 2) toInst = arrows[curArrow];
            else toInst = mag_arrows[curArrow];
        }
        else
        {
            toInst = Resources.Load("Prefabs/Arrows/HotShot_0") as GameObject;
            GameObject effect = Instantiate(Resources.Load("Prefabs/Effects/HotShotEffect") as GameObject);
            effect.transform.position = transform.position;
            Destroy(effect, 1);
            isHotShot = false;
        }

        GameObject arrow = Instantiate(toInst);
        arrow.transform.position = transform.position;

        arrow.GetComponent<ArrowFly>().target = new Vector2(h, v).normalized;
        arrow.transform.position = transform.position + new Vector3(0, 0.5f);
        arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(v, h) * 180 / Mathf.PI);

        arrow.GetComponent<PlayerAttacker>().isLightning = isLightning;
        isLightning = false;

        //SHAKING THE CAMERA
        if (GetComponent<PlayerMovement>().playerType == 2)
        {
            if (cam == null) cam = GameObject.Find("Main Camera").GetComponent<CamFollow>();
            cam.startShakeArrow();

            arrow.GetComponent<PlayerAttacker>().bleeding = Random.Range(1, 100) <= InfoController.perks[7].value;
            arrow.GetComponent<PlayerAttacker>().expl = Random.Range(1, 100) <= InfoController.perks[8].value;
        }
        else if (GetComponent<PlayerMovement>().playerType == 3)
        {
            arrow.GetComponent<PlayerAttacker>().sub = InfoController.perks[10].lvl != 0;
            arrow.GetComponent<PlayerAttacker>().explMag = Random.Range(1, 100) <= InfoController.perks[11].value;
        }
    }

    public void skill_8()
    {
        StartCoroutine(skill_8Event());
    }

    IEnumerator skill_8Event()
    {
        attackSpeed = 0.2f;
        yield return new WaitForSeconds(4);
        attackSpeed = 0.4f;
    }
}
