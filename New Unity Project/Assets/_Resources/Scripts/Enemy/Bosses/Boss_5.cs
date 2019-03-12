using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_5 : MonoBehaviour
{
    Transform _t;

    Animator _a;
    AIMethods AI;
    GameObject player;

    int points;

    float delay, delta;
    GridOfNodes gon;

    int actionType;

    List<Vector2> path = new List<Vector2>();

    void Start()
    {
        _t = transform;
        AI = GetComponent<AIMethods>();
        _a = GetComponent<Animator>();
        player = GameObject.Find("Player");
        if (gon == null)
            gon = FindObjectOfType<GridOfNodes>();

        float sin, cos;
        Vector2 _n, res;

        for (int i = 0; i < partsParent.childCount; i++)
        {
            if (partsParent.GetChild(i).gameObject.activeSelf)
            {
                sin = Mathf.Sin(i * 70); cos = Mathf.Cos(i * 70) / 1.2f;

                _n = new Vector2(cos, sin).normalized;
                res = new Vector2(Mathf.RoundToInt(_n.x), Mathf.RoundToInt(_n.y));

                partsParent.GetChild(i).transform.position = _t.position + new Vector3(cos, sin);
                partsParent.GetChild(i).GetComponent<SpriteRenderer>().sprite = sprites[(int)((res.x + 1) + (res.y + 1) * 3)];
            }
        }
    }

    [SerializeField] Sprite[] sprites = new Sprite[9];
    bool rotate = true;

    [SerializeField] Transform partsParent;
    IEnumerator rotateParts()
    {
        rotationStarted = true;
        float timer = 0, sin, cos;
        Vector2 _n, res;

        while (true)
        {
            if (rotate)
            {
                timer += Time.deltaTime;

                for (int i = 0; i < partsParent.childCount; i++)
                {
                    if (partsParent.GetChild(i).gameObject.activeSelf)
                    {
                        sin = Mathf.Sin(timer + i * 70) * Mathf.Clamp(timer, 1f, 2); cos = Mathf.Cos(timer + i * 70) * Mathf.Clamp(timer, 0.8f, 3);

                        _n = new Vector2(cos, sin).normalized;
                        res = new Vector2(Mathf.RoundToInt(_n.x), Mathf.RoundToInt(_n.y));

                        partsParent.GetChild(i).transform.position = _t.position + new Vector3(cos, sin);
                        partsParent.GetChild(i).GetComponent<SpriteRenderer>().sprite = sprites[(int)((res.x + 1) + (res.y + 1) * 3)];
                    }
                }
            }

            yield return null;
        }
    }
    
    float shootTimer = 0;
    bool shooted;

    Vector2 playerRandomPoint;

    bool rotationStarted;

    void Update()
    {
        if (AI.stanned) return;

        if (!rotationStarted) StartCoroutine(rotateParts());

        if (!makingAction)
            if (actionType == 0)
            {
                delay += Time.deltaTime;
                delta -= Time.deltaTime;
                shootTimer -= Time.deltaTime;

                if (delta <= 0)
                {
                    shooted = false;
                    //if (Vector2.Distance(transform.position, ((targets.Count == 0) ? player.transform.position : targets[0].transform.position)) > 2)
                    {
                        playerRandomPoint = new Vector2(player.transform.position.x + Random.Range(-6f, 6f),
                            player.transform.position.y + Random.Range(-6f, 6f));

                        if (gon.GetNodeByPos(playerRandomPoint) != null)
                            while (!gon.GetNodeByPos(playerRandomPoint).walkable)
                            {
                                playerRandomPoint = new Vector2(player.transform.position.x + Random.Range(-6f, 6f),
                                    player.transform.position.y + Random.Range(-6f, 6f));
                            }
                        else
                        {
                            playerRandomPoint = player.transform.position;
                        }

                        playerRandomPoint = gon.GetNodeByPos(playerRandomPoint).pos;
                    }

                    delta = 4;
                }

                if (delay >= Random.Range(0, 0.3f))
                {
                    path = AI.setDestination(transform.position, playerRandomPoint);
                    points = path.Count - 1;
                    delay = 0;
                }

                if (AI.netting) return;

                if (path != null)
                    if (path.Count != 0)
                    {
                        _t.position = Vector2.MoveTowards(_t.position, path[points], AI.ms * Time.deltaTime);

                        if (points == 0 && !shooted)
                        {
                            shooted = true;

                            int action = Random.Range(0, 1+4);

                            if (action == 0)
                                StartCoroutine(action_0());
                            else if (action == 1)
                                StartCoroutine(action_1());
                            else if (action == 2)
                                StartCoroutine(action_2());
                            else if (action == 3)
                                StartCoroutine(action_3());
                            else if (action == 4)
                                action_4();

                            return;
                        }

                        if (Vector2.Distance(_t.position, path[points]) < 0.1f && points != 0)
                        {
                            points--;

                            if (points <= 0) return;
                        }
                    }
            }
    }

    bool makingAction;
    IEnumerator action_0()
    {
        makingAction = true;
        rotate = false;

        for (int i = 0; i < partsParent.childCount; i++)
            if (partsParent.GetChild(i).gameObject.activeSelf)
                partsParent.GetChild(i).GetChild(0).gameObject.SetActive(true);

        float timer = 0;
        while (timer < .1f)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < partsParent.childCount; i++)
            {
                if (partsParent.GetChild(i).gameObject.activeSelf)
                {
                    partsParent.GetChild(i).transform.position -= (_t.position - partsParent.GetChild(i).position) * Time.deltaTime * 8;
                }
            }

            yield return null;
        }

        for (int i = 0; i < partsParent.childCount; i++)
            if (partsParent.GetChild(i).gameObject.activeSelf)
                partsParent.GetChild(i).GetChild(0).gameObject.SetActive(false);

        yield return new WaitForSeconds(0.8f);

        timer = 0.1f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            for (int i = 0; i < partsParent.childCount; i++)
            {
                if (partsParent.GetChild(i).gameObject.activeSelf)
                {
                    partsParent.GetChild(i).transform.position += (_t.position - partsParent.GetChild(i).position) * Time.deltaTime * 8;
                }
            }

            yield return null;
        }
        rotate = true;
        makingAction = false;
    }

    [SerializeField] GameObject ExplPrefab;
    IEnumerator action_1()
    {
        makingAction = true;
        rotate = false;

        for (int i = 0; i < partsParent.childCount; i++)
        {
            float timer = .02f;

            while (timer > 0)
            {
                timer -= Time.deltaTime;

                if (partsParent.GetChild(i).gameObject.activeSelf)
                {
                    partsParent.GetChild(i).transform.position -= new Vector3(0, 1) * Time.deltaTime * 18;
                }

                yield return null;
            }

            GameObject expl = Instantiate(ExplPrefab, partsParent.GetChild(i).position, Quaternion.identity);
            Destroy(expl, 0.2f);

            yield return new WaitForSeconds(.2f);
        }

        yield return new WaitForSeconds(2);

        for (int i = 0; i < partsParent.childCount; i++)
        {
            float timer = .02f;

            while (timer > 0)
            {
                timer -= Time.deltaTime;

                if (partsParent.GetChild(i).gameObject.activeSelf)
                {
                    partsParent.GetChild(i).transform.position += new Vector3(0, 1) * Time.deltaTime * 18;
                }

                yield return null;
            }
        }

        rotate = true;
        makingAction = false;
    }

    PlayerHP _php;
    IEnumerator action_2()
    {
        float damTimer = 0;
        float timer = 0;
        int lm = LayerMask.GetMask("Player", "Collisions");

        if (_php == null)
            _php = FindObjectOfType<PlayerHP>();

        List<GameObject> lasers = new List<GameObject>();

        for (int i = 0; i < partsParent.childCount; i++)
        {
            if (Random.Range(0, 3) == 0)
                lasers.Add(partsParent.GetChild(i).gameObject);
        }

        if (lasers.Count == 0) lasers.Add(partsParent.GetChild(0).gameObject);

        while (timer < 4)
        {
            damTimer += Time.deltaTime;
            timer += Time.deltaTime;

            for (int i = 0; i < lasers.Count; i++)
            {
                Transform part = lasers[i].transform;
                if (part.gameObject.activeSelf)
                {
                    RaycastHit2D rh = Physics2D.Raycast(part.position + new Vector3(0, 1), -(transform.position - part.position).normalized, 20, lm);

                    float dist = rh ? rh.distance : 20;

                    if (rh && rh.transform.CompareTag("Player") && damTimer > .1f)
                    {
                        damTimer = 0;
                        _php.toDamage(4);
                    }

                    LineRenderer lr = part.GetComponent<LineRenderer>();
                    lr.SetPosition(0, part.position + new Vector3(0, 1));
                    lr.SetPosition(1, part.position + new Vector3(0, 1) - (transform.position - part.position).normalized * dist);
                }
            }

            yield return null;
        }

        for (int i = 0; i < partsParent.childCount; i++)
            if (partsParent.GetChild(i).gameObject.activeSelf)
            {
                LineRenderer lr = partsParent.GetChild(i).GetComponent<LineRenderer>();
                lr.SetPosition(0, Vector2.zero);
                lr.SetPosition(1, Vector2.zero);
            }
    }

    [SerializeField] GameObject object_0;
    IEnumerator action_3()
    {
        makingAction = true;

        float timer = 0, actionTimer = 0;
        while (actionTimer < 1)
        {
            timer += Time.deltaTime;
            actionTimer += Time.deltaTime;

            if (timer >= 0.05f)
            {
                GameObject _arrow = Instantiate(object_0);
                _arrow.transform.position = transform.position;
                Vector3 target = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                _arrow.GetComponent<EnemyArrowFly>().target = target.normalized;
                _arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((target).normalized.y,
                    (target).normalized.x) * 180 / Mathf.PI + 90);

                timer = 0;
            }

            yield return null;
        }

        makingAction = false;
    }

    [SerializeField] GameObject tiles;
    void action_4()
    {
        for (int i = 0; i < tiles.transform.childCount; i++)
        {
            tiles.transform.GetChild(i).GetComponent<Animator>().SetTrigger("Event");
        }
    }
}