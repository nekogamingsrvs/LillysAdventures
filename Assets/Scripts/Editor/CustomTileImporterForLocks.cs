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
class CustomTileImportForLocks : Tiled2Unity.ICustomTiledImporter
{
	private Sprite _LockTexture;

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
		ItemDatabase itemDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();

		if (itemDatabase == null)
			return;

		_LockTexture = itemDatabase.LockTexture;

		var gameObj = GameObject.Find("Locks");
		if (gameObj == null)
			return;
		
		Collider2D[] colliderList = gameObj.GetComponentsInChildren<Collider2D>();

		if (colliderList == null)
			return;

		foreach (Collider2D collider in colliderList)
		{

			for (int i = 0; i < (int)(collider.bounds.size.y / 16); i++)
			{
				GameObject tempLock = new GameObject("Locks")
				{
					layer = LayerMask.NameToLayer("Locks")
				};

				tempLock.AddComponent<SpriteRenderer>();

				tempLock.GetComponent<SpriteRenderer>().sprite = _LockTexture;
				tempLock.GetComponent<SpriteRenderer>().sortingLayerName = "Locks";
				tempLock.GetComponent<SpriteRenderer>().sortingOrder = 0;
				collider.gameObject.AddComponent<LockIdentifier>();
				collider.GetComponent<BoxCollider2D>().isTrigger = true;

				tempLock.transform.parent = collider.gameObject.transform;

				tempLock.transform.localPosition = new Vector3(8, -(i * 16) + -8, 0);
			}
		}
	}
}
