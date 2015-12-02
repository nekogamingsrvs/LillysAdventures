using System.Collections.Generic;
using Tiled2Unity;
using UnityEngine;
using UnityEngine.UI;

namespace VoidInc
{
	public class GameManager : MonoBehaviour
	{
		/// <summary>
		/// Gets or sets if the game is in debug mode.
		/// </summary>
		public bool isDebugActive;

		/// <summary>
		/// Gets or sets the current level.
		/// </summary>
		public int CurrentLevel;

		/// <summary>
		/// Gets or sets the max amount of gems in the level.
		/// </summary>
		public int MaxGems;

		/// <summary>
		/// Gets or sets the total number of gems from each level.
		/// </summary>
		public int TotalGems;

		[HideInInspector]
		public bool IsLoaded;

		/// <summary>
		/// Gets or sets the score of the game.
		/// </summary>
		[HideInInspector]
		public int Score;

		/// <summary>
		/// Gets or sets the amount of keys the player has.
		/// </summary>
		[HideInInspector]
		public int Keys;

		/// <summary>
		/// Gets or sets the amount of gems the player has.
		/// </summary>
		[HideInInspector]
		public int Gems;

		/// <summary>
		/// The current level of the game.
		/// </summary>
		[HideInInspector]
		public TiledMap Level;

		/// <summary>
		/// The text box for keeping score.
		/// </summary>
		[HideInInspector]
		public Text ScorePanelText;

		/// <summary>
		/// The boundaries of the map.
		/// </summary>
		[HideInInspector]
		public Rect LevelBoundries;

		/// <summary>
		/// Gets or sets the list of key id's.
		/// </summary>
		[HideInInspector]
		public List<string> KeyIdentifiers = new List<string>();

		/// <summary>
		/// The list of destroyed game objects.
		/// </summary>
		[HideInInspector]
		public List<string> DestroyedGameObjects = new List<string>();

		[HideInInspector]
		public ConfigFileManager ConfigFileManager = new ConfigFileManager();

		[HideInInspector]
		public int DialogNum;

		[HideInInspector]
		public Vector3 PlayersPosition;

		// Use this for initialization
		void Awake()
		{
			// Get the current level of the game.
			Level = GameObject.Find("level" + CurrentLevel).GetComponent<TiledMap>();
			// Set the ScorePanel's Textbox for the GameManafer.
			ScorePanelText = GameObject.FindGameObjectWithTag("ScorePanel").GetComponent<Text>();

			// If this is not a mobile game then remove the mobile controls.
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			{
				foreach (GameObject go in GameObject.FindGameObjectsWithTag("MobileControls"))
				{
					go.SetActive(false);
				}
			}

			// Sets the level boundaries of the map for the camera.
			LevelBoundries = new Rect();
			LevelBoundries.xMin = 0;
			LevelBoundries.xMax = Level.MapWidthInPixels * 2;
			LevelBoundries.yMin = -Level.MapHeightInPixels * 2;
			LevelBoundries.yMax = 0;

			// Sets the score to be able to be seen from the debug panel.
			if (isDebugActive)
			{
				//DebugWindowManager.AddToDatabase("Score", Score);
			}

			if (IsLoaded)
			{
				GameObject.FindGameObjectWithTag("Player").transform.position = ConfigFileManager.SaveFile.PlayerData.Position;
				Score = ConfigFileManager.SaveFile.PlayerData.Score;
				TotalGems = ConfigFileManager.SaveFile.PlayerData.TotalGems;
				Gems = ConfigFileManager.SaveFile.PlayerData.Gems;
				Keys = ConfigFileManager.SaveFile.PlayerData.Keys;
				KeyIdentifiers = ConfigFileManager.SaveFile.PlayerData.KeyIdentifiers;
				DialogNum = ConfigFileManager.SaveFile.DialogNumber;
				DestroyedGameObjects = ConfigFileManager.SaveFile.DestroyedGameObjects;

				foreach (string gameObj in DestroyedGameObjects)
				{
					if (GameObject.Find(gameObj).GetComponent<ItemIdentifier>().Destroyed == true)
					{
						Destroy(GameObject.Find(gameObj));
					}
				}

				IsLoaded = false;
			}
		}

		void Update()
		{
			// Updates the score box text.
			ScorePanelText.text = "Score:" + Score;
			// Set the player's position.
			PlayersPosition = GameObject.Find("Player").transform.position;

			if (Gems == MaxGems)
			{
				foreach (CLAEoS claeos in GameObject.FindGameObjectWithTag("LevelChangerManager").GetComponentsInChildren<CLAEoS>())
				{
					claeos.CanTransition = true;
				}

				Gems = 0;
			}

			ConfigFileManager.SaveFile.PlayerData.Score = Score;
			ConfigFileManager.SaveFile.PlayerData.TotalGems = TotalGems;
			ConfigFileManager.SaveFile.PlayerData.Gems = Gems;
			ConfigFileManager.SaveFile.PlayerData.Keys = Keys;
			ConfigFileManager.SaveFile.PlayerData.Level = CurrentLevel;
			ConfigFileManager.SaveFile.PlayerData.Position = PlayersPosition;
			ConfigFileManager.SaveFile.PlayerData.KeyIdentifiers = KeyIdentifiers;
			ConfigFileManager.SaveFile.DialogNumber = DialogNum;
		}

		void LateUpdate()
		{
			ConfigFileManager.SaveGame();
		}
	}
}