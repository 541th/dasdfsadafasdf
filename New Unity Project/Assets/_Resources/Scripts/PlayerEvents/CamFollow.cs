using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
        {
            targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }
}
