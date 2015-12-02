using System;
using System.Collections.Generic;
using UnityEngine;
using VoidInc;

/// <summary>
/// Imports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
class CustomTileImportForItems : Tiled2Unity.ICustomTiledImporter
{
	Dictionary<string, string> KeyUUIDs = new Dictionary<string, string>();
	Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		if (props.ContainsKey("lwa:item"))
		{
			LoadKeys(props);
			LoadSprites();

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			var collider = gameObject.GetComponent<Collider2D>();
			collider.isTrigger = true;

			if (props["lwa:item"] == "Key")
			{
				var itemIdentifier = gameObject.AddComponent<ItemIdentifier>();
				itemIdentifier.Identifier = KeyUUIDs[props["lwa:keyId"]];
				itemIdentifier.ItemType = ItemIdentifier._ItemType.Key;
				spriteRenderer.sprite = Sprites["Key_1"];
				spriteRenderer.sortingLayerName = "Items";
				spriteRenderer.sortingOrder = 0;
				spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
			}
			else if (props["lwa:item"] == "Coin1")
			{
				var itemIdentifier = gameObject.AddComponent<ItemIdentifier>();
				itemIdentifier.Score = 100;
				itemIdentifier.ItemType = ItemIdentifier._ItemType.Coin;
				spriteRenderer.sprite = Sprites["Coin1_1"];
				spriteRenderer.sortingLayerName = "Items";
				spriteRenderer.sortingOrder = 1;
				spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
			}
			else if (props["lwa:item"] == "Coin5")
			{
				var itemIdentifier = gameObject.AddComponent<ItemIdentifier>();
				itemIdentifier.Score = 500;
				itemIdentifier.ItemType = ItemIdentifier._ItemType.Coin;
				spriteRenderer.sprite = Sprites["Coin5_1"];
				spriteRenderer.sortingLayerName = "Items";
				spriteRenderer.sortingOrder = 2;
				spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
			}
			else if (props["lwa:item"] == "Coin10")
			{
				var itemIdentifier = gameObject.AddComponent<ItemIdentifier>();
				itemIdentifier.Score = 1000;
				itemIdentifier.ItemType = ItemIdentifier._ItemType.Coin;
				spriteRenderer.sprite = Sprites["Coin10_1"];
				spriteRenderer.sortingLayerName = "Items";
				spriteRenderer.sortingOrder = 3;
				spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
			}
			else if (props["lwa:item"] == "Gem")
			{
				var itemIdentifier = gameObject.AddComponent<ItemIdentifier>();
				itemIdentifier.Score = 5000;
				itemIdentifier.ItemType = ItemIdentifier._ItemType.Gem;
				spriteRenderer.sprite = Sprites["Gem_1"];
				spriteRenderer.sortingLayerName = "Items";
				spriteRenderer.sortingOrder = 4;
				spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
			}
			else if (props["lwa:item"] == "Lock")
			{
				var itemIdentifier = gameObject.AddComponent<ItemIdentifier>();
				itemIdentifier.Identifier = KeyUUIDs[props["lwa:lockId"]];
				itemIdentifier.ItemType = ItemIdentifier._ItemType.Key;
				spriteRenderer.sprite = Sprites["Lock_1"];
				spriteRenderer.sortingLayerName = "Objects";
				spriteRenderer.sortingOrder = 0;
				spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
			}
			else if (props["lwa:item"] == "Torch")
			{
				spriteRenderer.sprite = Sprites["Torch_0"];
				spriteRenderer.sortingLayerName = "Objects";
				spriteRenderer.sortingOrder = 0;
				spriteRenderer.material = Resources.Load<Material>("DiffuseSprite");
					
                var light = gameObject.AddComponent<Light>();
				light.color = new Color(1, 1, 1);
				light.range = 50;
				light.intensity = 4;
				light.bounceIntensity = 0;

				gameObject.transform.localPosition += new Vector3(8, -8, 0);
					
                var animator = gameObject.AddComponent<Animator>();
				animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("TorchAnimator");
			}
			else if (props["lwa:item"] == "Sign")
			{
			}
			else
			{
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
			if (value == "Key")
			{
				KeyUUIDs.Add(props["lwa:keyId"], Guid.NewGuid().ToString());
			}
		}
	}

	public void LoadSprites()
	{
		foreach (Sprite sprite in Resources.LoadAll<Sprite>("kenney_items_16x16"))
		{
			if (!Sprites.ContainsKey(sprite.name))
				Sprites.Add(sprite.name, sprite);
		}
		foreach (Sprite sprite in Resources.LoadAll<Sprite>("torch"))
		{
			if (!Sprites.ContainsKey(sprite.name))
				Sprites.Add(sprite.name, sprite);
		}
	}
}
