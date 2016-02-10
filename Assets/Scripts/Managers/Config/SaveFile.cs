using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace VoidInc.LWA
{
	public class PlayerData
	{
		/// <summary>
		/// The level number that the player is currently on.
		/// </summary>
		[JsonProperty("level")]
		public int Level = 0;
		/// <summary>
		/// The position of the player.
		/// </summary>
		[JsonProperty("position")]
		public Vector3 Position = Vector3.zero;
		/// <summary>
		/// The amount of lives the player has.
		/// </summary>
		[JsonProperty("lives")]
		public int Lives = 0;
		/// <summary>
		/// The current score the player has.
		/// </summary>
		[JsonProperty("score")]
		public int Score = 0;
		/// <summary>
		/// The current amount of gems the player has on the current level.
		/// </summary>
		[JsonProperty("gems")]
		public int Gems = 0;
		/// <summary>
		/// The total number of gems the player has obtained.
		/// </summary>
		[JsonProperty("total gems")]
		public int TotalGems = 0;
		/// <summary>
		/// The amount of keys the player has on them.
		/// </summary>
		[JsonProperty("keys")]
		public int Keys = 0;
		/// <summary>
		/// The key identifiers to unlock locks.
		/// </summary>
		[JsonProperty("key identifiers")]
		public Dictionary<int, string> KeyIdentifiers = new Dictionary<int, string>();
		/// <summary>
		/// The level changer to teleport the player to.
		/// </summary>
		[JsonProperty("teleport to")]
		public string PlayerTranisitionTeleTo = "";
		/// <summary>
		/// The level changer at position Y to teleport the player to.
		/// </summary>
		[JsonProperty("teleport position y")]
		public float PlayerTransitionPositionY = 0.0f;
	}

	public class SaveFile
	{
		/// <summary>
		/// The save version of the file.
		/// </summary>
		[JsonProperty("version")]
		public string SaveVersion = "";
		/// <summary>
		/// The player data of the save file.
		/// </summary>
		[JsonProperty("player")]
		public PlayerData PlayerData = new PlayerData();
		/// <summary>
		/// The list of destroyed game object's identifiers.
		/// </summary>
		[JsonProperty("destroyed")]
		public List<string> DestroyedGameObjects = new List<string>();
		/// <summary>
		/// The story line position that the story line is at.
		/// </summary>
		[JsonProperty("storyline")]
		public int StoryLine = 0;
	}
}