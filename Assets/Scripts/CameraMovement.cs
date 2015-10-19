using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the main camera's moevement.
/// </summary>
public class CameraMovement : MonoBehaviour
{
	/// <summary>
	/// The bounds for the map.
	/// </summary>
	private Rect bounds;
	/// <summary>
	/// The left most bound.
	/// </summary>
	private float leftBound;
	/// <summary>
	/// The right most bound.
	/// </summary>
	private float rightBound;
	/// <summary>
	/// The highest bound.
	/// </summary>
	private float topBound;
	/// <summary>
	/// The lowest bound.
	/// </summary>
	private float bottomBound;
	
	// Use this for initialization
	void Start()
	{
		// Get the bounds from game GameManager.
		bounds = FindObjectOfType<GameManager>().levelBoundries;

		// Calculate the extents of the camera.
		float vertExtent = GetComponent<Camera>().orthographicSize;
		float horzExtent = vertExtent * Screen.width / Screen.height;

		// Calculate the camera's bounds to the map.
		leftBound = (float)(horzExtent - bounds.width / 2.0f);
		rightBound = (float)(bounds.width / 2.0f - horzExtent);
		bottomBound = (float)(vertExtent - bounds.height / 2.0f);
		topBound = (float)(bounds.height / 2.0f - vertExtent);
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
		// Clauclates the clamp for the camera's position.
		Vector3 vectorClamp = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		// Clamps the camera.
		vectorClamp.x = Mathf.Clamp(vectorClamp.x, leftBound, rightBound);
		vectorClamp.y = Mathf.Clamp(vectorClamp.y, bottomBound, topBound);
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