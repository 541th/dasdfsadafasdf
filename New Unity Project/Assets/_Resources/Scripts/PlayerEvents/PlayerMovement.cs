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

    bool isSubMS;
    public void subMS(float value)
    {
        StartCoroutine(subMSEvent(value));
    }

    IEnumerator subMSEvent(float value)
    {
        isSubMS = true;

        yield return new WaitForSeconds(value);

        isSubMS = false;
    }

    public void updadeMS()
    {
        ms = startMS + InfoController.perks[5].value;
    }

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
            FindObjectOfType<CamFollow>().setCamAsUsuall();
        }
        else
        if (playerType == 1)
        {
            GameObject.Find("ButtonAttackType").transform.GetChild(1).gameObject.SetActive(false);  
            Destroy(GetComponent<PlayerAttack_Archer>());
            FindObjectOfType<CamFollow>().setCamAsUsuall();
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
            FindObjectOfType<CamFollow>().setCamAsUsuall();
        }

        if (playerType == 3)
            ms = startMS;

        ms += InfoController.perks[5].value;

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

        {

            isMoving = false;

            if (h == 0 && v == 0)
                moveInput = Vector2.zero;
            
            moveInput = calcDir();

            if (moveInput != Vector2.zero)
            {
                _rb.velocity = new Vector2(
                    (moveInput.x) * ms * 60 * Time.deltaTime,
                    (moveInput.y) * ms * 60 * Time.deltaTime);

                if (isSubMS) _rb.velocity /= 3;

                isMoving = true;

                lastMove = moveInput;

                if (lastMove.x != 0 && lastMove.y != 0) lastMove.x = 0;
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
        updadeMS();

        Vector2 _n = new Vector2(h, v).normalized;

        if (Mathf.Abs(h) < Mathf.Abs(_n.x) / 4 && Mathf.Abs(v) < Mathf.Abs(_n.y) / 4) ms /= 2f;

        Vector2 res = new Vector2(Mathf.RoundToInt(_n.x), Mathf.RoundToInt(_n.y));

        if (res.x != 0 && res.y != 0) res /= 1.5f;

        return res;
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
        if ((h != 0 || v != 0) && !isGlide && !dontMove)
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

        Vector2 target = new Vector2(h, v).normalized;

        while (_t > 0)
        {
            Instantiate(smoke, transform.position - new Vector3(0, 0.4f), Quaternion.identity);
            _t -= Time.deltaTime;

            _rb.velocity = new Vector3(target.x * (80 + InfoController.perks[9].value), target.y * (80 + InfoController.perks[9].value), 0);

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
