using System.Collections.Generic;
using Tiled2Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace VoidInc
{
	public class GameManager : MonoBehaviour
	{
		#region Public Variables
		///////////////////////////////////////
		// Setting up the map.
		///////////////////////////////////////
		/// <summary>
		/// Gets or sets if the game is in debug mode.
		/// </summary>
		public bool IsDebugActive;
		/// <summary>
		/// Gets or sets the current level.
		/// </summary>
		public int CurrentLevel;
		/// <summary>
		/// If the game got loaded.
		/// </summary>
		public bool IsLoaded = false;
		/// <summary>
		/// The boundaries of the map.
		/// </summary>
		public Rect LevelBoundries;

		///////////////////////////////////////
		//  The variables for the map.
		///////////////////////////////////////
		/// <summary>
		/// Gets or sets the max amount of gems in the level.
		/// </summary>
		public int MaxGems;
		/// <summary>
		/// Gets or sets the amount of gems the player has.
		/// </summary>
		public int Gems;

		///////////////////////////////////////
		//  GameObjects on the scene.
		///////////////////////////////////////
		/// <summary>
		/// Gets or sets the GameDataManager, which is used to transfer data between levels.
		/// </summary>
		[HideInInspector]
		public GameDataManager GameDataManager;
		/// <summary>
		/// The current level of the game.
		/// </summary>
		public TiledMap Level;
		/// <summary>
		/// The text box for keeping score.
		/// </summary>
		public Text ScorePanelText;
		/// <summary>
		/// The tag to look for when disabling mobile controls.
		/// </summary>
		public string MobileControlsTag;

		///////////////////////////////////////
		// Misc variables.
		///////////////////////////////////////
		/// <summary>
		/// The player's position compute.
		/// </summary>
		public Vector3 PlayersPosition;
		/// <summary>
		/// The current time of the game.
		/// </summary>
		[HideInInspector]
		public float CurrentTime = 0;
		/// <summary>
		/// The current time to calculate when to save.
		/// </summary>
		[HideInInspector]
		public float SaveTime = 0;
		/// <summary>
		/// The time in between to save the game.
		/// </summary>
		[HideInInspector]
		public float TimeBetweenSaves = 2.500f;
		/// <summary>
		/// PlayerController
		/// </summary>
		[HideInInspector]
		public PlayerController2D PlayerController;
		#endregion

		// Use this for initialization
		void Awake()
		{
			GetGameDataManager();
			CheckPlatform();
			LoadLevelBoundires();
			PlayerController = GameObject.FindObjectOfType<PlayerController2D>();
			if (GameDataManager.NewSaveWorld)
			{
				SpawnPlayer();
				GameDataManager.NewSaveWorld = false;
			}
			else
			{
				LoadSaveData();
			}
			if (GameDataManager.Transitioned)
			{
				TransitionedToWorld();
			}
		}

		void Update()
		{
			CurrentTime += Time.smoothDeltaTime;
			SaveTime += Time.smoothDeltaTime;

			// Updates the score box text.
			ScorePanelText.text = "Score:" + GameDataManager.Score;
			// Set the player's position.
			PlayersPosition = GameObject.Find("Player").transform.position;

			if (Gems == MaxGems || GameDataManager.TotalGems >= MaxGems)
			{
				foreach (CLAEoS claeos in GameObject.FindGameObjectWithTag("LevelChangerManager").GetComponentsInChildren<CLAEoS>())
				{
					claeos.CanTransition = true;
				}
			}

			if (SaveGameTicks())
			{
				SaveGame();
			}
		}

		void LateUpdate()
		{
		}

		#region Startup Functions
		void CheckPlatform()
		{
			// If this is not a mobile game then we remove the mobile controls.
			if (InputCheck.IsPCPlatforms || InputCheck.IsEditorPlatforms || InputCheck.IsConsolePlatforms)
			{
				foreach (GameObject go in GameObject.FindGameObjectsWithTag(MobileControlsTag))
				{
					go.SetActive(false);
				}
			}
		}

		void LoadLevelBoundires()
		{
			// Sets the level boundaries of the map for the camera.
			LevelBoundries = new Rect();
			LevelBoundries.xMin = 0;
			LevelBoundries.xMax = Level.MapWidthInPixels * 2;
			LevelBoundries.yMin = -Level.MapHeightInPixels * 2;
			LevelBoundries.yMax = 0;
		}

		void GetGameDataManager()
		{
			if (FindObjectOfType<GameDataManager>() != null)
			{
				GameDataManager = FindObjectOfType<GameDataManager>();
			}
		}

		void SpawnPlayer()
		{
			// Spawn the player spawn, if it could be found.
			if (GameObject.Find("PlayerSpawn") && !IsLoaded && !GameDataManager.Transitioned)
			{
				GameObject.Find("Player").transform.position = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position + new Vector3(16, 16, 0);
			}
		}

		void TransitionedToWorld()
		{
			foreach (int gameObj in GameDataManager.DestroyedGameObjects)
			{
				if (EditorUtility.InstanceIDToObject(gameObj) != null && GameObject.Find(EditorUtility.InstanceIDToObject(gameObj).name) != null)
				{
					Destroy(GameObject.Find(EditorUtility.InstanceIDToObject(gameObj).name));
				}
				else
				{
					Debug.Log("Couldn't find DestoryedGameObjects");
				}
			}

			foreach (int gameObj in GameDataManager.ActivatedGameObjects)
			{
				if (EditorUtility.InstanceIDToObject(gameObj) != null && GameObject.Find(EditorUtility.InstanceIDToObject(gameObj).name) != null)
				{
					GameObject.Find(EditorUtility.InstanceIDToObject(gameObj).name).GetComponent<ObjectManager>().DoUpdate();
				}
				else
				{
					Debug.Log("Couldn't find DestoryedGameObjects");
				}
			}

			Debug.Log("Loaded");
		}
		#endregion

		#region Save Data Functions
		bool SaveGameTicks()
		{
			if (SaveTime >= TimeBetweenSaves)
			{
				SaveTime = 0;
				return true;
			}

			return false;
		}

		void SaveGame()
		{
			GameDataManager.SaveFile.PlayerData.Score = GameDataManager.Score;
			GameDataManager.SaveFile.PlayerData.TotalGems = GameDataManager.TotalGems;
			GameDataManager.SaveFile.PlayerData.Gems = Gems;
			GameDataManager.SaveFile.PlayerData.Keys = GameDataManager.Keys;
			GameDataManager.SaveFile.PlayerData.Level = CurrentLevel;
			GameDataManager.SaveFile.PlayerData.Position = PlayersPosition;
			GameDataManager.SaveFile.PlayerData.KeyIdentifiers = GameDataManager.KeyIdentifiers;
			GameDataManager.SaveFile.StoryLine = GameDataManager.StoryLine;
			GameDataManager.SaveGame();
		}

		void LoadSaveData()
		{
			var SaveFile = GameDataManager.SaveFile;

			GameObject.FindGameObjectWithTag("Player").transform.position = SaveFile.PlayerData.Position;
			GameDataManager.Score = SaveFile.PlayerData.Score;
			GameDataManager.TotalGems = SaveFile.PlayerData.TotalGems;
			Gems = SaveFile.PlayerData.Gems;
			GameDataManager.Keys = SaveFile.PlayerData.Keys;
			GameDataManager.KeyIdentifiers = SaveFile.PlayerData.KeyIdentifiers;
			GameDataManager.StoryLine = SaveFile.StoryLine;
			GameDataManager.DestroyedGameObjects = SaveFile.DestroyedGameObjects;
			GameDataManager.ActivatedGameObjects = SaveFile.ActivatedGameObjects;

			foreach (int gameObj in GameDataManager.DestroyedGameObjects)
			{
				if (EditorUtility.InstanceIDToObject(gameObj) != null && GameObject.Find(EditorUtility.InstanceIDToObject(gameObj).name) != null)
				{
					Destroy(GameObject.Find(EditorUtility.InstanceIDToObject(gameObj).name));
				}
			}

			foreach (int gameObj in GameDataManager.ActivatedGameObjects)
			{
				if (EditorUtility.InstanceIDToObject(gameObj) != null && GameObject.Find(EditorUtility.InstanceIDToObject(gameObj).name) != null)
				{
					GameObject.Find(EditorUtility.InstanceIDToObject(gameObj).name).GetComponent<ObjectManager>().DoUpdate();
				}
			}
		}
		#endregion

		public override string ToString()
		{
			return "GameManager | Not null!";
		}
	}
}