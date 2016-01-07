using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace VoidInc
{
	public class PlayerData
	{
		[JsonProperty("level")]
		public int Level;

		[JsonProperty("position")]
		public Vector3 Position;

		[JsonProperty("score")]
		public int Score;

		[JsonProperty("gems")]
		public int Gems;

		[JsonProperty("total gems")]
		public int TotalGems;

		[JsonProperty("keys")]
		public int Keys;

		[JsonProperty("key identifiers")]
		public Dictionary<int, string> KeyIdentifiers = new Dictionary<int, string>();

		[JsonProperty("player position tele to")]
		public string PlayerPositionWER_TeleTo;

		[JsonProperty("player transition y")]
		public float PlayerPositionWER_Y;

		[JsonProperty("Transitioned")]
		public bool Transitioned;
	}

	public class SaveFile
	{
		[JsonProperty("version")]
		public string SaveVersion;

		[JsonProperty("new save")]
		public bool NewSave = true;

		[JsonProperty("player")]
		public PlayerData PlayerData = new PlayerData();

		[JsonProperty("destroyed")]
		public List<int> DestroyedGameObjects = new List<int>();

		[JsonProperty("activated")]
		public List<int> ActivatedGameObjects = new List<int>();

		[JsonProperty("storyline")]
		public int StoryLine;
	}

	public class ConfigFileManager
	{
		public SaveFile SaveFile = new SaveFile();

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

		public void ResetFile()
		{
			SaveFile.NewSave = true;
		}
	}
}