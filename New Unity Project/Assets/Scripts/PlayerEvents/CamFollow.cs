using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject followTarget;

    private Vector3 targetPos;
    public float moveSpeed;

    public static bool cameraExists;

    public BoxCollider2D boundBox;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private Camera theCamera;
    float halfHeight;
    float halfWidth;

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

        updateWH();
        setBounds(boundBox);

        if (followTarget == null) followTarget = GameObject.Find("Player");
    }

    public void updateWH()
    {
        theCamera = GetComponent<Camera>();
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
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

        clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void setBounds(BoxCollider2D newBounds)
    {
        boundBox = newBounds;

        minBounds = boundBox.bounds.min;
        maxBounds = boundBox.bounds.max;
    }
}
