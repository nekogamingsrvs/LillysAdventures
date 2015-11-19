using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VoidInc
{
	public class ConfigFileSetting
	{
		public string settingName;

		public object settingValue;
	}

	public class ConfigFileManager
	{
		private List<ConfigFileSetting> configFileSettings = new List<ConfigFileSetting>();

		public void AddSettingToList(ConfigFileSetting configFileSetting)
		{
			configFileSettings.Add(configFileSetting);
		}

		public void Save()
		{
			using (FileStream fileStream = new FileStream("save.config", FileMode.Create))
			using (StreamWriter streamWriter = new StreamWriter(fileStream))
			using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
			{
				jsonWriter.Formatting = Formatting.Indented;

				JsonSerializer jsonSerializer = new JsonSerializer();

				jsonSerializer.Serialize(jsonWriter, configFileSettings);

				byte[] info = new UTF8Encoding(true).GetBytes(jsonSerializer.ToString());
				fileStream.Write(info, 0, info.Length);

				jsonWriter.Close();
				streamWriter.Close();
				fileStream.Close();
			}
		}

		public T LoadPart<T>(string property)
		{
			using (FileStream fileStream = File.OpenRead("save.config"))
			using (StreamReader streamReader = new StreamReader(fileStream))
			{
				List<ConfigFileSetting> prasedData = JsonConvert.DeserializeObject<List<ConfigFileSetting>>(streamReader.ToString());

				streamReader.Close();
				fileStream.Close();

				ConfigFileSetting foundData = prasedData.Find(element => element.settingName == property);

				return (T)Convert.ChangeType(foundData.settingValue, typeof(T));
			}
		}
	}
}