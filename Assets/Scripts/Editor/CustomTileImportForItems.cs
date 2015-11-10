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
	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		if (props.ContainsKey("lwa:key"))
		{
			gameObject.AddComponent<KeyIdentifier>().Identifier = Convert.ToInt32(props["lwa:keyId"]);

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

			foreach (Sprite sp in Resources.LoadAll<Sprite>("kenney_items_16x16"))
			{
				if (sp.name == "Key_1")
				{
					spriteRenderer.sprite = sp;
				}
			}

			spriteRenderer.sortingLayerName = "Items";
			spriteRenderer.sortingOrder = 0;
			spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
		}
		if (props.ContainsKey("lwa:coin1"))
		{
			gameObject.AddComponent<CoinIdentifier>().CoinScore = 100;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

			foreach (Sprite sp in Resources.LoadAll<Sprite>("kenney_items_16x16"))
			{
				if (sp.name == "Coin1_1")
				{
					spriteRenderer.sprite = sp;
				}
			}

			spriteRenderer.sortingLayerName = "Items";
			spriteRenderer.sortingOrder = 0;
			spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
		}
		if (props.ContainsKey("lwa:coin5"))
		{
			gameObject.AddComponent<CoinIdentifier>().CoinScore = 500;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

			foreach (Sprite sp in Resources.LoadAll<Sprite>("kenney_items_16x16"))
			{
				if (sp.name == "Coin5_1")
				{
					spriteRenderer.sprite = sp;
				}
			}

			spriteRenderer.sortingLayerName = "Items";
			spriteRenderer.sortingOrder = 0;
			spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
		}
		if (props.ContainsKey("lwa:coin10"))
		{
			gameObject.AddComponent<CoinIdentifier>().CoinScore = 1000;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

			foreach (Sprite sp in Resources.LoadAll<Sprite>("kenney_items_16x16"))
			{
				if (sp.name == "Coin10_1")
				{
					spriteRenderer.sprite = sp;
				}
			}

			spriteRenderer.sortingLayerName = "Items";
			spriteRenderer.sortingOrder = 0;
			spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
		}
		if (props.ContainsKey("lwa:gem"))
		{
			gameObject.AddComponent<GemIdentifier>().GemScore = 5000;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

			foreach (Sprite sp in Resources.LoadAll<Sprite>("kenney_items_16x16"))
			{
				if (sp.name == "Gem_1")
				{
					spriteRenderer.sprite = sp;
				}
			}

			spriteRenderer.sortingLayerName = "Items";
			spriteRenderer.sortingOrder = 0;
			spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
		}
		if (props.ContainsKey("lwa:torch"))
		{
			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

			foreach (Sprite sp in Resources.LoadAll<Sprite>("torch"))
			{
				if (sp.name == "torch_0")
				{
					spriteRenderer.sprite = sp;
				}
			}

			spriteRenderer.sortingLayerName = "Objects";
			spriteRenderer.sortingOrder = 0;
			spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");

			var lightObj = new GameObject("Light");

			var light = lightObj.AddComponent<Light>();
			light.color = new Color(1, 1, 1);
			light.range = 50;
			light.intensity = 4;
			light.bounceIntensity = 0;

			lightObj.transform.SetParent(gameObject.transform);
			lightObj.transform.localPosition = new Vector3(8, -4, -25);

			var animator = gameObject.AddComponent<Animator>();
			animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("TorchAnimator");
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
