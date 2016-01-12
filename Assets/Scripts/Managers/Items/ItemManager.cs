using UnityEngine;

namespace VoidInc
{
	public class ItemManager : MonoBehaviour
	{
		/// <summary>
		/// The item type enum to determine the item function.
		/// </summary>
		public enum ItemType
		{
			Coin1,
			Coin5,
			Coin10,
			Gem,
			Key
		}
		/// <summary>
		/// If the item has been destroyed.
		/// </summary>
		public bool Destroyed;
		/// <summary>
		/// What type of item is the item.
		/// </summary>
		public ItemType Type;
		/// <summary>
		/// The identifier for keys to open locks and for locks to be opened by keys.
		/// </summary>
		public string Identifier;
		/// <summary>
		/// The key id to check the identifier with.
		/// </summary>
		public int KeyID;
		/// <summary>
		/// The GameManager class for the items.
		/// </summary>
		[HideInInspector]
		private GameManager _GameManager;

		void Awake()
		{
			_GameManager = FindObjectOfType<GameManager>();
		}

		/// <summary>
		/// Removes the item from the world.
		/// </summary>
		void RemoveItem()
		{
			switch (Type)
			{
				case ItemType.Coin1:
					RemoveCoin(100);
					break;
				case ItemType.Coin5:
					RemoveCoin(500);
					break;
				case ItemType.Coin10:
					RemoveCoin(1000);
					break;
				case ItemType.Gem:
					RemoveGem(8000);
					break;
				case ItemType.Key:
					RemoveKey();
					break;
				default:
					break;
			}
		}

		private void RemoveCoin(int score)
		{
			_GameManager.GameDataManager.DestroyedGameObjects.Add(gameObject.GetInstanceID());
			Destroy(gameObject);
			_GameManager.GameDataManager.Score += score;
		}

		private void RemoveGem(int score)
		{
			_GameManager.GameDataManager.DestroyedGameObjects.Add(gameObject.GetInstanceID());
			Destroy(gameObject);
			_GameManager.GameDataManager.Score += score;
			_GameManager.Gems += 1;
			_GameManager.GameDataManager.TotalGems += 1;
		}
		
		private void RemoveKey()
		{
			_GameManager.GameDataManager.DestroyedGameObjects.Add(gameObject.GetInstanceID());
			Destroy(gameObject);
			_GameManager.GameDataManager.Keys += 1;
			_GameManager.GameDataManager.KeyIdentifiers.Add(KeyID, Identifier);
		}
	}
}
