using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class PlayerMovement : MonoBehaviour
{
    float ms;
    [SerializeField] float startMS;
    Transform _t;

    public bool canMove;
    bool isMoving;
    public Vector2 moveInput, lastMove;

    //[SerializeField] Rigidbody2D _rb;

    void Start()
    {
        ms = startMS;
        _t = transform;
        //_rb = GetComponent<Rigidbody2D>();
    }

    float h, v;
    void Update()
    {
        h = CnInputManager.GetAxis("Horizontal");
        v = CnInputManager.GetAxis("Vertical");

        if (!canMove)
        {
            //anim.SetBool("AttackPose", false);
            //anim.SetBool("Moving", false);

            //attacking = false;
            //_rb.velocity = Vector2.zero;

            //if ((Input.GetKeyUp(KeyCode.M) || CnInputManager.GetButtonUp("Menu")) && !GameObject.Find("Game menu canvas").transform.GetChild(0).gameObject.activeSelf)
            //{
                // clОse menu
            //}
            
            return;
        }
        else
        {

            //if ((Input.GetKeyUp(KeyCode.M) || CnInputManager.GetButtonUp("Menu")) && !GameObject.Find("Game menu canvas").transform.GetChild(0).gameObject.activeSelf)
            //{
                //оpen menu
            //}

            isMoving = false;

            if (h == 0 && v == 0)
                moveInput = Vector2.zero;
            
            moveInput = calcDir();

            //if (Input.GetKeyDown(KeyCode.Space)) attackButtonDown();

            //if (Input.GetKeyUp(KeyCode.Space)) attackButtonUp();

            if (moveInput != Vector2.zero)
            {
                //_rb.velocity = new Vector2(
                //    (moveInput.x) * ms * Time.deltaTime,
                //    (moveInput.y) * ms * Time.deltaTime);

                _t.position += new Vector3((moveInput.x) * ms * Time.deltaTime, (moveInput.y) * ms * Time.deltaTime, 0);
                isMoving = true;

                lastMove = moveInput;

                if (lastMove.x != 0 && lastMove.y != 0) lastMove.x = 0;
            }
            else
            {
                //_rb.velocity = Vector2.zero;
                //anim.SetBool("Moving", false);
                //anim.SetBool("Walk", false);
            }

            //anim.SetFloat("MoveX", moveInput.x);
            //anim.SetFloat("MoveY", moveInput.y);
            //anim.SetBool("Moving", playerMoving);
            //anim.SetFloat("LastMoveX", lastMove.x);
            //anim.SetFloat("LastMoveY", lastMove.y);
        }
    }

    Vector2 calcDir()
    {
        float mult = Mathf.Sin(h) * Mathf.Cos(v);

        ms = startMS;

        //anim.SetBool("Moving", true);
       // anim.SetBool("Walk", false);

        if (Mathf.Abs(Mathf.Cos(v)) > 0.90f && Mathf.Abs(Mathf.Sin(h)) < 0.30f)
        {
            ms /= 2f;
            //anim.SetBool("Moving", false);
            //anim.SetBool("Walk", true);
        }

        if (Mathf.Abs(Mathf.Cos(v)) > 0.96f && Mathf.Abs(Mathf.Sin(h)) < 0.10f)
            ms = startMS;
        
        if (returnSign(h) != 0
            && Mathf.Abs(v) < 0.2f
            && Mathf.Abs(h) > 0.2f)
            return new Vector2(returnSign(h) * 1, 0);

        if (mult > -0.25f && mult < 0.25f) return new Vector2(0, 1 * returnSign(v));

        if (mult > 0.25f && mult < 0.67f) return new Vector2(1, 1 * returnSign(v));

        if (mult > -0.67f && mult < -0.25f) return new Vector2(-1, 1 * returnSign(v));
        
        return new Vector2(returnSign(h) * 1, 0);
    }

    public float returnSign(float value)
    {
        if (value > 0) return 1;
        if (value == 0) return 0;

        return -1;
    }

}
