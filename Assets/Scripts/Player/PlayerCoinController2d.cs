using UnityEngine;
using System.Collections;

public class PlayerCoinController2d : MonoBehaviour
{
	// Use this for initialization
	void Update()
	{
		var coinObjects = GameObject.FindObjectsOfType<CoinIdentifier>();
		foreach (var coinObject in coinObjects)
		{
			if (gameObject.GetComponent<BoxCollider2D>().IsTouching(coinObject.gameObject.GetComponent<BoxCollider2D>()))
            {
				coinObject.RemoveCoin();
			}
        }
	}
}
