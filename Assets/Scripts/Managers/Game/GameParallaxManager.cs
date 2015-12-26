using UnityEngine;

namespace VoidInc
{
	public class GameParallaxManager : MonoBehaviour
	{
		/// <summary>
		/// The type of parallax the parallaxmanager is using.
		/// </summary>
		public enum _ParallaxType
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

		/// <summary>
		/// The parallax type the parallax manager is using.
		/// </summary>
		public _ParallaxType ParallaxType;

		/// <summary>
		/// The Y axis offset for Follow parallax type.
		/// </summary>
		public float YOffset;

		/// <summary>
		/// The Y axis position for FixedY parallax type.
		/// </summary>
		public float PositionY;

		/// <summary>
		/// Object with SpriteRenderer to be drawn, ensure this is larger than the camera viewport.
		/// </summary>
		public GameObject LayerSprite;

		/// <summary>
		/// The size of the layer sprite.
		/// </summary>
		private Vector2 _Size;

		/// <summary>
		/// position of parent, and the meeting of the 4 corners of our 4 layerSprites
		/// </summary>
		private Vector2 _Center;

		/// <summary>
		/// Our four layer sprites.
		/// </summary>
		private GameObject obj1, obj2, obj3, obj4;

		/// <summary>
		/// Our four layer sprite positions.
		/// </summary>
		private Vector2 obj1p, obj2p, obj3p, obj4p;

		void Awake()
		{
			_Size = LayerSprite.GetComponent<Renderer>().bounds.size;
			_Center = new Vector2(parent.position.x, parent.position.y);

			//instantiate all 4 objects
			obj1 = LayerSprite;
			obj2 = Instantiate(LayerSprite);
			obj3 = Instantiate(LayerSprite);
			obj4 = Instantiate(LayerSprite);

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
			_Center.x = f(parent.position.x, depth, _Size.x);
			_Center.y = f(parent.transform.position.y, depth, _Size.y);

			if (ParallaxType == _ParallaxType.Normal)
			{
				//update 4 object positions
				obj1p.x = _Center.x + _Size.x / 2;
				obj1p.y = _Center.y + _Size.y / 2;
				obj1.transform.position = obj1p;

				obj2p.x = _Center.x - _Size.x / 2;
				obj2p.y = _Center.y - _Size.y / 2;
				obj2.transform.position = obj2p;

				obj3p.x = _Center.x - _Size.x / 2;
				obj3p.y = _Center.y + _Size.y / 2;
				obj3.transform.position = obj3p;

				obj4p.x = _Center.x + _Size.x / 2;
				obj4p.y = _Center.y - _Size.y / 2;
				obj4.transform.position = obj4p;
			}
			else if (ParallaxType == _ParallaxType.FixedY)
			{
				//update 4 object positions
				obj1p.x = _Center.x + _Size.x / 2;
				obj1p.y = PositionY;
				obj1.transform.position = obj1p;

				obj2p.x = _Center.x - _Size.x / 2;
				obj2p.y = PositionY;
				obj2.transform.position = obj2p;

				obj3p.x = (_Center.x - _Size.x / 2) - _Size.x;
				obj3p.y = PositionY;
				obj3.transform.position = obj3p;

				obj4p.x = (_Center.x + _Size.x / 2) + _Size.x;
				obj4p.y = PositionY;
				obj4.transform.position = obj4p;
			}
			else if (ParallaxType == _ParallaxType.Follow)
			{
				//update 4 object positions
				obj1p.x = _Center.x + _Size.x / 2;
				obj1p.y = parent.position.y + YOffset;
				obj1.transform.position = obj1p;

				obj2p.x = _Center.x - _Size.x / 2;
				obj2p.y = parent.position.y + YOffset;
				obj2.transform.position = obj2p;

				obj3p.x = (_Center.x - _Size.x / 2) - _Size.x;
				obj3p.y = parent.position.y + YOffset;
				obj3.transform.position = obj3p;

				obj4p.x = (_Center.x + _Size.x / 2) + _Size.x;
				obj4p.y = parent.position.y + YOffset;
				obj4.transform.position = obj4p;
			}
		}

		/// <summary>
		/// Computes parallax position.
		/// </summary>
		/// <param name="p">position, in this scenario, x or y</param>
		/// <param name="d">depth</param>
		/// <param name="w">width or height of object</param>
		/// <returns>Parallax Position (float)</returns>
		private float f(float p, float d, float w)
		{
			return d * p + Mathf.Round(p * (1 - d) / w) * w;
		}
	}
}