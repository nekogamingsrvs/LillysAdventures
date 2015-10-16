using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public bool isDebugActive;
	public UnityEngine.UI.Image debugTextPanel;
	public GameObject level;

	[HideInInspector]
	public Rect levelBoundries;

	// Use this for initialization
	void Start()
	{
		debugTextPanel.gameObject.SetActive(isDebugActive);
		levelBoundries = new Rect();
		levelBoundries.xMin = 0;
		levelBoundries.xMax = level.GetComponent<Tiled2Unity.TiledMap>().MapWidthInPixels;
		levelBoundries.yMin = -level.GetComponent<Tiled2Unity.TiledMap>().MapHeightInPixels;
		levelBoundries.yMax = 0;
	}

	// Update is called once per frame
	void Update()
	{
	}
}
