using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VoidInc;

/// <summary>
/// Imports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
class CustomTileImportForObjects : Tiled2Unity.ICustomTiledImporter
{
	Dictionary<int, string> KeyUUIDs = new Dictionary<int, string>();

	Dictionary<string, Sprite> ItemDictonary = new Dictionary<string, Sprite>();

	RuntimeAnimatorController Coin1Animator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/Animators/Coin1Animator.controller");
	RuntimeAnimatorController Coin5Animator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/Animators/Coin5Animator.controller");
	RuntimeAnimatorController Coin10Animator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/Animators/Coin10Animator.controller");
	RuntimeAnimatorController GemAnimator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/Animators/GemAnimator.controller");
	RuntimeAnimatorController KeyAnimator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/Animators/KeyAnimator.controller");
	RuntimeAnimatorController TorchAnimator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/Animators/TorchAnimator.controller");
	RuntimeAnimatorController SignAnimator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/Animators/SignAnimator.controller");

	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		LoadKeys(props);
		LoadImages();

		var collider = gameObject.GetComponent<Collider2D>();
		collider.isTrigger = true;

		if (props.ContainsKey("lwa:point") && props["lwa:point"] == "playerSpawn")
		{
			gameObject.name = "PlayerSpawn";
			gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			gameObject.tag = "PlayerSpawn";
		}
		if (props.ContainsKey("lwa:item") && props["lwa:item"] == "key")
		{
			gameObject.name = "Key";
			gameObject.layer = LayerMask.NameToLayer("Items");
			gameObject.tag = "Items";

			var itemManager = gameObject.AddComponent<ItemManager>();
			itemManager.ItemType = ItemManager._ItemType.Key;
			itemManager.KeyID = Convert.ToInt32(props["lwa:keyID"]);
			itemManager.Identifier = KeyUUIDs[Convert.ToInt32(props["lwa:keyID"])];

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = ItemDictonary["key_frame_0"];
			spriteRenderer.sortingLayerName = "Items";
			spriteRenderer.sortingOrder = 0;

			var animator = gameObject.AddComponent<Animator>();
			animator.runtimeAnimatorController = KeyAnimator;
		}
		else if (props.ContainsKey("lwa:item") && props["lwa:item"] == "coin1")
		{
			gameObject.name = "Coin1";
			gameObject.layer = LayerMask.NameToLayer("Items");
			gameObject.tag = "Items";

			var itemManager = gameObject.AddComponent<ItemManager>();
			itemManager.ItemType = ItemManager._ItemType.Coin1;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = ItemDictonary["coin1_frame_0"];
			spriteRenderer.sortingLayerName = "Items";
			spriteRenderer.sortingOrder = 1;

			var animator = gameObject.AddComponent<Animator>();
			animator.runtimeAnimatorController = Coin1Animator;
		}
		else if (props.ContainsKey("lwa:item") && props["lwa:item"] == "coin5")
		{
			gameObject.name = "Coin5";
			gameObject.layer = LayerMask.NameToLayer("Items");
			gameObject.tag = "Items";

			var itemManager = gameObject.AddComponent<ItemManager>();
			itemManager.ItemType = ItemManager._ItemType.Coin5;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = ItemDictonary["coin5_frame_0"];
			spriteRenderer.sortingLayerName = "Items";
			spriteRenderer.sortingOrder = 2;

			var animator = gameObject.AddComponent<Animator>();
			animator.runtimeAnimatorController = Coin5Animator;
		}
		else if (props.ContainsKey("lwa:item") && props["lwa:item"] == "coin10")
		{
			gameObject.name = "Coin10";
			gameObject.layer = LayerMask.NameToLayer("Items");
			gameObject.tag = "Items";

			var itemManager = gameObject.AddComponent<ItemManager>();
			itemManager.ItemType = ItemManager._ItemType.Coin10;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = ItemDictonary["coin10_frame_0"];
			spriteRenderer.sortingLayerName = "Items";
			spriteRenderer.sortingOrder = 3;

			var animator = gameObject.AddComponent<Animator>();
			animator.runtimeAnimatorController = Coin10Animator;
		}
		else if (props.ContainsKey("lwa:item") && props["lwa:item"] == "gem")
		{
			gameObject.name = "Gem";
			gameObject.layer = LayerMask.NameToLayer("Items");
			gameObject.tag = "Items";

			var itemManager = gameObject.AddComponent<ItemManager>();
			itemManager.ItemType = ItemManager._ItemType.Gem;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = ItemDictonary["gem_frame_0"];
			spriteRenderer.sortingLayerName = "Items";
			spriteRenderer.sortingOrder = 4;

			var animator = gameObject.AddComponent<Animator>();
			animator.runtimeAnimatorController = GemAnimator;
		}
		else if (props.ContainsKey("lwa:object") && props["lwa:object"] == "torch")
		{
			gameObject.name = "Torch";
			gameObject.layer = LayerMask.NameToLayer("Objects");
			gameObject.tag = "Objects";

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = ItemDictonary["torch_frame_0"];
			spriteRenderer.sortingLayerName = "Objects";
			spriteRenderer.sortingOrder = 0;

			var animator = gameObject.AddComponent<Animator>();
			animator.runtimeAnimatorController = TorchAnimator;

			GameObject tempTorchLight = new GameObject("TorchLight");

			var light = tempTorchLight.AddComponent<Light>();
			light.type = LightType.Point;
			light.range = 100;
			light.intensity = 6;
			light.bounceIntensity = 0;
			light.color = Color.white;

			tempTorchLight.transform.parent = gameObject.transform;
			tempTorchLight.transform.localPosition = new Vector3(16, -16, -50);
		}
		else if (props.ContainsKey("lwa:object") && props["lwa:object"] == "lock")
		{
			gameObject.name = "Lock";
			gameObject.layer = LayerMask.NameToLayer("Locks");
			gameObject.tag = "Objects";

			var objectManager = gameObject.AddComponent<ObjectManager>();
			objectManager.Type = ObjectManager.ObjectType.Lock;
			objectManager.KeyID = Convert.ToInt32(props["lwa:keyID"]);
			objectManager.Identifier = KeyUUIDs[Convert.ToInt32(props["lwa:keyID"])];

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = ItemDictonary["lock_frame_0"];
			spriteRenderer.sortingLayerName = "Objects";
			spriteRenderer.sortingOrder = 1;
		}
		else if (props.ContainsKey("lwa:sign"))
		{
			gameObject.name = "Sign";
			gameObject.layer = LayerMask.NameToLayer("Signs");
			gameObject.tag = "Objects";

			var objectManager = gameObject.AddComponent<ObjectManager>();
			objectManager.Type = ObjectManager.ObjectType.Sign;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = ItemDictonary["sign_frame_0"];
			spriteRenderer.sortingLayerName = "Objects";
			spriteRenderer.sortingOrder = 2;

			var animator = gameObject.AddComponent<Animator>();
			animator.runtimeAnimatorController = SignAnimator;

			if (props["lwa:sign"] == "save")
			{
				objectManager.SignType.Type = ObjectManager.Sign.SignType.save;
			}
			else if (props["lwa:sign"] == "heal")
			{
				objectManager.SignType.Type = ObjectManager.Sign.SignType.heal;
			}
			else if (props["lwa:sign"] == "dialog")
			{
				objectManager.SignType.Type = ObjectManager.Sign.SignType.dialog;
				objectManager.SignType.Dialog = props["lwa:sign:dialog"];
				animator.SetInteger("SignType", Convert.ToInt32(props["lwa:sign:skin"]));
			}
		}
		else if (props.ContainsKey("lwa:itemBlock"))
		{
			gameObject.name = "ItemBlock";
			gameObject.layer = LayerMask.NameToLayer("ItemBlocks");
			gameObject.tag = "Objects";

			var objectManager = gameObject.AddComponent<ObjectManager>();
			objectManager.Type = ObjectManager.ObjectType.ItemBlock;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = ItemDictonary["itemBlock_frame_0"];
			spriteRenderer.sortingLayerName = "Objects";
		    spriteRenderer.sortingOrder = 3;

			if (props["lwa:itemBlock"] == "key")
			{
				objectManager.GameObjectSpawn = GenerateKey(props["lwa:keyID"]);
				objectManager.GameObjectSpawn.transform.parent = gameObject.transform;
				objectManager.GameObjectSpawn.transform.localPosition = Vector3.zero;
				objectManager.GameObjectSpawn.SetActive(false);
			}
			else if (props["lwa:itemBlock"] == "coin1")
			{
				objectManager.GameObjectSpawn = GenerateCoin1();
				objectManager.GameObjectSpawn.transform.parent = gameObject.transform;
				objectManager.GameObjectSpawn.transform.localPosition = Vector3.zero;
				objectManager.GameObjectSpawn.SetActive(false);
			}
			else if (props["lwa:itemBlock"] == "coin5")
			{
				objectManager.GameObjectSpawn = GenerateCoin5();
				objectManager.GameObjectSpawn.transform.parent = gameObject.transform;
				objectManager.GameObjectSpawn.transform.localPosition = Vector3.zero;
				objectManager.GameObjectSpawn.SetActive(false);
			}
			else if (props["lwa:itemBlock"] == "coin10")
			{
				objectManager.GameObjectSpawn = GenerateCoin10();
				objectManager.GameObjectSpawn.transform.parent = gameObject.transform;
				objectManager.GameObjectSpawn.transform.localPosition = Vector3.zero;
				objectManager.GameObjectSpawn.SetActive(false);
			}
			else if (props["lwa:itemBlock"] == "gem")
			{
				objectManager.GameObjectSpawn = GenerateGem();
				objectManager.GameObjectSpawn.transform.parent = gameObject.transform;
				objectManager.GameObjectSpawn.transform.localPosition = Vector3.zero;
				objectManager.GameObjectSpawn.SetActive(false);
			}
		}
		else if (props.ContainsKey("lwa:trapBlock"))
		{
			gameObject.name = "TrapBlock";
			gameObject.layer = LayerMask.NameToLayer("TrapBlocks");
			gameObject.tag = "Objects";

			var objectManager = gameObject.AddComponent<ObjectManager>();
			objectManager.Type = ObjectManager.ObjectType.TrapBlock;

			var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = ItemDictonary["trapBlock_frame_0"];
			gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
		}
	}

	/// <summary>
	/// Adds the spines and other colliders to the ladders.
	/// </summary>
	/// <param name="prefab">The prefab of the level.</param>
	public void CustomizePrefab(GameObject prefab)
	{
	}

	public void LoadImages()
	{
		Sprite[] spriteList = AssetDatabase.LoadAllAssetsAtPath("Assets/Sprites/Items/items_objects_32x32.png").OfType<Sprite>().ToArray();

		for (int i = 0; i < spriteList.Length; i++)
		{
			if (!ItemDictonary.ContainsKey(spriteList[i].name))
			{
				ItemDictonary.Add(spriteList[i].name, spriteList[i]);
			}
		}
	}

	public void LoadKeys(IDictionary<string, string> props)
	{
		if (props.ContainsKey("lwa:keyID") && !KeyUUIDs.ContainsKey(Convert.ToInt32(props["lwa:keyID"])))
		{
			KeyUUIDs.Add(Convert.ToInt32(props["lwa:keyID"]), Guid.NewGuid().ToString());
		}
	}

	public GameObject GenerateKey(string keyID)
	{
		GameObject tempGameObject = new GameObject();

		tempGameObject.name = "Key";
		tempGameObject.layer = LayerMask.NameToLayer("Items");
		tempGameObject.tag = "Items";

		var itemManager = tempGameObject.AddComponent<ItemManager>();
		itemManager.ItemType = ItemManager._ItemType.Key;
		itemManager.KeyID = Convert.ToInt32(keyID);
		itemManager.Identifier = KeyUUIDs[Convert.ToInt32(keyID)];

		var spriteRenderer = tempGameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = ItemDictonary["key_frame_0"];
		spriteRenderer.sortingLayerName = "Items";
		spriteRenderer.sortingOrder = 0;

		var animator = tempGameObject.AddComponent<Animator>();
		animator.runtimeAnimatorController = KeyAnimator;

		return tempGameObject;
	}

	public GameObject GenerateCoin1()
	{
		GameObject tempGameObject = new GameObject();

		tempGameObject.name = "Coin1";
		tempGameObject.layer = LayerMask.NameToLayer("Items");
		tempGameObject.tag = "Items";

		var itemManager = tempGameObject.AddComponent<ItemManager>();
		itemManager.ItemType = ItemManager._ItemType.Coin1;

		var spriteRenderer = tempGameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = ItemDictonary["coin1_frame_0"];
		spriteRenderer.sortingLayerName = "Items";
		spriteRenderer.sortingOrder = 1;

		var animator = tempGameObject.AddComponent<Animator>();
		animator.runtimeAnimatorController = Coin1Animator;

		return tempGameObject;
	}


	public GameObject GenerateCoin5()
	{
		GameObject tempGameObject = new GameObject();

		tempGameObject.name = "Coin5";
		tempGameObject.layer = LayerMask.NameToLayer("Items");
		tempGameObject.tag = "Items";

		var itemManager = tempGameObject.AddComponent<ItemManager>();
		itemManager.ItemType = ItemManager._ItemType.Coin5;

		var spriteRenderer = tempGameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = ItemDictonary["coin5_frame_0"];
		spriteRenderer.sortingLayerName = "Items";
		spriteRenderer.sortingOrder = 2;

		var animator = tempGameObject.AddComponent<Animator>();
		animator.runtimeAnimatorController = Coin5Animator;

		return tempGameObject;
	}

	public GameObject GenerateCoin10()
	{
		GameObject tempGameObject = new GameObject();

		tempGameObject.name = "Coin10";
		tempGameObject.layer = LayerMask.NameToLayer("Items");
		tempGameObject.tag = "Items";

		var itemManager = tempGameObject.AddComponent<ItemManager>();
		itemManager.ItemType = ItemManager._ItemType.Coin10;

		var spriteRenderer = tempGameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = ItemDictonary["coin10_frame_0"];
		spriteRenderer.sortingLayerName = "Items";
		spriteRenderer.sortingOrder = 3;

		var animator = tempGameObject.AddComponent<Animator>();
		animator.runtimeAnimatorController = Coin10Animator;

		return tempGameObject;
	}

	public GameObject GenerateGem()
	{
		GameObject tempGameObject = new GameObject();

		tempGameObject.name = "Gem";
		tempGameObject.layer = LayerMask.NameToLayer("Items");
		tempGameObject.tag = "Items";

		var itemManager = tempGameObject.AddComponent<ItemManager>();
		itemManager.ItemType = ItemManager._ItemType.Gem;

		var spriteRenderer = tempGameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = ItemDictonary["gem_frame_0"];
		spriteRenderer.sortingLayerName = "Items";
		spriteRenderer.sortingOrder = 4;

		var animator = tempGameObject.AddComponent<Animator>();
		animator.runtimeAnimatorController = GemAnimator;

		return tempGameObject;
	}
}