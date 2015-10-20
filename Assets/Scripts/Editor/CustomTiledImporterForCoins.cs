using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Inports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterForCoins1 : Tiled2Unity.ICustomTiledImporter
{
	private Sprite coinsTexture;

	/// <summary>
	/// Handtles custom properties, does nothing.
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
		ItemDatabase itemDatabseObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();
		coinsTexture = itemDatabseObject.Coin1Texture;

		Collider2D[] coinColliderList = new Collider2D[0];

		// Find all the polygon colliders in the pefab
		var gameObj = GameObject.Find("Coins1");
		if (gameObj == null)
			return;

		coinColliderList = gameObj.GetComponentsInChildren<Collider2D>();

		if (coinColliderList == null)
		{
			Debug.LogError("Error: coinColliderList: is <null>.");
		}

		// For each ladder path in a ladder polygon collider
		// add a top, spine, and bottom edge collider
		foreach (var coins in coinColliderList)
		{
			GameObject coinImage = new GameObject("Coins");
			coinImage.layer = LayerMask.NameToLayer("Coins");
			coinImage.AddComponent<SpriteRenderer>();

			var coinRenderer = coinImage.GetComponent<SpriteRenderer>();

			coinRenderer.sprite = coinsTexture;
			coinRenderer.sortingLayerName = "Coin1";
			coinRenderer.sortingOrder = 0;

			coins.gameObject.AddComponent<CoinIdentifier>().CoinScore = 100;

			// Parent the ladder components to our ladder object
			coinImage.transform.parent = coins.gameObject.transform;

			coinImage.transform.localPosition = new Vector3(0, 0, 0);
		}
	}
}

/// <summary>
/// Inports the coins' controller objects for Coins5
/// </summary>
[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterForCoins5 : Tiled2Unity.ICustomTiledImporter
{
	private Sprite coinsTexture;

	/// <summary>
	/// Handtles custom properties, does nothing.
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
		ItemDatabase itemDatabseObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();
		coinsTexture = itemDatabseObject.Coin5Texture;

		Collider2D[] coinColliderList = new Collider2D[0];

		// Find all the polygon colliders in the pefab
		var gameObj = GameObject.Find("Coins5");
		if (gameObj == null)
			return;

		coinColliderList = gameObj.GetComponentsInChildren<Collider2D>();

		if (coinColliderList == null)
		{
			Debug.LogError("Error: coinColliderList: is <null>.");
		}

		// For each ladder path in a ladder polygon collider
		// add a top, spine, and bottom edge collider
		foreach (var coins in coinColliderList)
		{
			GameObject coinImage = new GameObject("Coins");
			coinImage.layer = LayerMask.NameToLayer("Coins");
			coinImage.AddComponent<SpriteRenderer>();

			var coinRenderer = coinImage.GetComponent<SpriteRenderer>();

			coinRenderer.sprite = coinsTexture;
			coinRenderer.sortingLayerName = "Coin5";
			coinRenderer.sortingOrder = 0;

			coins.gameObject.AddComponent<CoinIdentifier>().CoinScore = 500;

			// Parent the ladder components to our ladder object
			coinImage.transform.parent = coins.gameObject.transform;

			coinImage.transform.localPosition = new Vector3(0, 0, 0);
		}
	}
}

/// <summary>
/// Inports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterForCoins10 : Tiled2Unity.ICustomTiledImporter
{
	private Sprite coinsTexture;

	/// <summary>
	/// Handtles custom properties, does nothing.
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
		ItemDatabase itemDatabseObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();
		coinsTexture = itemDatabseObject.Coin10Texture;

		Collider2D[] coinColliderList = new Collider2D[0];

		// Find all the polygon colliders in the pefab
		var gameObj = GameObject.Find("Coins10");
		if (gameObj == null)
			return;

		coinColliderList = gameObj.GetComponentsInChildren<Collider2D>();

		if (coinColliderList == null)
		{
			Debug.LogError("Error: coinColliderList: is <null>.");
		}

		// For each ladder path in a ladder polygon collider
		// add a top, spine, and bottom edge collider
		foreach (var coins in coinColliderList)
		{
			GameObject coinImage = new GameObject("Coins");
			coinImage.layer = LayerMask.NameToLayer("Coins");
			coinImage.AddComponent<SpriteRenderer>();

			var coinRenderer = coinImage.GetComponent<SpriteRenderer>();

			coinRenderer.sprite = coinsTexture;
			coinRenderer.sortingLayerName = "Coin10";
			coinRenderer.sortingOrder = 0;

			coins.gameObject.AddComponent<CoinIdentifier>().CoinScore = 1000;

			// Parent the ladder components to our ladder object
			coinImage.transform.parent = coins.gameObject.transform;

			coinImage.transform.localPosition = new Vector3(0, 0, 0);
		}
	}
}