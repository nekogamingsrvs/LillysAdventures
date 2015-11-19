using System;
using System.Collections.Generic;
using UnityEngine;
using VoidInc;

/// <summary>
/// Imports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
class CustomTileImportForLocks : Tiled2Unity.ICustomTiledImporter
{
	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		if (props.ContainsKey("lwa:lock"))
		{
			var lockIdentifier = gameObject.AddComponent<LockIdentifier>();
			lockIdentifier.Identifier = Convert.ToInt32(props["lwa:lockId"]);

			for (int i = 0; i < (int)(gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 16); i++)
			{
				var tempLock = new GameObject("Locks");
				tempLock.layer = LayerMask.NameToLayer("Locks");

				var spriteRenderer = tempLock.AddComponent<SpriteRenderer>();

				foreach (Sprite sp in Resources.LoadAll<Sprite>("kenney_items_16x16"))
				{
					if (sp.name == "Lock_1")
					{
						spriteRenderer.sprite = sp;
					}
				}

				spriteRenderer.sortingLayerName = "Objects";
				spriteRenderer.sortingOrder = 0;
				spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");

				tempLock.transform.parent = gameObject.transform;

				tempLock.transform.localPosition = new Vector3(0, -(i * 16), 0);
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
