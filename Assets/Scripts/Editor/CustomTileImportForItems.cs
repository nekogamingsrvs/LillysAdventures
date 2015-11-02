using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoidInc;

/// <summary>
/// Imports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
class CustomTileImportForItems : Tiled2Unity.ICustomTiledImporter
{
	public Sprite Coin1Texture;
	public Sprite Coin5Texture;
	public Sprite Coin10Texture;
	public Sprite GemTexture;
	public Sprite KeyTexture;

	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		ItemDatabase itemDatabaseObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();

		if (itemDatabaseObject == null)
			return;

		Coin1Texture = itemDatabaseObject.Coin1Texture;
		Coin5Texture = itemDatabaseObject.Coin5Texture;
		Coin10Texture = itemDatabaseObject.Coin10Texture;
		GemTexture = itemDatabaseObject.GemTexture;
		KeyTexture = itemDatabaseObject.KeyTexture;

		if (props.ContainsKey("keyId"))
		{
			gameObject.AddComponent<KeyIdentifier>();
			gameObject.GetComponent<KeyIdentifier>().Identifier = Convert.ToInt32(props["keyId"]);

			GameObject item = new GameObject("Keys")
			{
				layer = LayerMask.NameToLayer("Keys")
			};

			item.AddComponent<SpriteRenderer>();
			item.GetComponent<SpriteRenderer>().sprite = KeyTexture;
			item.GetComponent<SpriteRenderer>().sortingLayerName = "Keys";
			item.GetComponent<SpriteRenderer>().sortingOrder = 0;
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

			item.transform.parent = gameObject.transform;

			item.transform.localPosition = new Vector3(8, -8, 0);
		}
		if (props.ContainsKey("itemType") && props["itemType"] == "Coins1")
		{
			gameObject.AddComponent<CoinIdentifier>().CoinScore = 100;

			GameObject item = new GameObject("Coins1")
			{
				layer = LayerMask.NameToLayer("Coins1")
			};

			item.AddComponent<SpriteRenderer>();
			item.GetComponent<SpriteRenderer>().sprite = Coin1Texture;
			item.GetComponent<SpriteRenderer>().sortingLayerName = "Coins1";
			item.GetComponent<SpriteRenderer>().sortingOrder = 0;
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

			item.transform.parent = gameObject.transform;

			item.transform.localPosition = new Vector3(8, -8, 0);
		}
		if (props.ContainsKey("itemType") && props["itemType"] == "Coins5")
		{
			gameObject.AddComponent<CoinIdentifier>().CoinScore = 500;
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

			GameObject item = new GameObject("Coins5")
			{
				layer = LayerMask.NameToLayer("Coins5")
			};

			item.AddComponent<SpriteRenderer>();
			item.GetComponent<SpriteRenderer>().sprite = Coin5Texture;
			item.GetComponent<SpriteRenderer>().sortingLayerName = "Coins5";
			item.GetComponent<SpriteRenderer>().sortingOrder = 0;

			item.transform.parent = gameObject.transform;

			item.transform.localPosition = new Vector3(8, -8, 0);
		}
		if (props.ContainsKey("itemType") && props["itemType"] == "Coins10")
		{
			gameObject.AddComponent<CoinIdentifier>().CoinScore = 1000;

			GameObject item = new GameObject("Coins10")
			{
				layer = LayerMask.NameToLayer("Coins10")
			};

			item.AddComponent<SpriteRenderer>();
			item.GetComponent<SpriteRenderer>().sprite = Coin10Texture;
			item.GetComponent<SpriteRenderer>().sortingLayerName = "Coins10";
			item.GetComponent<SpriteRenderer>().sortingOrder = 0;
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

			item.transform.parent = gameObject.transform;

			item.transform.localPosition = new Vector3(8, -8, 0);
		}
		if (props.ContainsKey("itemType") && props["itemType"] == "Gems")
		{
			gameObject.AddComponent<GemIdentifier>().GemScore = 5000;

			GameObject item = new GameObject("Gems")
			{
				layer = LayerMask.NameToLayer("Gems")
			};

			item.AddComponent<SpriteRenderer>();
			item.GetComponent<SpriteRenderer>().sprite = GemTexture;
			item.GetComponent<SpriteRenderer>().sortingLayerName = "Gems";
			item.GetComponent<SpriteRenderer>().sortingOrder = 0;
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

			item.transform.parent = gameObject.transform;

			item.transform.localPosition = new Vector3(8, -8, 0);
		}
	}

	/// <summary>
	/// Adds the spines and other colliders to the ladders.
	/// </summary>
	/// <param name="prefab">The prefab of the level.</param>
	public void CustomizePrefab(GameObject prefab)
	{
	}
}
