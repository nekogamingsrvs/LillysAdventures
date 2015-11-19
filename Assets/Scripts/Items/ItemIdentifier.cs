using UnityEngine;

namespace VoidInc
{
	public class ItemIdentifier : MonoBehaviour
	{
		public enum ItemType
		{
			Coin,
			Gem,
			Key,
		}

		[HideInInspector]
		public int Score;

		[HideInInspector]
		public ItemType itemType;

		[HideInInspector]
		public int Identifier;

		private GameManager gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

		public void RemoveCoin(int layer)
		{
			if (itemType == ItemType.Coin)
			{
				Destroy(gameObject);

				gameController.Score += Score;
			}
		}

		public void RemoveGem(int layer)
		{
			if (itemType == ItemType.Gem)
			{
				Destroy(gameObject);

				gameController.Score += Score;
			}
		}


		public void RemoveKey()
		{
			if (itemType == ItemType.Key)
			{
				Destroy(gameObject);

				gameController.Keys += 1;

				gameController.KeyIds.Add(Identifier);
			}
		}
	}
}
