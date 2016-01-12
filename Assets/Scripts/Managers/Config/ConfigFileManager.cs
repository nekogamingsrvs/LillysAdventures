using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System;
using System.Reflection;

namespace VoidInc
{
	public class PlayerData
	{
		/// <summary>
		/// The level number that the player is currently on.
		/// </summary>
		[JsonProperty("level")]
		public int Level;
		/// <summary>
		/// The position of the player.
		/// </summary>
		[JsonProperty("position")]
		public Vector3 Position;
		/// <summary>
		/// The current score the player has.
		/// </summary>
		[JsonProperty("score")]
		public int Score;
		/// <summary>
		/// The current amount of gems the player has on the current level.
		/// </summary>
		[JsonProperty("gems")]
		public int Gems;
		/// <summary>
		/// The total number of gems the player has obtained.
		/// </summary>
		[JsonProperty("total gems")]
		public int TotalGems;
		/// <summary>
		/// The amount of keys the player has on them.
		/// </summary>
		[JsonProperty("keys")]
		public int Keys;
		/// <summary>
		/// The key identifiers to unlock locks.
		/// </summary>
		[JsonProperty("key identifiers")]
		public Dictionary<int, string> KeyIdentifiers = new Dictionary<int, string>();
		/// <summary>
		/// The level changer to teleport the player to.
		/// </summary>
		[JsonProperty("teleport to")]
		public string PlayerTranisitionTeleTo;
		/// <summary>
		/// The level changer at position Y to teleport the player to.
		/// </summary>
		[JsonProperty("teleport position y")]
		public float PlayerTransitionPositionY;
	}

	public class SaveFile
	{
		/// <summary>
		/// The save version of the file.
		/// </summary>
		[JsonProperty("version")]
		public string SaveVersion = "0.0.2";
		/// <summary>
		/// The player data of the save file.
		/// </summary>
		[JsonProperty("player")]
		public PlayerData PlayerData = new PlayerData();
		/// <summary>
		/// The list of destroyed game object's identifiers.
		/// </summary>
		[JsonProperty("destroyed")]
		public List<int> DestroyedGameObjects = new List<int>();
		/// <summary>
		/// The list of activated game object's identifiers.
		/// </summary>
		[JsonProperty("activated")]
		public List<int> ActivatedGameObjects = new List<int>();
		/// <summary>
		/// The story line position that the story line is at.
		/// </summary>
		[JsonProperty("storyline")]
		public int StoryLine;
	}

	public class ConfigFileManager
	{
		public SaveFile SaveFile = new SaveFile();

		/// <summary>
		/// Saves the game to a save file.
		/// </summary>
		public void SaveGame()
		{
			string json = JsonConvert.SerializeObject(SaveFile, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

			File.WriteAllText("save.json", json);
		}

		/// <summary>
		/// Loads the Save file and saves it into SaveData and returns the data too.
		/// </summary>
		/// <returns>SaveFile data.</returns>
		public SaveFile LoadSave()
		{
			if (!File.Exists("save.json"))
			{
				SaveGame();
			}
			else
			{
				using (FileStream fileStream = File.OpenRead("save.json"))
				using (StreamReader streamReader = new StreamReader(fileStream))
				using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
				{
					JObject jsonSaveData = JObject.Load(jsonTextReader);

					jsonTextReader.Close();
					streamReader.Close();
					fileStream.Close();

					SaveFile = jsonSaveData.ToObject<SaveFile>();

					return jsonSaveData.ToObject<SaveFile>();
				}
			}
			return null;
		}
	}
}