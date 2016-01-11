﻿using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace VoidInc
{
	public class GameDataManager : MonoBehaviour
	{
		/// <summary>
		/// The level has been transitioned.
		/// </summary>
		public bool Transitioned;
		/// <summary>
		/// If the game is loading a new world.
		/// </summary>
		public bool NewSaveWorld;
		/// <summary>
		/// The current amount of gems the player has.
		/// </summary>
		public int TotalGems;
		/// <summary>
		/// Gets or sets the score of the game.
		/// </summary>
		public int Score;
		/// <summary>
		/// The number of keys the player has on them.
		/// </summary>
		public int Keys;
		/// <summary>
		/// The list of key identifiers.
		/// </summary>
		public Dictionary<int, string> KeyIdentifiers = new Dictionary<int, string>();
		/// <summary>
		/// The identifier of the dialog the player has activated.
		/// </summary>
		public int StoryLine;
		public SaveFile SaveFile = new SaveFile();
		/// <summary>
		/// The list of destroyed game objects.
		/// </summary>
		public List<int> DestroyedGameObjects = new List<int>();
		/// <summary>
		/// The list of activated game objects.
		/// </summary>
		public List<int> ActivatedGameObjects = new List<int>();
		/// <summary>
		/// The ConfigFileManager to save and load with.
		/// </summary>
		private ConfigFileManager ConfigFileManager = new ConfigFileManager();

		void Awake()
		{
			SaveFile = ConfigFileManager.LoadSave();
		}

		void Update()
		{
		}

		public void SaveGame()
		{
			ConfigFileManager.SaveGame();
		}

		public override string ToString()
		{
			return "GameDataManager | Not null!";
		}
	}
}
