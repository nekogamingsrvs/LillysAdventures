using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	/// <summary>
	/// The level of the game.
	/// </summary>
	public GameObject level;

	/// <summary>
	/// Gets or sets if the game is in debug mode.
	/// </summary>
	public bool isDebugActive;

	/// <summary>
	/// The boundries of the map.
	/// </summary>
	[HideInInspector]
	public Rect levelBoundries;

	private Vector2 mouse;

	// Use this for initialization
	void Start()
	{
		// Calulates the level's boundries.
		levelBoundries = new Rect();
		levelBoundries.xMin = 0;
		levelBoundries.xMax = level.GetComponent<Tiled2Unity.TiledMap>().MapWidthInPixels * 2;
		levelBoundries.yMin = -level.GetComponent<Tiled2Unity.TiledMap>().MapHeightInPixels * 2;
		levelBoundries.yMax = 0;
	}
}