using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VoidInc;

/// <summary>
/// Imports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
class CustomTileImportForDoors : Tiled2Unity.ICustomTiledImporter
{
	public Sprite InitialDoorTextureTop;
	public Sprite InitialDoorTextureBottom;

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

		InitialDoorTextureTop = itemDatabase.ClosedDoorTextureTop;
		InitialDoorTextureBottom = itemDatabase.ClosedDoorTextureBottom;

		if (props.ContainsKey("isDoorway"))
		{
			gameObject.AddComponent<DoorIdentifier>();
			gameObject.GetComponent<DoorIdentifier>().IsDoorway = Convert.ToBoolean(props["isDoorway"]);
			gameObject.GetComponent<DoorIdentifier>().LeadsTo = props["leadsTo"];
			gameObject.GetComponent<DoorIdentifier>().RequiresGems = Convert.ToBoolean(props["requiresGems"]);
			gameObject.GetComponent<DoorIdentifier>().AmountGems = Convert.ToInt32(props["amountGems"]);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

			#region Top Door
			GameObject tempDoorTop = new GameObject("Doors")
			{
				layer = LayerMask.NameToLayer("Doors")
			};

			tempDoorTop.AddComponent<SpriteRenderer>();

			tempDoorTop.GetComponent<SpriteRenderer>().sprite = InitialDoorTextureTop;
			tempDoorTop.GetComponent<SpriteRenderer>().sortingLayerName = "Doors";
			tempDoorTop.GetComponent<SpriteRenderer>().sortingOrder = 0;

			tempDoorTop.transform.parent = gameObject.transform;

			tempDoorTop.transform.localPosition = new Vector3(8, -8, 0);
			#endregion

			#region Bottom Door
			GameObject tempDoorBottom = new GameObject("Doors")
			{
				layer = LayerMask.NameToLayer("Doors")
			};

			tempDoorBottom.AddComponent<SpriteRenderer>();

			tempDoorBottom.GetComponent<SpriteRenderer>().sprite = InitialDoorTextureBottom;
			tempDoorBottom.GetComponent<SpriteRenderer>().sortingLayerName = "Doors";
			tempDoorBottom.GetComponent<SpriteRenderer>().sortingOrder = 0;

			tempDoorBottom.transform.parent = gameObject.transform;

			tempDoorBottom.transform.localPosition = new Vector3(8, -24, 0);
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
