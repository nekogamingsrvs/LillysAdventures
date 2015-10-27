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
	private Sprite _Coin1Texture;
	private Sprite _Coin5Texture;
	private Sprite _Coin10Texture;
	private Sprite _GemTexture;
	private Sprite _KeyTexture;

	private string[] ItemLayers = { "Coins1", "Coins5", "Coins10", "Gems", "Keys" };

	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		// Do nothing
	}

	/// <summary>
	/// Adds the spines and other colliders to the ladders.
	/// </summary>
	/// <param name="prefab">The prefab of the level.</param>
	public void CustomizePrefab(GameObject prefab)
	{
		ItemDatabase itemDatabaseObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();

		if (itemDatabaseObject == null)
			return;

		_Coin1Texture = itemDatabaseObject.Coin1Texture;
		_Coin5Texture = itemDatabaseObject.Coin5Texture;
		_Coin10Texture = itemDatabaseObject.Coin10Texture;
		_GemTexture = itemDatabaseObject.GemTexture;
		_KeyTexture = itemDatabaseObject.KeyTexture;

		foreach (string s in ItemLayers)
		{
			var gameObj = GameObject.Find(s);
			if (gameObj == null)
				return;
			
			Collider2D[] colliderList = gameObj.GetComponentsInChildren<Collider2D>();

			if (colliderList == null)
				return;

			foreach (Collider2D collider in colliderList)
			{
				GameObject item = new GameObject(s)
				{
					layer = LayerMask.NameToLayer(s)
				};

				item.AddComponent<SpriteRenderer>();

				if (s == ItemLayers[0])
				{
					item.GetComponent<SpriteRenderer>().sprite = _Coin1Texture;
					item.GetComponent<SpriteRenderer>().sortingLayerName = s;
					item.GetComponent<SpriteRenderer>().sortingOrder = 0;
					collider.gameObject.AddComponent<CoinIdentifier>().CoinScore = 100;
					collider.GetComponent<BoxCollider2D>().isTrigger = true;
				}
				else if (s == ItemLayers[1])
				{
					item.GetComponent<SpriteRenderer>().sprite = _Coin5Texture;
					item.GetComponent<SpriteRenderer>().sortingLayerName = s;
					item.GetComponent<SpriteRenderer>().sortingOrder = 0;
					collider.gameObject.AddComponent<CoinIdentifier>().CoinScore = 500;
					collider.GetComponent<BoxCollider2D>().isTrigger = true;
				}
				else if (s == ItemLayers[2])
				{
					item.GetComponent<SpriteRenderer>().sprite = _Coin10Texture;
					item.GetComponent<SpriteRenderer>().sortingLayerName = s;
					item.GetComponent<SpriteRenderer>().sortingOrder = 0;
					collider.gameObject.AddComponent<CoinIdentifier>().CoinScore = 1000;
					collider.GetComponent<BoxCollider2D>().isTrigger = true;
				}
				else if (s == ItemLayers[3])
				{
					item.GetComponent<SpriteRenderer>().sprite = _GemTexture;
					item.GetComponent<SpriteRenderer>().sortingLayerName = s;
					item.GetComponent<SpriteRenderer>().sortingOrder = 0;
					collider.gameObject.AddComponent<GemIdentifier>().GemScore = 5000;
					collider.GetComponent<BoxCollider2D>().isTrigger = true;
				}
				else if (s == ItemLayers[4])
				{
					item.GetComponent<SpriteRenderer>().sprite = _KeyTexture;
                    item.GetComponent<SpriteRenderer>().sortingLayerName = s;
					item.GetComponent<SpriteRenderer>().sortingOrder = 0;
					collider.gameObject.AddComponent<KeyIdentifier>();
					collider.GetComponent<BoxCollider2D>().isTrigger = true;
				}
				else
				{
					return;
				}

				item.transform.parent = collider.gameObject.transform;

				item.transform.localPosition = new Vector3(8, -8, 0);
			}
		}
	}
}
