using UnityEngine;
using System.Collections;

/// <summary>
/// this script just captures the OnTrigger* messages and passes them on to the CharacterController2D
/// </summary>
public class CC2DTriggerHelper : MonoBehaviour
{
	/// <summary>
	/// The parent Character Controller 2D to use in the trigger helper.
	/// </summary>
	private CharacterController2D _parentCharacterController;

	/// <summary>
	/// Sets the parent Character Controller 2D used in the trigger helper.
	/// </summary>
	/// <param name="parentCharacterController">The parent Character Controller.</param>
	public void setParentCharacterController(CharacterController2D parentCharacterController)
	{
		_parentCharacterController = parentCharacterController;
	}
	
	#region MonoBehavior
	/// <summary>
	/// Gets if the trigger entered a 2D collision.
	/// </summary>
	/// <param name="col">The collider to check.</param>
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.isTrigger)
		{
			_parentCharacterController.OnTriggerEnter2D(col);
		}
	}

	/// <summary>
	/// Gets if the trigger stayed on a 2D collision.
	/// </summary>
	/// <param name="col">The collider to check.</param>
	void OnTriggerStay2D(Collider2D col)
	{
		if (col.isTrigger)
		{
			_parentCharacterController.OnTriggerStay2D(col);
		}
	}

	/// <summary>
	/// Gets if the trigger exited a 2D collision.
	/// </summary>
	/// <param name="col">The collider to check.</param>
	void OnTriggerExit2D(Collider2D col)
	{
		if (col.isTrigger)
			_parentCharacterController.OnTriggerExit2D(col);
	}
	#endregion
}