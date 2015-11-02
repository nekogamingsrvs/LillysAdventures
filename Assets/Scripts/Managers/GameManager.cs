using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using Tiled2Unity;

namespace VoidInc
{
	public class GameManager : MonoBehaviour
	{
		/// <summary>
		/// The current level of the game.
		/// </summary>
		public TiledMap Level;

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
		/// Gets or sets the amount of gems the player has.
		/// </summary>
		public int Gems;

		/// <summary>
		/// The boundaries of the map.
		/// </summary>
		[HideInInspector]
		public Rect LevelBoundries;

		/// <summary>
		/// The DebugLabelManager for debuging the game.
		/// </summary>
		public DebugLabelManager DebugLabelController;

		/// <summary>
		/// The text box for keeping score.
		/// </summary>
		public Text ScoreTextBox;

		/// <summary>
		/// Gets or sets the current level.
		/// </summary>
		[HideInInspector]
		public string CurrentLevel = "0";

		/// <summary>
		/// Gets or sets the last level that was used.
		/// </summary>
		private string LastLevel;

		/// <summary>
		/// Gets or sets the list of key id's.
		/// </summary>
		[HideInInspector]
		public List<int> KeyIds;

		// Use this for initialization
		void Start()
		{
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			{
				foreach (GameObject go in GameObject.FindGameObjectsWithTag("MobileControls"))
				{
					go.SetActive(false);
				}
			}

			LevelBoundries = new Rect();
			LevelBoundries.xMin = 0;
			LevelBoundries.xMax = Level.MapWidthInPixels * 2;
			LevelBoundries.yMin = -Level.MapHeightInPixels * 2;
			LevelBoundries.yMax = 0;

			if (isDebugActive)
				DebugLabelController.AddToDatabase("Score", Score);
		}

		void Update()
		{
			ScoreTextBox.text = "Score : " + Score;

			if (isDebugActive)
				DebugLabelController.UpdateToDatabase("Score", Score);
		}

		void LateUpdate()
		{
			LastLevel = CurrentLevel;
		}

		public void ChangeLevel()
		{
			if (CurrentLevel != LastLevel)
			{
				Application.LoadLevel("level" + CurrentLevel);
			}
		}
	}
}