using UnityEngine;
using System.Collections;

namespace VoidInc
{
	public class DoorIdentifier : MonoBehaviour
	{
		public bool IsDoorway;
		public string LeadsTo;
		public bool RequiresGems;
		public int AmountGems;

		public void OpenDoor()
		{
			if (IsDoorway)
			{
				if (RequiresGems && GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Gems == AmountGems)
				{
					//GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().CurrentLevel = LeadsTo;
				}
				else
				{
					//GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().CurrentLevel = LeadsTo;
				}
			}
			else
			{
				return;
			}
		}
	}
}