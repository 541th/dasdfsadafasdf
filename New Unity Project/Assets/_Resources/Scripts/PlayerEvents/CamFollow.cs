using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class CamFollow : MonoBehaviour
{
    public GameObject followTarget;
    Transform shotDir;

    private Vector3 targetPos;
    public float moveSpeed;

    public static bool cameraExists;

    public void updateCamSize()
    { 
        if (_pm == null) _pm = FindObjectOfType<PlayerMovement>();
        GetComponent<Camera>().orthographicSize = 6 + (_pm.playerType == 2 ? InfoController.perks[6].value : 0);
    }

    public void setCamAsUsuall()
    {
        GetComponent<Camera>().orthographicSize = 6;
    }

    public void lightOnMap()
    {
        StartCoroutine(lightOnMapEvent());
    }

    public void startCameraRotating()
    {
        if (!started) StartCoroutine("CamRotating");
    }

    public void stopCameraRotating()
    {
        started = false;
        StopCoroutine("CamRotating");

        if (!returning) StartCoroutine(returnCamPos());
    }

    bool started;
    IEnumerator CamRotating()
    {
        started = true;
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;

            transform.eulerAngles += new Vector3(0, 0, Mathf.Sin(timer) * Time.deltaTime / 2);
            GetComponent<Camera>().orthographicSize += Mathf.Cos(timer + 40) * Time.deltaTime / 2;

            yield return null;
        }
    }

    bool returning;
    IEnumerator returnCamPos()
    {
        returning = true;
        if (_pm == null) _pm = FindObjectOfType<PlayerMovement>();
        float camSizeUsuall = 6 + (_pm.playerType == 2 ? InfoController.perks[6].value : 0);

        int sizeSign = GetComponent<Camera>().orthographicSize - camSizeUsuall > 0 ? 1 : -1;

        while ((GetComponent<Camera>().orthographicSize - camSizeUsuall) * sizeSign > 0)
        {
            GetComponent<Camera>().orthographicSize -= Time.deltaTime * sizeSign;
            yield return null;
        }

        transform.eulerAngles = Vector3.zero;
        returning = false;
    }

    IEnumerator lightOnMapEvent()
    {
        if (PlayerPrefs.GetInt("LevelType") == 1)
            GetComponent<Camera>().backgroundColor = new Color(.9f, .2f, 0);
        else if (PlayerPrefs.GetInt("LevelType") == 2)
            GetComponent<Camera>().backgroundColor = new Color(.3f, .1f, .6f);
        else if (PlayerPrefs.GetInt("LevelType") == 3)
            GetComponent<Camera>().backgroundColor = new Color(.3f, .1f, .6f);
        else if (PlayerPrefs.GetInt("LevelType") == 4)
            GetComponent<Camera>().backgroundColor = new Color(.3f, .6f, .1f);
        else if (PlayerPrefs.GetInt("LevelType") == 5)
            GetComponent<Camera>().backgroundColor = new Color(.8f, .1f, .6f);

        yield return new WaitForSeconds(0.1f);

        GetComponent<Camera>().backgroundColor = new Color(0, 0, 0);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        updateCamSize();

        if (followTarget == null) followTarget = GameObject.Find("Player");
        shotDir = followTarget.transform.GetChild(followTarget.transform.childCount - 1);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (PlayerPrefs.GetInt("CutScene") == 1)
        {
            StartCoroutine(cutscene());
            return;
        }

        loadPlayer();

        if (followTarget == null) followTarget = GameObject.Find("Player");
        shotDir = followTarget.transform.GetChild(followTarget.transform.childCount - 1);

        FindObjectOfType<PlayerHP>().setAllFull();
    }

    IEnumerator cutscene()
    {
        GameObject.Find("Player").transform.position = GameObject.Find("StartGame").transform.position;
        transform.position = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, transform.position.z);

        GameObject blackScreen = FindObjectOfType<UIManager>().blackScreen;
        UnityEngine.UI.Image blackScreenImage = blackScreen.GetComponent<UnityEngine.UI.Image>();
        blackScreen.SetActive(true);

        FindObjectOfType<UIManager>().setAllItems(false);

        blackScreenImage.color = new Color(0, 0, 0, 1);

        while (blackScreenImage.color.a >= 0)
        {
            blackScreenImage.color -= new Color(0, 0, 0, Time.deltaTime / 2);
            yield return null;
        }

        GameObject.Find("Player").transform.GetChild(0).GetComponent<Animator>().SetFloat("lmx", 1);
        GameObject.Find("Player").transform.GetChild(0).GetComponent<Animator>().SetFloat("lmy", 1);

        blackScreen.SetActive(false);

        dialogueBox.SetActive(true);

        yield return new WaitForSeconds(1);

        followTarget = GameObject.Find("Human (4)");

        string[] textNodes = new string[6];

        textNodes[0] = LanguageLines.getLine(14);
        textNodes[1] = LanguageLines.getLine(15);
        textNodes[2] = LanguageLines.getLine(16);
        textNodes[3] = LanguageLines.getLine(17);
        textNodes[4] = LanguageLines.getLine(18);
        textNodes[5] = LanguageLines.getLine(19);

        while (curNode < textNodes.Length)
        {
            yield return new WaitForSeconds(0.6f);

            dialogueBox.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "";

            while (dialogueBox.transform.localPosition.y < -130)
            {
                dialogueBox.transform.localPosition += new Vector3(0, Time.deltaTime * 600, 0);

                yield return null;
            }

            int counter = 0;

            while (counter < textNodes[curNode].Length)
            {
                dialogueBox.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text += textNodes[curNode][counter];
                counter++;

                yield return new WaitForSeconds(0.06f);
            }

            dialogueBox.transform.GetChild(1).gameObject.SetActive(true);

            while (!pressed)
            {
                yield return null;
            }

            pressed = false;

            while (dialogueBox.transform.localPosition.y > -400)
            {
                dialogueBox.transform.localPosition -= new Vector3(0, Time.deltaTime * 600, 0);

                yield return null;
            }

            yield return null;
        }

        followTarget = GameObject.Find("Player");
        FindObjectOfType<UIManager>().setAllItems(true);

        dialogueBox.SetActive(false);
    }

    bool pressed;
    int curNode = 0;

    public void nextNode()
    {
        curNode++;
        pressed = true;
        dialogueBox.transform.GetChild(1).gameObject.SetActive(false);
    }

    [SerializeField] GameObject dialogueBox;

    private void Awake()
    {
        if (!cameraExists)
        {
            cameraExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void loadPlayer()
    {
        FindObjectOfType<UIManager>().setAllItems(true);

        if (GameObject.Find("Player") == null)
        {
            GameObject player = Instantiate(Resources.Load("Prefabs/Player") as GameObject, GameObject.Find("StartPoint").transform.position, Quaternion.identity);
            player.name = "Player";
            player.GetComponent<PlayerMovement>().playerType = PlayerPrefs.GetInt("PlayerType");
            Invoke("setStartPlayerValues", .1f);
        }
        else
        {
            GameObject.Find("Player").transform.position = GameObject.Find("StartPoint").transform.position;
            GameObject.Find("Player").GetComponent<PlayerMovement>().playerType = PlayerPrefs.GetInt("PlayerType");
        }

        StartCoroutine(loadingEvent());
    }

    void setStartPlayerValues()
    {
        GameObject.Find("Player").GetComponent<PlayerMovement>().setStartValues();
    }

    IEnumerator loadingEvent()
    {
        GameObject blackScreen = FindObjectOfType<UIManager>().blackScreen;
        UnityEngine.UI.Image blackScreenImage = blackScreen.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Text blackScreenText = blackScreen.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        blackScreen.SetActive(true);

        blackScreenImage.color = new Color(0, 0, 0, 1);
        blackScreenText.color = new Color(1, 1, 1, 0);

        if (PlayerPrefs.GetInt("Continue") == 0)
            yield return new WaitForSeconds(0.6f);

        yield return new WaitForSeconds(0.1f);
        PlayerPrefs.SetInt("Continue", 0);

        if (_pm == null) _pm = FindObjectOfType<PlayerMovement>();
        GetComponent<Camera>().orthographicSize = 6 + (_pm.playerType == 2 ? InfoController.perks[6].value : 0);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "City" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Menu")
        {
            if (blackScreen == null)
            {
                blackScreen = FindObjectOfType<UIManager>().blackScreen;
                blackScreenImage = blackScreen.GetComponent<UnityEngine.UI.Image>();
                blackScreenText = blackScreen.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
            }

            blackScreenText.text = LanguageLines.getLine(33) + " " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            while (blackScreenText.color.a < 1)
            {
                blackScreenText.color += new Color(0, 0, 0, Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1);
        }

        blackScreen = FindObjectOfType<UIManager>().blackScreen;
        blackScreenImage = blackScreen.GetComponent<UnityEngine.UI.Image>();
        blackScreenText = blackScreen.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();

        while (blackScreenImage.color.a >= 0)
        {
            blackScreenText.color -= new Color(0, 0, 0, Time.deltaTime);

            blackScreenImage.color -= new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        blackScreen.SetActive(false);
    }

    PlayerMovement _pm;

    float getCamSizeUsuall()
    {
        if (_pm == null) _pm = FindObjectOfType<PlayerMovement>();

        return 6 + (_pm.playerType == 2 ? InfoController.perks[6].value : 0);
    }

    float clampedX, clampedY, a_h, a_v;
    Vector2 delta, swordShakeDir;
    bool arrowShotShaking;
    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
        {
            if (shotDir == null)
                shotDir = followTarget.transform.GetChild(followTarget.transform.childCount - 1);

            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, 
                getCamSizeUsuall() + new Vector2(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical")).magnitude / 2, moveSpeed * Time.deltaTime);

            a_h = CnInputManager.GetAxis("Attack_H");
            a_v = CnInputManager.GetAxis("Attack_V");

            delta = Vector2.zero;

            if (!arrowShotShaking)
                delta = new Vector2(a_h, a_v) * 4;

            if (swordShake)
            {
                delta += swordShakeDir;
            }

            if (a_h != 0 || a_v != 0)
            {
                shotDir.gameObject.SetActive(true);
                shotDir.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(a_v, a_h) * 180 / Mathf.PI - 90);
                shotDir.transform.localPosition = new Vector3(a_h, a_v, 1);
                shotDir.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Vector2.Distance(Vector2.zero, new Vector2(Mathf.Abs(a_h), Mathf.Abs(a_v))));
            }
            else
                shotDir.gameObject.SetActive(false);

            targetPos = new Vector3(followTarget.transform.position.x + delta.x, followTarget.transform.position.y + delta.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
        else
            followTarget = GameObject.Find("Player");
    }

    public void startShakeArrow()
    {
        StartCoroutine(arrowShotShake());
    }

    IEnumerator arrowShotShake()
    {
        arrowShotShaking = true;
        yield return new WaitForSeconds(0.02f);
        arrowShotShaking = false;
    }

    public void startSwordShake()
    {
        StartCoroutine(swordShakeEvent());
    }

    bool swordShake;
    IEnumerator swordShakeEvent()
    {
        swordShake = true;
        swordShakeDir = new Vector2(Random.Range(-4f, 4f), Random.Range(-4f, 4f));
        yield return new WaitForSeconds(0.03f);
        swordShake = false;
    }

    public void punchShake(float value)
    {
        transform.position += new Vector3(Random.Range(-value, value), Random.Range(-value, value));
    }
}