using UnityEngine;

public class startGame : MonoBehaviour
{
	public GameObject startPanel;
	public GameObject player;
	public GameObject spawnPoint;

	private bool gameStarted = false;

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
		{
			startPanel.SetActive(false);

			player.transform.position = spawnPoint.transform.position;
			player.SetActive(true);

			gameStarted = true;
		}
	}
}
