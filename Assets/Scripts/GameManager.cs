using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public bool isDebugActive;
	public UnityEngine.UI.Text debugText;
	public GameObject level;

	[HideInInspector]
	public Rect levelBoundries;

	// Use this for initialization
	void Start()
    {
		debugText.gameObject.SetActive(isDebugActive);
		levelBoundries = new Rect();
		levelBoundries.xMin = 0;
		levelBoundries.xMax = level.GetComponentInChildren<Collider2D>().bounds.size.x;

		if (level.GetComponentInChildren<Collider2D>().bounds.max.y > 0)
		{
			levelBoundries.yMax = level.GetComponentInChildren<Collider2D>().bounds.max.y;
		}
		else
		{
			levelBoundries.yMax = 0;
		}

		if (level.GetComponentInChildren<Collider2D>().bounds.min.y < -10)
		{
			levelBoundries.yMin = level.GetComponentInChildren<Collider2D>().bounds.max.y;
		}
		else
		{
			levelBoundries.yMin = -10;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
	}
}
