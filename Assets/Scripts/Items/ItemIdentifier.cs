using UnityEngine;
using System.Collections;

namespace VoidInc
{
	public class ItemIdentifier : MonoBehaviour
	{
		public int Score;

		public void RemoveCoin(int layer)
		{
			if (layer == LayerMask.NameToLayer("Coins"))
			{
				var gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

				Destroy(gameObject);

				gameController.Score += Score;
			}
		}

		public void RemoveGem(int layer)
		{
			if (layer == LayerMask.NameToLayer("Gems"))
			{
				var gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

				Destroy(gameObject);

				gameController.Score += Score;
			}
		}
	}
}
