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

    public bool canMove, dontMove, startInBattleScene;
    bool isMoving;
    public Vector2 moveInput, lastMove;
    Animator _aUp, _aDown;

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

    public void setStartValues()
    {
        transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Player animators/" + playerType + "_Torso") as RuntimeAnimatorController;
        transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Player animators/" + playerType + "_Legs") as RuntimeAnimatorController;

        if (playerType == 0)
        {
            FindObjectOfType<UIManager>().items[1].transform.GetChild(0).gameObject.SetActive(false);
            FindObjectOfType<UIManager>().items[1].transform.GetChild(1).gameObject.SetActive(false);
            Destroy(transform.GetChild(0).GetChild(0).gameObject);
            Destroy(GetComponent<PlayerAttack_Warrior>());
            Destroy(GetComponent<PlayerAttack_Archer>());
            FindObjectOfType<CamFollow>().setCamAsUsuall();
        }
        else
        if (playerType == 1)
        {
            FindObjectOfType<UIManager>().items[1].transform.GetChild(0).gameObject.SetActive(true);
            FindObjectOfType<UIManager>().items[1].transform.GetChild(1).gameObject.SetActive(false);
            Destroy(GetComponent<PlayerAttack_Archer>());
            FindObjectOfType<CamFollow>().setCamAsUsuall();
        }
        else
        if (playerType == 2)
        {
            FindObjectOfType<UIManager>().items[1].transform.GetChild(0).gameObject.SetActive(false);
            FindObjectOfType<UIManager>().items[1].transform.GetChild(1).gameObject.SetActive(true);
            if (transform.GetChild(0) != null && transform.GetChild(0).childCount != 0)
                Destroy(transform.GetChild(0).GetChild(0).gameObject);
            Destroy(GetComponent<PlayerAttack_Warrior>());
        }
        else
        {
            FindObjectOfType<UIManager>().items[1].transform.GetChild(0).gameObject.SetActive(false);
            FindObjectOfType<UIManager>().items[1].transform.GetChild(1).gameObject.SetActive(true);
            if (transform.GetChild(0) != null && transform.GetChild(0).childCount != 0)
                Destroy(transform.GetChild(0).GetChild(0).gameObject);
            Destroy(GetComponent<PlayerAttack_Warrior>());
            FindObjectOfType<CamFollow>().setCamAsUsuall();
        }

        if (playerType == 3)
            ms = startMS;

        if (PlayerPrefs.GetInt("CutScene") != 1)
            FindObjectOfType<UIManager>().setAllItems(true);

        PlayerPrefs.SetInt("CutScene", 0);

        ms += InfoController.perks[5].value;

        _aUp = transform.GetChild(0).GetComponent<Animator>();
        _aDown = transform.GetChild(1).GetComponent<Animator>();
    }

    void Start()
    {
        _aUp = transform.GetChild(0).GetComponent<Animator>();
        _aDown = transform.GetChild(1).GetComponent<Animator>();

        _t = transform;
        GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
        _rb = GetComponent<Rigidbody2D>();

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "City" || startInBattleScene)
            Invoke("setStartValues", .1f);
    }

    float h, v;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) glide();

        if (isGlide || dontMove) return;

        h = CnInputManager.GetAxis("Horizontal");
        v = CnInputManager.GetAxis("Vertical");

        {
            if (h == 0 && v == 0)
                moveInput = Vector2.zero;
            
            moveInput = calcDir();

            _aDown.SetBool("run", isMoving);
            _aUp.SetBool("run", isMoving);

            if (moveInput != Vector2.zero)
            {
                _aDown.SetFloat("lmx", lastMove.x);
                _aDown.SetFloat("lmy", lastMove.y);
                _aDown.SetFloat("mx", moveInput.x);
                _aDown.SetFloat("my", moveInput.y);

                if (playerType <= 1)
                {
                    _aUp.SetFloat("mx", moveInput.x);
                    _aUp.SetFloat("my", moveInput.y);
                    _aUp.SetFloat("lmx", lastMove.x);
                    _aUp.SetFloat("lmy", lastMove.y);
                }

                _rb.velocity = new Vector2(
                    (moveInput.x) * ms * 60 * Time.deltaTime,
                    (moveInput.y) * ms * 60 * Time.deltaTime);

                if (isSubMS) _rb.velocity /= 3;

                isMoving = true;

                lastMove = moveInput;

                if (lastMove.x != 0 && lastMove.y != 0) lastMove.x = 0;
            }
            else
               isMoving = false;
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

        AttackModifiers.addModifier('*', 3);

        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).name == "Other")
            {
                other = transform.GetChild(i);
                break;
            }

        rage.transform.SetParent(other);
        rage.transform.localPosition = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(10);

        AttackModifiers.removeModifier('*', 3);
        inRage = false;
        Destroy(rage);
    }

    [HideInInspector]
    public bool dontAttack;
    IEnumerator windRun()
    {
        dontAttack = true;

        float timer = 6, spawnTimer = .2f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                spawnTimer = .2f;

                GameObject _0 = new GameObject();
                _0.transform.localScale = transform.localScale;
                _0.AddComponent<SpriteRenderer>();
                _0.AddComponent<AdjM>();
                _0.transform.position = transform.position;
                //_0.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
                _0.GetComponent<SpriteRenderer>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                _0.GetComponent<SpriteRenderer>().sortingLayerName = transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName;
                _0.GetComponent<SpriteRenderer>().sortingOrder = transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder;

                GameObject _1 = new GameObject();
                _1.transform.localScale = transform.localScale;
                _1.AddComponent<SpriteRenderer>();
                _1.AddComponent<AdjM>();
                _1.transform.position = transform.position;
                //_1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
                _1.GetComponent<SpriteRenderer>().sprite = transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
                _1.GetComponent<SpriteRenderer>().sortingLayerName = transform.GetChild(1).GetComponent<SpriteRenderer>().sortingLayerName;
                _1.GetComponent<SpriteRenderer>().sortingOrder = transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder;

                Destroy(_0, 1f);
                Destroy(_1, 1f);
            }

            yield return null;
        }

        dontAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Banner"))
        {
            FindObjectOfType<PlayerHP>().startBanner();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Banner"))
        {
            FindObjectOfType<PlayerHP>().stopBanner();
        }
    }
}
