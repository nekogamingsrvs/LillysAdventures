using UnityEngine;

namespace VoidInc
{
	public class GameParallaxManager : MonoBehaviour
	{
		/// <summary>
		/// The type of parallax the parallax manager is using.
		/// </summary>
		public enum ParallaxType
		{
			Normal,
			FixedY,
			Follow
		}
		/// <summary>
		/// The object to track. Generally a camera.
		/// </summary>
		public Transform Parent;
		/// <summary>
		/// Generally 0 - 1. less than 0 moves as foreground.
		/// </summary>
		public float Depth;
		/// <summary>
		/// The parallax type the parallax manager is using.
		/// </summary>
		public ParallaxType Type;
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
		private GameObject _Object1, _Object2, _Object3, _Object4;
		/// <summary>
		/// Our four layer sprite positions.
		/// </summary>
		private Vector2 _Object1Pos, _Object2Pos, _Object3Pos, _Object4Pos;

		void Awake()
		{
			_Size = LayerSprite.GetComponent<Renderer>().bounds.size;
			_Center = new Vector2(Parent.position.x, Parent.position.y);

			//instantiate all 4 objects
			_Object1 = LayerSprite;
			_Object2 = Instantiate(LayerSprite);
			_Object3 = Instantiate(LayerSprite);
			_Object4 = Instantiate(LayerSprite);

			_Object2.transform.SetParent(gameObject.transform);
			_Object3.transform.SetParent(gameObject.transform);
			_Object4.transform.SetParent(gameObject.transform);

			_Object1Pos = new Vector3();
			_Object2Pos = new Vector3();
			_Object3Pos = new Vector3();
			_Object4Pos = new Vector3();
		}

		void LateUpdate()
		{
			//compute our new position
			_Center.x = f(Parent.position.x, Depth, _Size.x);
			_Center.y = f(Parent.transform.position.y, Depth, _Size.y);

			if (Type == ParallaxType.Normal)
			{
				//update 4 object positions
				_Object1Pos.x = _Center.x + _Size.x / 2;
				_Object1Pos.y = _Center.y + _Size.y / 2;
				_Object1.transform.position = _Object1Pos;

				_Object2Pos.x = _Center.x - _Size.x / 2;
				_Object2Pos.y = _Center.y - _Size.y / 2;
				_Object2.transform.position = _Object2Pos;

				_Object3Pos.x = _Center.x - _Size.x / 2;
				_Object3Pos.y = _Center.y + _Size.y / 2;
				_Object3.transform.position = _Object3Pos;

				_Object4Pos.x = _Center.x + _Size.x / 2;
				_Object4Pos.y = _Center.y - _Size.y / 2;
				_Object4.transform.position = _Object4Pos;
			}
			else if (Type == ParallaxType.FixedY)
			{
				//update 4 object positions
				_Object1Pos.x = _Center.x + _Size.x / 2;
				_Object1Pos.y = PositionY;
				_Object1.transform.position = _Object1Pos;

				_Object2Pos.x = _Center.x - _Size.x / 2;
				_Object2Pos.y = PositionY;
				_Object2.transform.position = _Object2Pos;

				_Object3Pos.x = (_Center.x - _Size.x / 2) - _Size.x;
				_Object3Pos.y = PositionY;
				_Object3.transform.position = _Object3Pos;

				_Object4Pos.x = (_Center.x + _Size.x / 2) + _Size.x;
				_Object4Pos.y = PositionY;
				_Object4.transform.position = _Object4Pos;
			}
			else if (Type == ParallaxType.Follow)
			{
				//update 4 object positions
				_Object1Pos.x = _Center.x + _Size.x / 2;
				_Object1Pos.y = Parent.position.y + YOffset;
				_Object1.transform.position = _Object1Pos;

				_Object2Pos.x = _Center.x - _Size.x / 2;
				_Object2Pos.y = Parent.position.y + YOffset;
				_Object2.transform.position = _Object2Pos;

				_Object3Pos.x = (_Center.x - _Size.x / 2) - _Size.x;
				_Object3Pos.y = Parent.position.y + YOffset;
				_Object3.transform.position = _Object3Pos;

				_Object4Pos.x = (_Center.x + _Size.x / 2) + _Size.x;
				_Object4Pos.y = Parent.position.y + YOffset;
				_Object4.transform.position = _Object4Pos;
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