using UnityEngine;

namespace VoidInc
{
	public class MenuParallaxManager : MonoBehaviour
	{
		/// <summary>
		/// The speed of the background.
		/// </summary>
		public Vector2 UvAnimationSpeed;

		/// <summary>
		/// Object with SpriteRenderer to be drawn, ensure this is larger than the camera viewport.
		/// </summary>
		public GameObject LayerSprite;

		/// <summary>
		/// Size of the layerSprite
		/// </summary>
		private Vector2 _Size;

		/// <summary>
		/// Position of parent, and the meeting of the 4 corners of our 4 layerSprites.
		/// </summary>
		private Vector2 _Center;

		/// <summary>
		/// Our four layer sprites.
		/// </summary>
		private GameObject obj1, obj2, obj3;

		/// <summary>
		/// Our four layer spite positions.
		/// </summary>
		private Vector2 obj1p, obj2p, obj3p;

		void Awake()
		{
			_Size = LayerSprite.GetComponent<Renderer>().bounds.size;
			_Center = new Vector2(240, -256);

			//instantiate all 4 objects
			obj1 = LayerSprite;
			obj2 = Instantiate(LayerSprite);
			obj3 = Instantiate(LayerSprite);

			obj1p = new Vector3();
			obj2p = new Vector3();
			obj3p = new Vector3();
		}

		void Update()
		{
			_Center += (UvAnimationSpeed * Time.deltaTime);

			//compute our new position
			//center.x = f(parent.position.x, -uvAnimationSpeed.x / 6, size.x);

			//update 4 object positions
			obj1p.x = _Center.x + _Size.x / 2;
			//obj1p.y = -2.88f;
			obj1p.y = -256;
			obj1.transform.position = obj1p;

			obj2p.x = _Center.x - _Size.x / 2;
			//obj2p.y = -2.88f;
			obj2p.y = -256;
			obj2.transform.position = obj2p;

			obj3p.x = (_Center.x - _Size.x / 2) + _Size.x * 2;
			//obj2p.y = -2.88f;
			obj3p.y = -256;
			obj3.transform.position = obj3p;

			if (_Center.x < -_Size.x / 2)
			{
				_Center = new Vector2(240, -256);
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