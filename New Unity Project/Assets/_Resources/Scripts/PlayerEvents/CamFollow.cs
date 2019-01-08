using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class CamFollow : MonoBehaviour
{
    public GameObject followTarget;

    private Vector3 targetPos;
    public float moveSpeed;

    public static bool cameraExists;

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
    }
    
    float clampedX, clampedY;
    Vector2 delta;
    bool arrowShotShaking;
    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
        {
            if (!arrowShotShaking)
                delta = new Vector2(CnInputManager.GetAxis("Attack_H") * 2, CnInputManager.GetAxis("Attack_V") * 2);
            else
                delta = Vector2.zero;
            
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