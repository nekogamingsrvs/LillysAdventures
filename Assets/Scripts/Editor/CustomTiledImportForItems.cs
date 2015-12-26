using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoidInc;

/// <summary>
/// Imports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
public class CustomTileImportForItems : Tiled2Unity.ICustomTiledImporter
{
	//public static Dictionary<int, string> KeyUUIDs = new Dictionary<int, string>();

	GameObject Key;
	GameObject Coin1;
	GameObject Coin5;
	GameObject Coin10;
	GameObject Gem;

	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		Key = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Key");
		Coin1 = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Coin1");
		Coin5 = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Coin5");
		Coin10 = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Coin10");
		Gem = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Gem");

		if (props.ContainsKey("lwa:item"))
		{
			//LoadKeys(props);

			var collider = gameObject.GetComponent<Collider2D>();
			collider.isTrigger = true;

			if (props["lwa:item"] == "key")
			{
				gameObject.AddComponent(Key.GetComponent<ItemManager>());
				gameObject.GetComponent<ItemManager>().KeyID = Convert.ToInt32(props["lwa:keyID"]);
				gameObject.GetComponent<ItemManager>().Identifier = props["lwa:keyID"]; //KeyUUIDs[Convert.ToInt32(props["lwa:keyID"])];
				gameObject.AddComponent(Key.AddComponent<SpriteRenderer>());
				gameObject.AddComponent(Key.AddComponent<Animator>());
			}
			else if (props["lwa:item"] == "coin1")
			{
				gameObject.AddComponent(Coin1.GetComponent<ItemManager>());
				gameObject.AddComponent(Coin1.AddComponent<SpriteRenderer>());
				gameObject.AddComponent(Coin1.AddComponent<Animator>());
			}
			else if (props["lwa:item"] == "coin5")
			{
				gameObject.AddComponent(Coin5.GetComponent<ItemManager>());
				gameObject.AddComponent(Coin5.AddComponent<SpriteRenderer>());
				gameObject.AddComponent(Coin5.AddComponent<Animator>());
			}
			else if (props["lwa:item"] == "coin10")
			{
				gameObject.AddComponent(Coin10.GetComponent<ItemManager>());
				gameObject.AddComponent(Coin10.AddComponent<SpriteRenderer>());
				gameObject.AddComponent(Coin10.AddComponent<Animator>());
			}
			else if (props["lwa:item"] == "gem")
			{
				gameObject.AddComponent(Gem.GetComponent<ItemManager>());
				gameObject.AddComponent(Gem.AddComponent<SpriteRenderer>());
				gameObject.AddComponent(Gem.AddComponent<Animator>());
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
