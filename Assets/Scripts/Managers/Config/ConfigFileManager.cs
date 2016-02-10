using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VoidInc.LWA
{
	/// <summary>
	/// The manager for config and save files in Lilly's Wonderful Adventures.
	/// </summary>
	public class ConfigFileManager
	{
		#region Save Files
		/// <summary>
		/// The save file for the game.
		/// </summary>
		public SaveFile SaveFile = new SaveFile();
		/// <summary>
		/// The current save file version
		/// </summary>
		public string SaveFileVersion = "0.0.1";

		/// <summary>
		/// Saves the game to a save file.
		/// </summary>
		public void SaveGame()
		{
			SaveFile.SaveVersion = SaveFileVersion;

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

					SaveFile saveFile = jsonSaveData.ToObject<SaveFile>();

					if (CheckSaveFileVersion(SaveFileVersion, saveFile.SaveVersion))
					{
						SaveFile = jsonSaveData.ToObject<SaveFile>();

						return jsonSaveData.ToObject<SaveFile>();
					}
					else
					{
						return null;
					}
				}
			}
			return null;
		}
		#endregion

		#region Save file version check
		bool CheckSaveFileVersion(string currentVersion, string savedVersion)
		{
			int cmajor = 0, cminor = 0, cdev = 0;
			int smajor = 0, sminor = 0, sdev = 0;

			cmajor = Convert.ToInt32(currentVersion.Substring(0, 1));
			cminor = Convert.ToInt32(currentVersion.Substring(2, 1));
			cdev = Convert.ToInt32(currentVersion.Substring(4, 1));

			smajor = Convert.ToInt32(savedVersion.Substring(0, 1));
			sminor = Convert.ToInt32(savedVersion.Substring(2, 1));
			sdev = Convert.ToInt32(savedVersion.Substring(4, 1));

			if (sdev >= cdev)
			{
				if (sminor >= cminor)
				{
					if (smajor >= cmajor)
					{
						return true;
					}
				}
			}
			return false;
		}
		#endregion
	}
}