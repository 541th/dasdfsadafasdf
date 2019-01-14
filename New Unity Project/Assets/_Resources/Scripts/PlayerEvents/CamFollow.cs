﻿using System.Collections;
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
        GetComponent<Camera>().orthographicSize = 6 + InfoController.perks[6].value;
    }

    public void setCamAsUsuall()
    {
        GetComponent<Camera>().orthographicSize = 6;
    }

    private void Start()
    {
        updateCamSize();
    }

    // Use this for initialization
    void Awake()
    {
        if (!cameraExists)
        {
            cameraExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        if (followTarget == null) followTarget = GameObject.Find("Player");
        shotDir = followTarget.transform.GetChild(followTarget.transform.childCount - 1);
    }
    
    float clampedX, clampedY, a_h, a_v;
    Vector2 delta;
    bool arrowShotShaking;
    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
        {
            a_h = CnInputManager.GetAxis("Attack_H") * 2;
            a_v = CnInputManager.GetAxis("Attack_V") * 2;
            if (!arrowShotShaking)
                delta = new Vector2(a_h, a_v);
            else
                delta = Vector2.zero;
            
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
    }

    public void startShakeArrow()
    {
        StartCoroutine(arrowShotShake());
    }

    IEnumerator arrowShotShake()
    {
        arrowShotShaking = true;
        yield return new WaitForSeconds(0.01f);
        arrowShotShaking = false;
    }
}