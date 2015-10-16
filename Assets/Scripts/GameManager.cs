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
		levelBoundries.xMax = 100;
		levelBoundries.yMin = -10;
		levelBoundries.yMax = 0;
	}
	
	// Update is called once per frame
	void Update()
	{
	}
}
