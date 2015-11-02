using UnityEngine;
using System.Collections;

namespace VoidInc
{
	public class KeyIdentifier : MonoBehaviour
	{
		public int Identifier;

		public void RemoveKey()
		{
			var gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

			Destroy(gameObject);

			gameController.Keys += 1;

			gameController.KeyIds.Add(Identifier);
		}
	}
}