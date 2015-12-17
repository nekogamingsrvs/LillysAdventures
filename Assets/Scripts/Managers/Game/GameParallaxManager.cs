using UnityEngine;

namespace VoidInc
{
	public class GameParallaxManager : MonoBehaviour
	{
		public enum ParallaxType
		{
			Normal,
			FixedY,
			Follow
		}

		/// <summary>
		/// The object to track. Generally a camera.
		/// </summary>
		public Transform parent;

		/// <summary>
		/// Generally 0-1. <0 moves as foreground.
		/// </summary>
		public float depth;

		public ParallaxType type;

		public float YOffset;

		public float PositionY;

		/// <summary>
		/// Object with SpriteRenderer to be drawn, ensure this is larger than the camera viewport.
		/// </summary>
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

			obj2.transform.SetParent(gameObject.transform);
			obj3.transform.SetParent(gameObject.transform);
			obj4.transform.SetParent(gameObject.transform);

			obj1p = new Vector3();
			obj2p = new Vector3();
			obj3p = new Vector3();
			obj4p = new Vector3();
		}

		void LateUpdate()
		{
			//compute our new position
			center.x = f(parent.position.x, depth, size.x);
			center.y = f(parent.transform.position.y, depth, size.y);

			if (type == ParallaxType.Normal)
			{
				//update 4 object positions
				obj1p.x = center.x + size.x / 2;
				obj1p.y = center.y + size.y / 2;
				obj1.transform.position = obj1p;

				obj2p.x = center.x - size.x / 2;
				obj2p.y = center.y - size.y / 2;
				obj2.transform.position = obj2p;

				obj3p.x = center.x - size.x / 2;
				obj3p.y = center.y + size.y / 2;
				obj3.transform.position = obj3p;

				obj4p.x = center.x + size.x / 2;
				obj4p.y = center.y - size.y / 2;
				obj4.transform.position = obj4p;
			}
			else if (type == ParallaxType.FixedY)
			{
				//update 4 object positions
				obj1p.x = center.x + size.x / 2;
				obj1p.y = PositionY;
				obj1.transform.position = obj1p;

				obj2p.x = center.x - size.x / 2;
				obj2p.y = PositionY;
				obj2.transform.position = obj2p;

				obj3p.x = (center.x - size.x / 2) - size.x;
				obj3p.y = PositionY;
				obj3.transform.position = obj3p;

				obj4p.x = (center.x + size.x / 2) + size.x;
				obj4p.y = PositionY;
				obj4.transform.position = obj4p;
			}
			else if (type == ParallaxType.Follow)
			{
				//update 4 object positions
				obj1p.x = center.x + size.x / 2;
				obj1p.y = parent.position.y + YOffset;
				obj1.transform.position = obj1p;

				obj2p.x = center.x - size.x / 2;
				obj2p.y = parent.position.y + YOffset;
				obj2.transform.position = obj2p;

				obj3p.x = (center.x - size.x / 2) - size.x;
				obj3p.y = parent.position.y + YOffset;
				obj3.transform.position = obj3p;

				obj4p.x = (center.x + size.x / 2) + size.x;
				obj4p.y = parent.position.y + YOffset;
				obj4.transform.position = obj4p;
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
}