using UnityEngine;
using System.Collections;

namespace VoidInc
{
	public class GemIdentifier : MonoBehaviour
	{
		public int GemScore;

		public void RemoveGem()
		{
			var gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

			Destroy(gameObject);

			gameController.Score += GemScore;

			gameController.Gems += 1;
		}
	}
}