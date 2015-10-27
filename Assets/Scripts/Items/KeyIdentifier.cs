using UnityEngine;
using System.Collections;

namespace VoidInc
{
	public class KeyIdentifier : MonoBehaviour
	{
		public void RemoveKey()
		{
			var gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

			Destroy(gameObject);

			gameController.Keys += 1;
		}
	}
}