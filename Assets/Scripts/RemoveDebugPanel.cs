using UnityEngine;
using System.Collections;

public class RemoveDebugPanel : MonoBehaviour
{
	public UnityEngine.UI.Text debugText;
	
	// Update is called once per frame
	void Update()
	{
		gameObject.SetActive(debugText.gameObject.activeInHierarchy);
	}
}
