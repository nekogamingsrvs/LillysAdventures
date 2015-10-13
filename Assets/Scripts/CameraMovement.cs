using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public Transform player;

    private Rect bounds;
    private float leftBound;
    private float rightBound;
    private float topBound;
    private float bottomBound;

    void Start()
    {
        bounds = FindObjectOfType<GameManager>().levelBoundries;

        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        leftBound = horzExtent;
        rightBound = (float)(bounds.width / 2.0f - horzExtent);
        bottomBound = (float)(vertExtent - bounds.height / 2.0f);
        topBound = -(float)(bounds.height / 2.0f - vertExtent - 0.5f);
    }

    void Update()
    {
        transform.localPosition = new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 vectorClamp = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        vectorClamp.x = Mathf.Clamp(vectorClamp.x, leftBound, rightBound);
        vectorClamp.y = Mathf.Clamp(vectorClamp.y, bottomBound, topBound);
        vectorClamp.z = -1.0f;


        transform.position = vectorClamp;
    }

    void OnLevelWasLoaded()
    {
        Start();
    }
}
