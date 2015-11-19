using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace VoidInc
{
	/// <summary>
	/// Manages debug labels.
	/// </summary>
	public class DebugLabelManager : MonoBehaviour
	{
		/// <summary>
		/// The debug string struct.
		/// </summary>
		protected class DebugString
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
			UpdateStrings();

			GetComponentInChildren<UnityEngine.UI.Text>().text = debugText;
		}

		void AddStrings()
		{
			foreach (DebugString debugString in _DebugStrings)
			{
				// Adds debug string to text box.
				debugText += FormatString(debugString);
			}
		}

		void UpdateStrings()
		{
			foreach (DebugString debugString in _DebugStrings)
			{
				ReplaceStringFormat(debugString);
			}
		}

		string FormatString(DebugString debugString)
		{
			return string.Format("{0}: {1} | ", debugString.id, debugString.variable.ToString());
		}

		void ReplaceStringFormat(DebugString debugString)
		{
			Regex tempRegex = new Regex(@"\" + debugString.id + @"\:\ ([A-Za-z0-9\-\.]+) \| ");

			debugText = tempRegex.Replace(debugText, FormatString(debugString));
		}

		/// <summary>
		/// Add a variable and it's parameter to the database of debug strings.
		/// </summary>
		/// <param name="id">The variable's name.</param>
		/// <param name="variable">The value or the variable it's self.</param>
		public void AddToDatabase(string id, object variable)
		{
			DebugString tempDebugString = new DebugString();
			tempDebugString.id = id;
			tempDebugString.variable = variable;

			_DebugStrings.Add(tempDebugString);
		}

		public void UpdateToDatabase(string id, object variable)
		{
			_DebugStrings.Find(x => x.id.Contains(id)).variable = variable;
		}
	}
}