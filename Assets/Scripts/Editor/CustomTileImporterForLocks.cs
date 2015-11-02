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
	public Sprite BlockLockTexture;

	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		ItemDatabase itemDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();

		if (itemDatabase == null)
			return;

		BlockLockTexture = itemDatabase.LockTexture;

		if (props.ContainsKey("lockId"))
		{
			gameObject.AddComponent<LockIdentifier>();
			gameObject.GetComponent<LockIdentifier>().Identifier = Convert.ToInt32(props["lockId"]);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

			for (int i = 0; i < gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 16; i++)
			{
				GameObject tempLock = new GameObject("Locks")
				{
					layer = LayerMask.NameToLayer("Locks")
				};

				tempLock.AddComponent<SpriteRenderer>();

				tempLock.GetComponent<SpriteRenderer>().sprite = BlockLockTexture;
				tempLock.GetComponent<SpriteRenderer>().sortingLayerName = "Locks";
				tempLock.GetComponent<SpriteRenderer>().sortingOrder = 0;

				tempLock.transform.parent = gameObject.transform;

				tempLock.transform.localPosition = new Vector3(8, -(i * 16) + -8, 0);
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
}
