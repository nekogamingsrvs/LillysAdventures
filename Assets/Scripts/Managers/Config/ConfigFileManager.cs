using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

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
		#endregion
	}
}