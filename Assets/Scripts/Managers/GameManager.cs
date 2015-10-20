using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	/// <summary>
	/// The level of the game.
	/// </summary>
	public GameObject Level;

	/// <summary>
	/// Gets or sets if the game is in debug mode.
	/// </summary>
	public bool isDebugActive;

	/// <summary>
	/// Gets or sets the score of the game.
	/// </summary>
	public int Score
	{
		get;
		set;
	}

	/// <summary>
	/// The boundaries of the map.
	/// </summary>
	[HideInInspector]
	public Rect LevelBoundries;

	// Use this for initialization
	void Start()
	{
		// Calculates the levels boundaries.
		levelBoundries = new Rect();
		levelBoundries.xMin = 0;
		levelBoundries.xMax = level.GetComponent<Tiled2Unity.TiledMap>().MapWidthInPixels * 2;
		levelBoundries.yMin = -level.GetComponent<Tiled2Unity.TiledMap>().MapHeightInPixels * 2;
		levelBoundries.yMax = 0;

		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
		{
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("MobileControls"))
            {
				go.SetActive(false);
			}
		}

		GameObject.FindObjectWithTag("GameController").GetComponent<GameManager>().AddToDatabase("Score", Score);
	}
}