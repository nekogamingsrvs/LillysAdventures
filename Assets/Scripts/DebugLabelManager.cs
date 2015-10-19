using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages debug labels.
/// </summary>
public class DebugLabelManager : MonoBehaviour
{
	/// <summary>
	/// The debug string struct.
	/// </summary>
	protected struct DebugString
	{
		public string id;
		public object variable;
	}

	/// <summary>
	/// The list of debug strings.
	/// </summary>
	private List<DebugString> debugStrings = new List<DebugString>();

	// Runs when the gui is active.
	void OnGUI()
	{
		// Set the debug variable to not active.
		gameObject.SetActive(FindObjectOfType<GameManager>().isDebugActive);
	}

	// Update is called once per frame
	void Update()
	{
		foreach (DebugString ds in debugStrings)
		{
			// Adds debug string to text box.
			GetComponentInChildren<UnityEngine.UI.Text>().text += string.Format("{0}: {1} | ", ds.id, ds.variable.ToString());
        }
	}

	/// <summary>
	/// Add a variable and it's parameter to the database of debug strings.
	/// </summary>
	/// <param name="id">The variable's name.</param>
	/// <param name="variable">The value or the variable it's self.</param>
	public void addToDatabase(string id, object variable)
	{
		DebugString tempDebugString = new DebugString();
		tempDebugString.id = id;
		tempDebugString.variable = variable;

		debugStrings.Add(tempDebugString);
	}
}