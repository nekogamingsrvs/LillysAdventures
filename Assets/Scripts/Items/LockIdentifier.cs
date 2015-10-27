using UnityEngine;
using System.Collections;

namespace VoidInc
{
	public class LockIdentifier : MonoBehaviour
	{
		public void CheckLock()
		{
			var gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

			if (gameController.Keys >= 1)
			{
				Destroy(gameObject);
				gameController.Keys -= 1;
			}
		}
	}
}