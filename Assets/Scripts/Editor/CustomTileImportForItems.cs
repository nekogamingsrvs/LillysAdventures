using System;
using System.Collections.Generic;
using UnityEngine;
using VoidInc;

/// <summary>
/// Imports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
class CustomTileImportForItems : Tiled2Unity.ICustomTiledImporter
{
	Dictionary<int, string> KeyUUIDs = new Dictionary<int, string>();

	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		if (props.ContainsKey("lwa:item"))
		{
			LoadKeys(props);

			var collider = gameObject.GetComponent<Collider2D>();
			collider.isTrigger = true;

			if (props["lwa:item"] == "key")
			{
				GameObject tempGameObject = Resources.Load<GameObject>("Key");
				tempGameObject.GetComponent<ItemManager>().KeyID = Convert.ToInt32(props["lwa:keyID"]);
				tempGameObject.GetComponent<ItemManager>().Identifier = KeyUUIDs[Convert.ToInt32(props["lwa:keyID"])];
				tempGameObject.transform.position = gameObject.transform.position;
			}
			else if (props["lwa:item"] == "coin1")
			{
				GameObject tempGameObject = Resources.Load<GameObject>("Coin1");
				tempGameObject.transform.position = gameObject.transform.position;
			}
			else if (props["lwa:item"] == "coin5")
			{
				GameObject tempGameObject = Resources.Load<GameObject>("Coin5");
				tempGameObject.transform.position = gameObject.transform.position;
			}
			else if (props["lwa:item"] == "coin10")
			{
				GameObject tempGameObject = Resources.Load<GameObject>("Coin10");
				tempGameObject.transform.position = gameObject.transform.position;
			}
			else if (props["lwa:item"] == "gem")
			{
				GameObject tempGameObject = Resources.Load<GameObject>("Gem");
				tempGameObject.transform.position = gameObject.transform.position;
			}
		}
	}

	/// <summary>
	/// Adds the spines and other colliders to the ladders.
	/// </summary>
	/// <param name="prefab">The prefab of the level.</param>
	public void CustomizePrefab(GameObject prefab)
	{
	}

	public void LoadKeys(IDictionary<string, string> props)
	{
		foreach (string value in props.Values)
		{
			if (value == "key")
			{
				KeyUUIDs.Add(Convert.ToInt32(props["lwa:keyID"]), Guid.NewGuid().ToString());
			}
		}
	}
}
