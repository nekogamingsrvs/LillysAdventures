using UnityEngine;

namespace VoidInc
{
	/// <summary>
	/// Handles the main camera's movement.
	/// </summary>
	public class CameraManager : MonoBehaviour
	{
		/// <summary>
		/// The LevelBounds for the map.
		/// </summary>
		private Rect _LevelBounds;

		/// <summary>
		/// The left most bound.
		/// </summary>
		private float _LeftBound;

		/// <summary>
		/// The right most bound.
		/// </summary>
		private float _RightBound;

		/// <summary>
		/// The highest bound.
		/// </summary>
		private float _TopBound;

		/// <summary>
		/// The lowest bound.
		/// </summary>
		private float _BottomBound;

		// Use this for initialization
		void Start()
		{
			// Get the LevelBounds from game GameManager.
			_LevelBounds = FindObjectOfType<GameManager>().LevelBoundries;

			// Calculate the extents of the camera.
			float VertExtent = GetComponent<Camera>().orthographicSize;
			float HorzExtent = VertExtent * Screen.width / Screen.height;

			// Calculate the camera's LevelBounds to the map.
			_LeftBound = (float)(HorzExtent);
			_RightBound = (float)(_LevelBounds.width / 2.0f - HorzExtent);
			_BottomBound = (float)(VertExtent - _LevelBounds.height / 2.0f);
			_TopBound = (float)(-VertExtent);
		}

		// Update is called once per frame
		void Update()
		{
			// Resets the camera to the player's local position.
			transform.localPosition = new Vector3(0, 0, -100);
		}

		// Update is called once per frame later than Update
		void LateUpdate()
		{
			// Classmates the clamp for the camera's position.
			Vector3 vectorClamp = new Vector3(transform.position.x, transform.position.y, transform.position.z);

			// Clamps the camera.
			vectorClamp.x = Mathf.Clamp(vectorClamp.x, _LeftBound, _RightBound);
			vectorClamp.y = Mathf.Clamp(vectorClamp.y, _BottomBound, _TopBound);
			vectorClamp.z = -100;

			// Sets the clamp to the camera.
			transform.position = vectorClamp;
		}

		// Starts when the level was loaded.
		void OnLevelWasLoaded()
		{
			Start();
		}
	}
}