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
		public int Level;
		/// <summary>
		/// The position of the player.
		/// </summary>
		[JsonProperty("position")]
		public Vector3 Position;
		[JsonProperty("lives")]
		public int Lives;
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
		public List<string> DestroyedGameObjects = new List<string>();
		/// <summary>
		/// The story line position that the story line is at.
		/// </summary>
		[JsonProperty("storyline")]
		public int StoryLine;
	}
}