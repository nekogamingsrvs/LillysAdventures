using UnityEngine;

namespace VoidInc
{
	public class ItemIdentifier : MonoBehaviour
	{
		/// <summary>
		/// The item type enum to determine the item function.
		/// </summary>
		public enum _ItemType
		{
			Coin,
			Gem,
			Key,
			Lock
		}
		
		/// <summary>
		/// The score that the item will add to the player.
		/// </summary>
		[HideInInspector]
		public int Score;

		/// <summary>
		/// If the item has been destroyed.
		/// </summary>
		[HideInInspector]
		public bool Destroyed;

		/// <summary>
		/// What type of item is the item.
		/// </summary>
		public _ItemType ItemType;

		/// <summary>
		/// The identifier for keys to open locks and for locks to be opened by keys.
		/// </summary>
		public string Identifier;

		/// <summary>
		/// The GameManager class for the items.
		/// </summary>
		[HideInInspector]
		private GameManager gameController;

		void Awake()
		{
			gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }

		public void RemoveItem()
		{
			switch (ItemType)
			{
				case _ItemType.Coin:
					RemoveCoin();
					break;
				case _ItemType.Gem:
					RemoveGem();
					break;
				case _ItemType.Key:
					RemoveKey();
					break;
				case _ItemType.Lock:
					CheckLock();
					break;
				default:
					break;
			}
		}

		private void RemoveCoin()
		{
			gameController.DestroyedGameObjects.Add(gameObject.name);
			Destroy(gameObject);
			gameController.Score += Score;
		}

		private void RemoveGem()
		{
			gameController.DestroyedGameObjects.Add(gameObject.name);
			Destroy(gameObject);
			gameController.Score += Score;
			gameController.Gems += 1;
			gameController.TotalGems += 1;
		}


		private void RemoveKey()
		{
			gameController.DestroyedGameObjects.Add(gameObject.name);
			Destroy(gameObject);
			gameController.Keys += 1;
			gameController.KeyIdentifiers.Add(Identifier);
		}

		private void CheckLock()
		{
			if (gameController.Keys >= 1 && gameController.KeyIdentifiers.Contains(Identifier))
			{
				gameController.DestroyedGameObjects.Add(gameObject.name);
				Destroy(gameObject);
				gameController.Keys -= 1;
				gameController.KeyIdentifiers.Remove(Identifier);
			}
		}
	}
}
