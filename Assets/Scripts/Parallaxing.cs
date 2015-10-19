using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour
{
    //generally camera
    public Transform parent;

    //generally 0-1. <0 moves as foreground
    public float depth;

    //object with SpriteRenderer to be drawn, ensure this is larger than the camera viewport
    public GameObject layerSprite;

    //size of the layerSprite
    private Vector2 size;

    //position of parent, and the meeting of the 4 corners of our 4 layerSprites
    private Vector2 center;

    //our 4 layerSprites
    private GameObject obj1;
    private GameObject obj2;
    private GameObject obj3;
    private GameObject obj4;

    //our 4 layerSprite positions
    private Vector2 obj1p;
    private Vector2 obj2p;
    private Vector2 obj3p;
    private Vector2 obj4p;

    void Awake()
    {
        size = layerSprite.GetComponent<Renderer>().bounds.size;
        center = new Vector2(parent.position.x, parent.position.y);

        //instantiate all 4 objects
        obj1 = layerSprite;
        obj2 = Instantiate(layerSprite);
        obj3 = Instantiate(layerSprite);
        obj4 = Instantiate(layerSprite);

        obj1p = new Vector3();
        obj2p = new Vector3();
        obj3p = new Vector3();
        obj4p = new Vector3();
    }

    void Update()
    {
        //compute our new position
        center.x = f(parent.position.x, depth, size.x);
        center.y = f(parent.transform.position.y, depth, size.y);

        //update 4 object positions
        obj1p.x = center.x + size.x / 2;
        //obj1p.y = -2.88f;
        obj1p.y = parent.transform.position.y + 16;
        obj1.transform.position = obj1p;

        obj2p.x = center.x - size.x / 2;
        //obj2p.y = -2.88f;
        obj2p.y = parent.transform.position.y + 16;
        obj2.transform.position = obj2p;

        obj3p.x = center.x - size.x / 2;
        //obj3p.y = -2.88f;
        obj3p.y = parent.transform.position.y + 16;
        obj3.transform.position = obj3p;

        obj4p.x = center.x + size.x / 2;
        //obj4p.y = -2.88f;
        obj4p.y = parent.transform.position.y + 16;
        obj4.transform.position = obj4p;
    }

    //p = position, in this scenario, x or y
    //d = depth
    //w = width or height of object
    private float f(float p, float d, float w)
    {
        return d * p + Mathf.Round(p * (1 - d) / w) * w;
    }
}