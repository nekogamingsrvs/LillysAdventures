using System;
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
		public List<string> KeyIdentifiers;

		[JsonProperty("PlayerPositionWER_TeleTo")]
		public string PlayerPositionWER_TeleTo;

		[JsonProperty("PlayerPositionWER_Y")]
		public float PlayerPositionWER_Y;
    }

	public class SaveFile
	{
		[JsonProperty("version")]
		public string SaveVersion;

		[JsonProperty("player")]
		public PlayerData PlayerData = new PlayerData();

		[JsonProperty("destroyed")]
		public List<string> DestroyedGameObjects;

		[JsonProperty("dialog")]
		public int DialogNumber;
	}

	public class ConfigFileManager
	{
		public SaveFile SaveFile = new SaveFile();

		public void SaveGame()
		{
			using (FileStream fileStream = new FileStream("save.config", FileMode.Create))
			using (StreamWriter streamWriter = new StreamWriter(fileStream))
			using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
			{
				jsonWriter.Formatting = Formatting.Indented;

				JsonConvert.SerializeObject(SaveFile, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

				jsonWriter.Close();
				streamWriter.Close();
				fileStream.Close();
			}
		}

		public void LoadSave()
		{
			if (!File.Exists("save.json"))
			{
				SaveGame();
			}

			using (FileStream fileStream = File.OpenRead("save.json"))
			using (StreamReader streamReader = new StreamReader(fileStream))
			using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
			{
				JObject jsonSaveData = JObject.Load(jsonTextReader);

				SaveFile = jsonSaveData.ToObject<SaveFile>();

				jsonTextReader.Close();
				streamReader.Close();
				fileStream.Close();
			}
		}
	}
}