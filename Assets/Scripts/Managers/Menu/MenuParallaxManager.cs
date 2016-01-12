using UnityEngine;

namespace VoidInc.LWA
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
		private GameObject _Object1, _Object2, _Object3;
		/// <summary>
		/// Our four layer spite positions.
		/// </summary>
		private Vector2 _Object1Pos, _Object2Pos, _Object3Pos;

		void Awake()
		{
			_Size = LayerSprite.GetComponent<Renderer>().bounds.size;
			_Center = new Vector2(240, -256);

			//instantiate all 4 objects
			_Object1 = LayerSprite;
			_Object2 = Instantiate(LayerSprite);
			_Object3 = Instantiate(LayerSprite);

			_Object1Pos = new Vector3();
			_Object2Pos = new Vector3();
			_Object3Pos = new Vector3();
		}

		void Update()
		{
			_Center += (UvAnimationSpeed * Time.deltaTime);

			//compute our new position
			//center.x = f(parent.position.x, -uvAnimationSpeed.x / 6, size.x);

			//update 4 object positions
			_Object1Pos.x = _Center.x + _Size.x / 2;
			//obj1p.y = -2.88f;
			_Object1Pos.y = -256;
			_Object1.transform.position = _Object1Pos;

			_Object2Pos.x = _Center.x - _Size.x / 2;
			//obj2p.y = -2.88f;
			_Object2Pos.y = -256;
			_Object2.transform.position = _Object2Pos;

			_Object3Pos.x = (_Center.x - _Size.x / 2) + _Size.x * 2;
			//obj2p.y = -2.88f;
			_Object3Pos.y = -256;
			_Object3.transform.position = _Object3Pos;

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