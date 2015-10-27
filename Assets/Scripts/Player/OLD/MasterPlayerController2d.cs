using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
/// <summary>
/// The master player controller for the character. (Requires a PlayerPlatfromController2d and a PlayerLadderController2d).
/// </summary>
[RequireComponent(typeof(PlayerPlatformController2d), typeof(PlayerLadderController2d))]
class MasterPlayerController2d : MonoBehaviour
{
	/// <summary>
	/// Gets or sets the PlayerPlatfromController2d for the master controller.
	/// </summary>
	public PlayerPlatformController2d PlatformController { get; private set; }

	/// <summary>
	/// Gets or sets the PlayerLadderController2d for the master controller.
	/// </summary>
	public PlayerLadderController2d LadderController { get; private set; }

	public PlayerItemController2d ItemController { get; private set; }

	public PlayerLockController2d LockController { get; private set; }

	/// <summary>
	/// Gets or sets the Sprite Animation on a child GameObject.
	/// </summary>
	public GameObject SpriteAnimator { get; private set; }

	/// <summary>
	/// Sets the player to the ladder state from the spine.
	/// </summary>
	/// <param name="ladderSpine">The EdgeCollder2D of the ladder's spine.</param>
	public void GotoLadderState_FromSpine(EdgeCollider2D ladderSpine)
	{
		// Snap to the ladder
		Vector3 pos = this.transform.position;
		float ladder_x = ladderSpine.points[0].x;
		this.transform.position = new Vector3(ladder_x, pos.y, pos.z);

		SwitchFacingDirection();

		// The platform controller is not active while on a ladder, but our ladder controller is
		this.PlatformController.SetPlayerControllerEnabled(false);
		this.LadderController.SetPlayerControllerEnabled(true);
	}

	/// <summary>
	/// Sets the player to the ladder state from the top.
	/// </summary>
	/// <param name="ladderSpine">The EdgeCollder2D of the ladder's top.</param>
	public void GotoLadderState_FromTop(EdgeCollider2D ladderSpine)
	{
		// We need to snap to the ladder plus move down past the ladder top so we don't collide with it
		// (We take for granted that the edge collider points for the spine go from down-to-up)
		Vector3 pos = this.transform.position;
		float ladder_x = ladderSpine.points[0].x;
		float ladder_y = ladderSpine.points[1].y;
		this.transform.position = new Vector3(ladder_x, ladder_y - 2.0f, pos.z);

		SwitchFacingDirection();

		// The platform controller is not active while on a ladder, but our ladder controller is
		this.PlatformController.SetPlayerControllerEnabled(false);
		this.LadderController.SetPlayerControllerEnabled(true);
	}

	/// <summary>
	/// Sets the player to the platform state.
	/// </summary>
	public void GotoPlatformState()
	{
		// Platform controller is now enabled. Other player controllers are disabled.
		this.LadderController.SetPlayerControllerEnabled(false);
		this.PlatformController.SetPlayerControllerEnabled(true);
	}

	/// <summary>
	/// Sets the player to face a specific direction.
	/// </summary>
	/// <param name="dx">The direction to face.</param>
	/// <returns>Boolean</returns>
	public bool FaceDirection(float dx)
	{
		Vector3 scale = this.SpriteAnimator.transform.localScale;

		if (Mathf.Sign(dx) != Mathf.Sign(scale.x))
		{
			this.SpriteAnimator.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
			return true;
		}

		return false;
	}

	/// <summary>
	/// Switches the direction that the player is facing.
	/// </summary>
	public void SwitchFacingDirection()
	{
		Vector3 scale = this.SpriteAnimator.transform.localScale;
		this.SpriteAnimator.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
	}

	// Runs when the scene starts.
	private void Awake()
	{
		this.PlatformController = this.gameObject.GetComponent<PlayerPlatformController2d>();
		this.LadderController = this.gameObject.GetComponent<PlayerLadderController2d>();
		this.ItemController = this.gameObject.GetComponent<PlayerItemController2d>();
		this.LockController = this.gameObject.GetComponent<PlayerLockController2d>();

		this.SpriteAnimator = this.transform.Find("Animator").gameObject;

		// Ladder Controller starts off disabled
		this.LadderController.SetPlayerControllerEnabled(false);
		this.PlatformController.SetPlayerControllerEnabled(true);
		this.ItemController.SetPlayerControllerEnabled(true);
		this.LockController.SetPlayerControllerEnabled(true);
	}
}
*/