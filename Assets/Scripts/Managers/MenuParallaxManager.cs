using UnityEngine;
using System.Collections;

public class MenuParallaxManager : MonoBehaviour
{
	/// <summary>
	/// The speed of the background.
	/// </summary>
	public Vector2 uvAnimationSpeed;
	
	/// <summary>
	/// Object with SpriteRenderer to be drawn, ensure this is larger than the camera viewport.
	/// </summary>
	public GameObject layerSprite;

	/// <summary>
	/// Size of the layerSprite
	/// </summary>
	private Vector2 size;

	/// <summary>
	/// Position of parent, and the meeting of the 4 corners of our 4 layerSprites.
	/// </summary>
	private Vector2 center;
	
	//our 4 layerSprites
	private GameObject obj1;
	private GameObject obj2;
	private GameObject obj3;

	//our 4 layerSprite positions
	private Vector2 obj1p;
	private Vector2 obj2p;
	private Vector2 obj3p;

	void Awake()
	{
		size = layerSprite.GetComponent<Renderer>().bounds.size;
		center = new Vector2(160, -170);

		//instantiate all 4 objects
		obj1 = layerSprite;
		obj2 = Instantiate(layerSprite);
		obj3 = Instantiate(layerSprite);

		obj1p = new Vector3();
		obj2p = new Vector3();
		obj3p = new Vector3();
	}

	void Update()
	{
		center += (uvAnimationSpeed * Time.deltaTime);

		//compute our new position
		//center.x = f(parent.position.x, -uvAnimationSpeed.x / 6, size.x);

		//update 4 object positions
		obj1p.x = center.x + size.x / 2;
		//obj1p.y = -2.88f;
		obj1p.y = -170;
		obj1.transform.position = obj1p;

		obj2p.x = center.x - size.x / 2;
		//obj2p.y = -2.88f;
		obj2p.y = -170;
		obj2.transform.position = obj2p;
		
		obj3p.x = (center.x - size.x / 2) + size.x * 2;
		//obj2p.y = -2.88f;
		obj3p.y = -170;
		obj3.transform.position = obj3p;

		if (center.x < -size.x / 2)
		{
			center = new Vector2(160, -170);
		}
	}

	//p = position, in this scenario, x or y
	//d = depth
	//w = width or height of object
	private float f(float p, float d, float w)
	{
		return d * p + Mathf.Round(p * (1 - d) / w) * w;
	}
}
