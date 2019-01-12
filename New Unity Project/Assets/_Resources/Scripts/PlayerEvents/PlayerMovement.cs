using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class PlayerMovement : MonoBehaviour
{
    float ms;
    [SerializeField] float startMS;
    public int playerType;
    Transform _t;
    Rigidbody2D _rb;

    public bool canMove, dontMove;
    bool isMoving;
    public Vector2 moveInput, lastMove;
    Animator _a;
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //foreach (Vector2 item in path)
        {
            Gizmos.DrawCube(FindObjectOfType<GridOfNodes>().GetNodeByPos(transform.position).pos, new Vector3(0.5f, 0.5f, 0.5f));
        }
    }*/

    void Start()
    {
        _a = GetComponent<Animator>();
        if (playerType == 0)
        {
            GameObject.Find("ButtonAttackType").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("ButtonAttackType").transform.GetChild(1).gameObject.SetActive(false);
            Destroy(transform.GetChild(0).GetChild(0).gameObject);
            Destroy(GetComponent<PlayerAttack_Warrior>());
            Destroy(GetComponent<PlayerAttack_Archer>());
        }
        else
        if (playerType == 1)
        {
            GameObject.Find("ButtonAttackType").transform.GetChild(1).gameObject.SetActive(false);  
            Destroy(GetComponent<PlayerAttack_Archer>());
        }
        else
        if (playerType == 2)
        {
            GameObject.Find("ButtonAttackType").transform.GetChild(0).gameObject.SetActive(false);
            Destroy(transform.GetChild(0).GetChild(0).gameObject);
            Destroy(GetComponent<PlayerAttack_Warrior>());
        }
        else
        {
            GameObject.Find("ButtonAttackType").transform.GetChild(0).gameObject.SetActive(false);
            Destroy(transform.GetChild(0).GetChild(0).gameObject);
            Destroy(GetComponent<PlayerAttack_Warrior>());
        }

        if (playerType == 3)
            ms = startMS;
        _t = transform;
        GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
        _rb = GetComponent<Rigidbody2D>();
    }

    float h, v;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) glide();

        if (isGlide || dontMove) return;

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
                _rb.velocity = new Vector2(
                    (moveInput.x) * ms * 60 * Time.deltaTime,
                    (moveInput.y) * ms * 60 * Time.deltaTime);

                //_t.position += new Vector3((moveInput.x) * ms * Time.deltaTime, (moveInput.y) * ms * Time.deltaTime, 0);
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

            _a.SetFloat("mx", moveInput.x);
            _a.SetFloat("my", moveInput.y);
            _a.SetBool("run", isMoving);
            _a.SetFloat("lmx", lastMove.x);
            _a.SetFloat("lmy", lastMove.y);
        }
    }

    Vector2 calcDir()
    {
        float mult = Mathf.Sin(h) * Mathf.Cos(v);

        ms = startMS;
        
        if (Mathf.Abs(Mathf.Cos(v)) > 0.90f && Mathf.Abs(Mathf.Sin(h)) < 0.30f)
        {
            ms /= 2f;
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

    public bool isGlide;
    public void glide()
    {
        if ((h != 0 || v != 0) && !isGlide)
        {
            FindObjectOfType<UIManager>().glideFalse();
            StartCoroutine(glideReturn());
            StartCoroutine(startGlide());
        }
    }

    [SerializeField] GameObject smoke;

    IEnumerator startGlide()
    {
        isGlide = true;
        float _t = 0.07f;
        Rigidbody2D _rb = GetComponent<Rigidbody2D>();

        while (_t > 0)
        {
            Instantiate(smoke, transform.position - new Vector3(0, 0.4f), Quaternion.identity);
            _t -= Time.deltaTime;

            _rb.velocity = new Vector3(h * 80, v * 80, 0);

            yield return null;
        }

        isGlide = false;

        _t = 0.2f;

        while (_t > 0)
        {
            Instantiate(smoke, transform.position - new Vector3(0, 0.4f), Quaternion.identity);
            _t -= Time.deltaTime;

            yield return null;
        }

    }

    public float glideTimer;
    IEnumerator glideReturn()
    {
        float timer = glideTimer;

        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        FindObjectOfType<UIManager>().glideReturn();
    }

    public void skill_4()
    {
        StartCoroutine(rage());
    }

    public void skill_5()
    {
        StartCoroutine(windRun());
    }

    [HideInInspector]
    public bool inRage;
    IEnumerator rage()
    {
        inRage = true;
        GameObject rage = Instantiate(Resources.Load("Prefabs/Effects/PlayerRage") as GameObject);
        Transform other = null;

        transform.GetChild(0).GetChild(0).GetComponent<PlayerAttacker>().addModifier('*', 3);

        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).name == "Other")
            {
                other = transform.GetChild(i);
                break;
            }

        rage.transform.SetParent(other);
        rage.transform.localPosition = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(10);

        transform.GetChild(0).GetChild(0).GetComponent<PlayerAttacker>().removeModifier('*', 3);
        inRage = false;
        Destroy(rage);
    }

    [HideInInspector]
    public bool dontAttack;
    IEnumerator windRun()
    {
        dontAttack = true;
        yield return new WaitForSeconds(6);
        dontAttack = false;
    }
}
