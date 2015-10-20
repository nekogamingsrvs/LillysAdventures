using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the main camera's moevement.
/// </summary>
public class CameraMovementManager : MonoBehaviour
{
	/// <summary>
	/// The LevelBounds for the map.
	/// </summary>
	private Rect LevelBounds;
	/// <summary>
	/// The left most bound.
	/// </summary>
	private float LeftBound;
	/// <summary>
	/// The right most bound.
	/// </summary>
	private float RightBound;
	/// <summary>
	/// The highest bound.
	/// </summary>
	private float TopBound;
	/// <summary>
	/// The lowest bound.
	/// </summary>
	private float BottomBound;
	
	// Use this for initialization
	void Start()
	{
		// Get the LevelBounds from game GameManager.
		LevelBounds = FindObjectOfType<GameManager>().levelBoundries;

		// Calculate the extents of the camera.
		float VertExtent = GetComponent<Camera>().orthographicSize;
		float HorzExtent = VertExtent * Screen.width / Screen.height;

		// Calculate the camera's LevelBounds to the map.
		LeftBound = (float)(HorzExtent - LevelBounds.width / 2.0f);
		RightBound = (float)(LevelBounds.width / 2.0f - HorzExtent);
		BottomBound = (float)(VertExtent - LevelBounds.height / 2.0f);
		TopBound = (float)(LevelBounds.height / 2.0f - VertExtent);
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
		Vector3 VectorClamp = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		// Clamps the camera.
		VectorClamp.x = Mathf.Clamp(vectorClamp.x, LeftBound, RightBound);
		VectorClamp.y = Mathf.Clamp(vectorClamp.y, BottomBound, TopBound);
		VectorClamp.z = -100;

		// Sets the clamp to the camera.
		transform.position = vectorClamp;
	}

	// Starts when the level was loaded.
	void OnLevelWasLoaded()
	{
		Start();
	}
}