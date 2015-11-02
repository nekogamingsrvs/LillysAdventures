using UnityEngine;
using System.Collections;

namespace VoidInc
{
	public class LockIdentifier : MonoBehaviour
	{
		public int Identifier;

		public void CheckLock()
		{
			var gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

			if (gameController.Keys >= 1 && gameController.KeyIds.Contains(Identifier))
			{
				Destroy(gameObject);
				gameController.Keys -= 1;
			}
		}
	}
}