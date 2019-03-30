using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_5_DeathCutscene : MonoBehaviour
{
    [SerializeField] GameObject[] toWhite;
    [SerializeField] GameObject[] toDestr;
    [SerializeField] Material white, black;
    [SerializeField] GameObject Boss;

    [SerializeField] GameObject particlePrefab, lastExpl, canvas;
    public IEnumerator finalCutscene()
    {
        //yield return new WaitForSeconds(2);

        Destroy(GetComponent<Boss_5>());
        FindObjectOfType<CamFollow>().stopCameraRotating();

        for (int i = 0; i < toWhite.Length; i++)
        {
            toWhite[i].GetComponent<SpriteRenderer>().material = white;
        }

        GameObject.Find("Player").transform.GetChild(0).GetComponent<SpriteRenderer>().material = white;
        GameObject.Find("Player").transform.GetChild(1).GetComponent<SpriteRenderer>().material = white;

        for (int i = 0; i < toDestr.Length; i++)
        {
            Destroy(toDestr[i]);
        }

        FindObjectOfType<UIManager>().setAllItems(false);

        GameObject camPoint = Instantiate(new GameObject());
        camPoint.transform.position = Boss.transform.position + new Vector3(0, 3);
        FindObjectOfType<CamFollow>().followTarget = camPoint;

        yield return new WaitForSeconds(1);

        Vector2 bossStartPos = Boss.transform.position;

        float timer = 2;
        while (timer > 0)
        {
            timer -= Time.deltaTime * 4;

            Boss.transform.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));

            yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));

            Boss.transform.position = bossStartPos;
            yield return null;

        }

        timer = 2;
        while (timer > 0)
        {
            timer -= Time.deltaTime * 4;

            Boss.transform.GetComponent<SpriteRenderer>().material = white;
            Boss.transform.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));

            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            Boss.transform.GetComponent<SpriteRenderer>().material = black;
            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));

            Boss.transform.position = bossStartPos;
            GameObject particle = Instantiate(particlePrefab, Boss.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(0, 6f)), Quaternion.identity);
            Destroy(particle, 0.31f);
        }

        GameObject LastExpl = Instantiate(lastExpl, Boss.transform.position + new Vector3(0, 4), Quaternion.identity);
        Destroy(LastExpl, 2);

        canvas.SetActive(true);

        string text = canvas.transform.GetChild(1).GetComponent<Text>().text;
        int curChar = 0;
        canvas.transform.GetChild(1).GetComponent<Text>().text = "";

        while (canvas.transform.GetChild(0).GetComponent<Image>().color.a <= 1)
        {
            canvas.transform.GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime / 2);

            yield return null;
        }

        yield return new WaitForSeconds(2);

        while (curChar < text.Length)
        {
            canvas.transform.GetChild(1).GetComponent<Text>().text += text[curChar];
            curChar++;

            yield return new WaitForSeconds(text[curChar - 1] != '.' ? 0.1f : 1);
        }

        yield return new WaitForSeconds(2);

        GameObject blackScreen = FindObjectOfType<UIManager>().blackScreen;

        blackScreen.SetActive(true);
        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        PlayerPrefs.SetInt("PlayerType", 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("City");
    }
}
