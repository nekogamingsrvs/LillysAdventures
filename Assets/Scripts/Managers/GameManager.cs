using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
	/// <summary>
	/// The level of the game.
	/// </summary>
	[HideInInspector]
	public GameObject Level;

	/// <summary>
	/// Gets or sets if the game is in debug mode.
	/// </summary>
	public bool isDebugActive;

	/// <summary>
	/// Gets or sets the score of the game.
	/// </summary>
	public int Score;

	/// <summary>
	/// Gets or sets the amount of keys the player has.
	/// </summary>
	public int Keys;

	/// <summary>
	/// The boundaries of the map.
	/// </summary>
	[HideInInspector]
	public Rect LevelBoundries;

	/// <summary>
	/// The DebugLabelManager for debuging the game.
	/// </summary>
	[HideInInspector]
	public DebugLabelManager DebugLabelController;

	/// <summary>
	/// The text box for keeping score.
	/// </summary>
	public Text ScoreTextBox;

	/// <summary>
	/// Gets or sets the current level.
	/// </summary>
	public int CurrentLevel = 1;

	// Use this for initialization
	void Start()
	{
		Level = GameObject.Find("level" + CurrentLevel);
		DebugLabelController = GameObject.Find("DebugPanel").GetComponent<DebugLabelManager>();

		// Calculates the levels boundaries.
		LevelBoundries = new Rect();
		LevelBoundries.xMin = 0;
		LevelBoundries.xMax = Level.GetComponent<Tiled2Unity.TiledMap>().MapWidthInPixels * 2;
		LevelBoundries.yMin = -Level.GetComponent<Tiled2Unity.TiledMap>().MapHeightInPixels * 2;
		LevelBoundries.yMax = 0;
		
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
		{
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("MobileControls"))
            {
				go.SetActive(false);
			}
		}

		if (isDebugActive)
			DebugLabelController.AddToDatabase("Score", Score);
	}

	void Update()
	{
		ScoreTextBox.text = "Score : " + Score;

		if (isDebugActive)
			DebugLabelController.UpdateToDatabase("Score", Score);
    }
}