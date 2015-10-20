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
	private List<DebugString> _DebugStrings = new List<DebugString>();
	
	private string debugText = "";

	// Runs when the GUI is active.
	void Start()
	{
		// Set the debug variable to not active.
		gameObject.SetActive(FindObjectOfType<GameManager>().isDebugActive);
		
		AddStrings();
	}

	// Update is called once per frame
	void Update()
	{
		UpdateString();
		
		GetComponentInChildren<UnityEngine.UI.Text>().text = debugText;
	}
	
	void AddString()
	{
		foreach (DebugString debugString in _DebugStrings)
		{
			// Adds debug string to text box.
			debugText += FormatString(debugString);
        }
	}
	
	void UpdateString()
	{
		tempStringText = "";
		
		foreach (DebugString debugString in _DebugStrings)
		{
			debugText.Replace(FindStringFormat(debugString), FormatString(debugString));
		}
	}
	
	string FormatString(DebugString debugString)
	{
		return string.Format("{0}: {1} | ", debugString.id, debugString.variable.ToString());
	}
	
	string FindStringFormat(DebugString)
	{
		return string.Format("{0}\:\ ([A-Za-z0-9\-]+) \| ", debugString.id);
	}

	/// <summary>
	/// Add a variable and it's parameter to the database of debug strings.
	/// </summary>
	/// <param name="id">The variable's name.</param>
	/// <param name="variable">The value or the variable it's self.</param>
	public void AddToDatabase(string id, object variable)
	{
		DebugString TempDebugString = new DebugString();
		TempDebugString.id = id;
		TempDebugString.variable = variable;

		_DebugStrings.Add(tempDebugString);
	}
}