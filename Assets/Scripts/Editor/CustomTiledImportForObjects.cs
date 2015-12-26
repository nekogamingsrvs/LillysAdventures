using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using VoidInc;

[JsonObject("liquid")]
public class LWALiquidParameters
{
	[JsonProperty("type")]
	string type;
}

[JsonObject("lock")]
public class LWALockParameters
{
	[JsonProperty("keyID")]
	string keyID;
}

[JsonObject("sign")]
public class LWASignParameters
{
	[JsonProperty("type")]
	string type;

	[JsonProperty("dialog")]
	string dialog;

	[JsonProperty("skin")]
	int skin;
}

[JsonObject("itemBlock")]
public class LWAItemBlockParameters
{
	[JsonProperty("item")]
	string type;
}

[JsonObject("trapBlock")]
public class LWATrapBlockParameters
{
	[JsonProperty("type")]
	string type;

	[JsonProperty("direction")]
	string direction;
}

[JsonObject("objectBlock")]
public class LWAObjectBlockParameters
{
	[JsonProperty("liquid")]
	LWALiquidParameters liquidParameters;

	[JsonProperty("lock")]
	LWALockParameters lockParameters;

	[JsonProperty("sign")]
	LWASignParameters signParameters;

	[JsonProperty("itemBlock")]
	LWAItemBlockParameters itemBlockParameters;

	[JsonProperty("trapBlock")]
	LWATrapBlockParameters trapBlockParameters;

	[JsonProperty("objectBlock")]
	LWAObjectBlockParameters objectBlockParameters;
}

[JsonObject()]
public class LWAObject
{
	[JsonProperty("object")]
	string type;

	[JsonProperty("lock")]
	LWALockParameters lockParameters;

	[JsonProperty("sign")]
	LWASignParameters signParameters;

	[JsonProperty("itemBlock")]
	LWAItemBlockParameters itemBlockParameters;

	[JsonProperty("trapBlock")]
	LWATrapBlockParameters trapBlockParameters;

	[JsonProperty("objectBlock")]
	LWAObjectBlockParameters objectBlockParameters;
}

/// <summary>
/// Imports the coins' controller objects for Coins1
/// </summary>
[Tiled2Unity.CustomTiledImporter]
public class CustomTileImportForObjects : MonoBehaviour, Tiled2Unity.ICustomTiledImporter
{
	//Dictionary<int, string> KeyUUIDs = new Dictionary<int, string>();

	GameObject Torch;
	GameObject Lock;
	GameObject Sign;
	GameObject ItemBlock;
	GameObject ObjectBlock;
	GameObject TrapBlock;

	GameObject Key;
	GameObject Coin1;
	GameObject Coin5;
	GameObject Coin10;
	GameObject Gem;

	/// <summary>
	/// Handles custom properties, does nothing.
	/// </summary>
	/// <param name="gameObject">The game object to change</param>
	/// <param name="props">The properties to change</param>
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
	{
		//KeyUUIDs = CustomTileImportForItems.KeyUUIDs;

		Torch = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Torch");
		Lock = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Lock");
		Sign = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Sign");
		ItemBlock = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/ItemBlock");
		ObjectBlock = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/ObjectBlock");
		TrapBlock = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/TrapBlock");

		Key = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Key");
		Coin1 = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Coin1");
		Coin5 = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Coin5");
		Coin10 = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Coin10");
		Gem = AssetDatabase.LoadAssetAtPath<GameObject>("/Prefabs/Gem");

		if (props.ContainsKey("lwa:object"))
		{
			var collider = gameObject.GetComponent<Collider2D>();
			collider.isTrigger = true;

			if (props["lwa:object"] == "torch")
			{
				GameObject tempGameObject = (GameObject)Resources.Load("Torch");
				tempGameObject.transform.position = gameObject.transform.position;
			}
			else if (props["lwa:object"] == "lock")
			{
				GameObject tempGameObject = (GameObject)Resources.Load("Lock");
				tempGameObject.GetComponent<ObjectManager>().KeyID = Convert.ToInt32(props["lwa:keyID"]);
				tempGameObject.GetComponent<ObjectManager>().Identifier = props["lwa:keyID"]; //KeyUUIDs[Convert.ToInt32(props["lwa:keyID"])];
				tempGameObject.transform.position = gameObject.transform.position;
			}
			else if (props["lwa:object"] == "sign")
			{
				GameObject tempGameObject = (GameObject)Resources.Load("Sign");
				if (props["lwa:sign"] == "save")
				{
					tempGameObject.GetComponent<ObjectManager>()._SignType.SignType = ObjectManager._Sign._SignType.save;
				}
				else if (props["lwa:sign"] == "heal")
				{
					tempGameObject.GetComponent<ObjectManager>()._SignType.SignType = ObjectManager._Sign._SignType.heal;
				}
				else if (props["lwa:sign"] == "dialog")
				{
					tempGameObject.GetComponent<ObjectManager>()._SignType.SignType = ObjectManager._Sign._SignType.dialog;
					tempGameObject.GetComponent<ObjectManager>()._SignType.Dialog = props["lwa:signDialog"];
					tempGameObject.GetComponent<Animator>().SetInteger("SignType", Convert.ToInt32(props["lwa:signType"]));
				}
				tempGameObject.transform.position = gameObject.transform.position;
			}
			else if (props["lwa:object"] == "itemBlock")
			{
				GameObject tempGameObject = (GameObject)Resources.Load("ItemBlock");
				if (props["lwa:itemBlock"] == "key")
				{
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load(props["lwa:itemBlock"]);
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ItemManager>().KeyID = Convert.ToInt32(props["lwa:keyID"]);
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ItemManager>().Identifier = props["lwa:keyID"]; //KeyUUIDs[Convert.ToInt32(props["lwa:keyID"])];
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.transform.parent = tempGameObject.transform;
				}
				else if (props["lwa:itemBlock"] == "coin1")
				{
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Coin1");
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.transform.parent = tempGameObject.transform;
				}
				else if (props["lwa:itemBlock"] == "coin5")
				{
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Coin5");
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.transform.parent = tempGameObject.transform;
				}
				else if (props["lwa:itemBlock"] == "coin10")
				{
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Coin10");
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.transform.parent = tempGameObject.transform;
				}
				else if (props["lwa:itemBlock"] == "gem")
				{
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Gem");
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.transform.parent = tempGameObject.transform;
				}
				tempGameObject.transform.position = gameObject.transform.position;
			}
			else if (props["lwa:object"] == "objectBlock")
			{
				GameObject tempGameObject = (GameObject)Resources.Load("ObjectBlock");
				if (props["lwa:objectBlock"] == "torch")
				{
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Torch");
				}
				else if (props["lwa:objectBlock"] == "sign")
				{
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Sign");
					if (props["lwa:sign"] == "save")
					{
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>()._SignType.SignType = ObjectManager._Sign._SignType.save;
					}
					else if (props["lwa:sign"] == "heal")
					{
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>()._SignType.SignType = ObjectManager._Sign._SignType.heal;
					}
					else if (props["lwa:sign"] == "dialog")
					{
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>()._SignType.SignType = ObjectManager._Sign._SignType.dialog;
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>()._SignType.Dialog = props["lwa:signDialog"];
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<Animator>().SetInteger("SignType", Convert.ToInt32(props["lwa:signType"]));
					}
				}
				else if (props["lwa:objectBlock"] == "itemBlock")
				{
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("ItemBlock");
					if (props["lwa:itemBlock"] == "key")
					{
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Key");
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ItemManager>().KeyID = Convert.ToInt32(props["lwa:keyID"]);
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ItemManager>().Identifier = props["lwa:keyID"]; //KeyUUIDs[Convert.ToInt32(props["lwa:keyID"])];
					}
					else if (props["lwa:itemBlock"] == "coin1")
					{
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Coin1");
					}
					else if (props["lwa:itemBlock"] == "coin5")
					{
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Coin5");
					}
					else if (props["lwa:itemBlock"] == "coin10")
					{
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Coin10");
					}
					else if (props["lwa:itemBlock"] == "gem")
					{
						tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("Gem");
					}
				}
				else if (props["lwa:objectBlock"] == "trapBlock")
				{
					tempGameObject.GetComponent<ObjectManager>().GameObjectSpawn = (GameObject)Resources.Load("TrapBlock");
				}
				tempGameObject.transform.parent = gameObject.transform;
			}
			else if (props["lwa:object"] == "trapBlock")
			{
				GameObject tempGameObject = (GameObject)Resources.Load("TrapBlock");
				tempGameObject.transform.parent = gameObject.transform;
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

	public void PhraseParameters(string json)
	{
		JObject jsonData = JObject.Parse(json);

		JToken objectType = "";

		if (jsonData.TryGetValue("/object", out objectType))
		{

		}
	}
}