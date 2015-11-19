using System;
using System.Collections.Generic;
using UnityEngine;
using VoidInc;

/// <summary>
/// Imports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
class CustomTileImportForDoors : Tiled2Unity.ICustomTiledImporter
{
	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		if (props.ContainsKey("lwa:door"))
		{
			var doorIdentifier = gameObject.AddComponent<DoorIdentifier>();
			doorIdentifier.IsDoorway = true;
			doorIdentifier.LeadsTo = props["lwa:leadsTo"];
			doorIdentifier.RequiresGems = Convert.ToBoolean(props["lwa:requiresGems"]);
			doorIdentifier.AmountGems = Convert.ToInt32(props["lwa:requiredGems"]);

			#region Top Door
			{
				var tempDoorTop = new GameObject("Doors");
				tempDoorTop.layer = LayerMask.NameToLayer("Doors");

				var spriteRenderer = tempDoorTop.AddComponent<SpriteRenderer>();

				foreach (Sprite sp in Resources.LoadAll<Sprite>("kenney_items_16x16"))
				{
					if (sp.name == "Door1_1")
					{
						spriteRenderer.sprite = sp;
					}
				}

				spriteRenderer.sortingLayerName = "Objects";
				spriteRenderer.sortingOrder = 0;
				spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");

				tempDoorTop.transform.parent = gameObject.transform;

				tempDoorTop.transform.localPosition = new Vector3(0, 0, 0);
			}
			#endregion

			#region Bottom Door
			{
				var tempDoorBottom = new GameObject("Doors");
				tempDoorBottom.layer = LayerMask.NameToLayer("Doors");

				var spriteRenderer = tempDoorBottom.AddComponent<SpriteRenderer>();

				foreach (Sprite sp in Resources.LoadAll<Sprite>("kenney_items_16x16"))
				{
					if (sp.name == "Door1_2")
					{
						spriteRenderer.sprite = sp;
					}
				}

				spriteRenderer.sortingLayerName = "Objects";
				spriteRenderer.sortingOrder = 0;
				spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");

				tempDoorBottom.transform.parent = gameObject.transform;

				tempDoorBottom.transform.localPosition = new Vector3(0, -16, 0);
			}
			#endregion
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
