using UnityEngine;
using System.Collections;

public class CoinIdentifier : MonoBehaviour
{
	public int CoinScore;

	public void RemoveCoin()
	{
		var gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

		Destroy(gameObject);

		gameController.Score += CoinScore;
	}
}
