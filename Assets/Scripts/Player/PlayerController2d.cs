using CnControls;
using Prime31;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VoidInc
{
	public class PlayerController2D : MonoBehaviour
	{
		public float RunMultiplierSpeed = 2.0f;

		public static readonly float Gravity = -0.25f * 60.0f * 60.0f;
		public static readonly float WalkSpeed = 1.375f * 60.0f;
		public static readonly float InAirDamping = 1.3125f * 60.0f;
		public static readonly float JumpHeight = 4.875f * 64.0f;

		private static readonly float GravityModifierInWater = 0.375f;

		public static readonly float AbsorbGroundedInputTime = 0.0675f;
		public static readonly float LadderVelocityPerSecond = 1.0f * 60.0f;

		[HideInInspector]
		private float NormalizedHorizontalSpeed = 0.0f;

		private CharacterController2D _Controller;
		private Animator _Animator;
		private Vector3 _Velocity;
		private bool _IsRunning;
		private bool _IsClimbing;
		private float _Climb;
		
		private Transform _Linecast1
		{
			get;
			set;
		}
		private Transform _Linecast2
		{
			get;
			set;
		}

		// Use this for initialization
		void Awake()
		{
			// Set the animator and controller components.
			_Animator = GetComponent<Animator>();
			_Controller = GetComponent<CharacterController2D>();

			// listen to some events for illustration purposes
			_Controller.onControllerCollidedEvent += onControllerCollider;
			_Controller.onTriggerEnterEvent += onTriggerEnterEvent;
			_Controller.onTriggerExitEvent += onTriggerExitEvent;

			_Linecast1 = transform.Find("LadderLinecast1");
			_Linecast2 = transform.Find("LadderLinecast2");

			if (GameObject.FindObjectOfType<GameManager>().isDebugActive)
			{
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("VelocityX", _Velocity.x);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("VelocityY", _Velocity.y);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("IsRunning", _IsRunning);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("IsGrounded", _Controller.isGrounded);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("IsClimbing", _IsClimbing);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("Linecast1", _Linecast1 != null);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("Linecast2", _Linecast2 != null);
			}
		}

		#region Event Listeners
		void onControllerCollider(RaycastHit2D hit)
		{
			// bail out on plain old ground hits cause they aren't very interesting
			if (hit.normal.y == 1f)
			{
				return;
			}

			if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ladders"))
			{
				GotoPlatformState();
			}

			// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
			//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
		}
		
		// Update when the trigger enters the event.
		void onTriggerEnterEvent(Collider2D col)
		{
			// If the trigger collided with an object with the tag "Coins1" then remove the coin and add score.
			col.gameObject.SendMessage("RemoveCoin", SendMessageOptions.DontRequireReceiver);
			// If the trigger collided with an object with the tag "Coins5" then remove the coin and add score.
			col.gameObject.SendMessage("RemoveGem", SendMessageOptions.DontRequireReceiver);
			// If the trigger collided with an object with the tag "Keys" then remove the key and add to the key amount.
			col.gameObject.SendMessage("RemoveKey", SendMessageOptions.DontRequireReceiver);
			// If the trigger collided with an object with the tag "Locks" then check if we have a key and unlock it if we do.
			col.gameObject.SendMessage("CheckLock", SendMessageOptions.DontRequireReceiver);
			// If the trigger collided with an object with the tag "Signs" then display the information about the sign.
			col.gameObject.SendMessage("DisplayDialog", SendMessageOptions.DontRequireReceiver);

			if (GameObject.Find("GameManager").GetComponent<GameManager>().isDebugActive)
			{
				Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
			}
		}

		// Update when the trigger exits the event.
		void onTriggerExitEvent(Collider2D col)
		{
			if (GameObject.Find("GameManager").GetComponent<GameManager>().isDebugActive)
			{
				Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
			}
		}
		#endregion

		// Update is called once per frame
		void Update()
		{
			#region Get or set input values
			// Get if the CharacterController2D is grounded and set the velocity to 0.
			if (_Controller.isGrounded)
			{
				_Velocity.y = 0;
				_Animator.SetBool("Grounded", true);
			}
			else
			{
				_Animator.SetBool("Grounded", false);
			}

			// Get if the CharacterController2D velocity is greater than 0 so that we can play the jumping animation.
			if (_Controller.velocity.y > 0)
			{
				_Animator.SetBool("Jumping", true);
			}
			else
			{
				_Animator.SetBool("Jumping", false);
			}

			if (_Controller.isGrounded && (Input.GetButton("Meow") || CnInputManager.GetButton("Meow")))
			{
				gameObject.GetComponent<AudioSource>().Play();

				_Animator.SetTrigger("Meowing");
			}

			if (LadderCheck())
			{
				_IsClimbing = true;
			}

			if (InputControlsManager.TestInput(GameManager.PCPlatforms))
			{
				if (Input.GetAxis("Run") == 0)
				{
					_IsRunning = false;
				}
				else if (Input.GetAxis("Run") == 1)
				{
					_IsRunning = true;
				}
				else
				{
					_IsRunning = Input.GetButton("Run");
				}
			}

			if (InputControlsManager.TestInput(GameManager.MobilePlatforms) && CnInputManager.GetButtonUp("Run"))
			{
				_IsRunning = !_IsRunning;
			}

			_Animator.SetBool("Running", _IsRunning);

			if (InputControlsManager.TestInput(GameManager.PCPlatforms))
			{
				NormalizedHorizontalSpeed = Input.GetAxis("Horizontal");
			}
			else if (InputControlsManager.TestInput(GameManager.MobilePlatforms))
			{
				NormalizedHorizontalSpeed = CnInputManager.GetAxis("Horizontal");
			}

			_Animator.SetFloat("Speed", Mathf.Abs(NormalizedHorizontalSpeed));

			if (_IsRunning)
			{
				NormalizedHorizontalSpeed *= RunMultiplierSpeed;
			}

			if (_Controller.isGrounded && (Input.GetButton("Jump") || CnInputManager.GetButton("Jump")))
			{
				// Jumping
				_Velocity.y = JumpHeight;

				// Raise our MegaDad dude up a bit to start the jump off
				// Otherwise, his leg starts off a bit too deep into the ground
				this.transform.Translate(0, 4, 0);
			}

			if (NormalizedHorizontalSpeed == 0)
			{
				_Animator.SetFloat("Speed", 0);
			}

			// Flip the character based off of his speed.
			if (NormalizedHorizontalSpeed > 0)
			{
				if (transform.localScale.x < 0f)
				{
					transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
				}
			}
			else if (NormalizedHorizontalSpeed < 0)
			{
				if (transform.localScale.x > 0f)
				{
					transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
				}
			}
			#endregion

			#region Ladder Controls
			if (_IsClimbing)
			{
				// We only animate the player while he is moving up/down the ladder
				float climbing = Input.GetAxisRaw("Vertical");

				// Are we jumping? If so, we leave this state.
				if (climbing == 0 && Input.GetButtonDown("Jump"))
				{
					GotoPlatformState();
					return;
				}

				// If we linecast across a ladder top than we are either climbing off of or onto a ladder (bending over a ladder)
				RaycastHit2D ladderTopRay = Physics2D.Linecast(_Linecast1.position, _Linecast2.position, 1 << LayerMask.NameToLayer("LadderTops"));

				// If we're moving up a ladder and our "top half" no longer crosses a ladder top then we climb off the ladder
				if (ladderTopRay && climbing > 0)
				{
					if (!Physics2D.Linecast(_Linecast1.position, this.transform.position, 1 << LayerMask.NameToLayer("LadderTops")))
					{
						// To "pop off" the ladder we have to abruptly move to our "foot" 
						// The bottom of our linecast is made to represent our foot position
						// And our foot needs to rest on the ladder top we're crossing.
						Vector3 position = this.transform.position;
						float feet_y = _Linecast2.transform.position.y;
						float ladderTop_y = ladderTopRay.point.y;

						position.y = ladderTop_y + (position.y - feet_y);
						this.transform.position = position;

						// We may be just above the ladder top and we don't want to fall for a split second
						// So, move our character controller back down to the ground. He should collide with the ladder top we just popped past.
						_Controller.move(new Vector3(0, -0.25f, 0));

						// Go back to the platform state
						GotoPlatformState();
					}
				}

				// If we're moving down a ladder then it's possible to fall off it (because it comes to an end)
				if (climbing < 0)
				{
					// Check if the "top half" of our player is crossing a "LadderBottom"
					RaycastHit2D ladderBottomRay = Physics2D.Linecast(_Linecast1.position, this.transform.position, 1 << LayerMask.NameToLayer("LadderBottoms"));

					if (ladderBottomRay)
					{
						GotoPlatformState();
					}
				}

				// Move our character up/down the ladder
				// Note: There is no graivty applied while on the ladder
				if (climbing != 0)
				{
					Vector2 velocity = new Vector2(0, Mathf.Sign(climbing) * LadderVelocityPerSecond * Time.deltaTime);
					_Controller.move(velocity);
				}
			}
			#endregion

			#region Movement Controls
			float modifier = _Controller.isGrounded ? WalkSpeed : InAirDamping;

			_Velocity.x = NormalizedHorizontalSpeed * modifier;

			// Apply gravity before moving.
			if (!_IsClimbing)
				_Velocity.y += Gravity * Time.deltaTime * 1.0f;

			// Move the controller at the velocity and fluctuate it with time.
			_Controller.move(_Velocity * Time.deltaTime);

			// grab our current _velocity to use as a base for all calculations
			_Velocity = _Controller.velocity;
			#endregion

			if (GameObject.FindObjectOfType<GameManager>().isDebugActive)
			{
				//GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("VelocityX", _Velocity.x);
				//GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("VelocityY", _Velocity.y);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("IsRunning", _IsRunning);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("IsGrounded", _Controller.isGrounded);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("IsClimbing", _IsClimbing);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("Linecast1", _Linecast1 != null);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("Linecast2", _Linecast2 != null);
			}
		}

		void LateUpdate()
		{
			//_LadderSpineCollider = null;
			//_LadderTopCollider = null;
		}

		private bool LadderCheck()
		{
			float climbing = InputControlsManager.TestInput(GameManager.MobilePlatforms) ? CnInputManager.GetAxis("Vertical") : Input.GetAxisRaw("Vertical");

			if (climbing != 0)
			{
				EdgeCollider2D ladderGrabEdgeSpine = transform.FindChild("LadderGrabTest").GetComponent<TriggerTest>().GetTriggerCollider<EdgeCollider2D>();
				EdgeCollider2D ladderDownEdgeSpine = transform.FindChild("LadderDownTest").GetComponent<TriggerTest>().GetTriggerCollider<EdgeCollider2D>();

				if (climbing > 0)
				{
					if (ladderGrabEdgeSpine != null)
					{
						GotoLadderState_FromSpine(ladderGrabEdgeSpine);
						return true;
					}
				}
				else if (climbing < 0)
				{
					// If there is already a ladder for us to grab onto without looking past a ladder top, than grab it
					// (In that case, we require the same ladder to be below us too)
					if (ladderGrabEdgeSpine != null && ladderDownEdgeSpine != null)
					{
						GotoLadderState_FromSpine(ladderGrabEdgeSpine);
						return true;
					}

					// Try to climb down a ladder (past a ladder top edge)
					if (ladderDownEdgeSpine != null)
					{
						GotoLadderState_FromTop(ladderDownEdgeSpine);
						return true;
					}
				}
			}
			return false;
		}

		public void GotoLadderState_FromSpine(EdgeCollider2D ladderSpine)
		{
			// Snap to the ladder
			Vector3 pos = this.transform.position;
			float ladder_x = ladderSpine.points[0].x;
			this.transform.position = new Vector3(ladder_x, pos.y, pos.z);

			// The platform controller is not active while on a ladder, but our ladder controller is
			_IsClimbing = true;
		}

		public void GotoLadderState_FromTop(EdgeCollider2D ladderSpine)
		{
			// We need to snap to the ladder plus move down past the ladder top so we don't collide with it
			// (We take for granted that the edge collider points for the spine go from down-to-up)
			Vector3 pos = this.transform.position;
			float ladder_x = ladderSpine.points[0].x;
			float ladder_y = ladderSpine.points[1].y;
			this.transform.position = new Vector3(ladder_x, ladder_y - 2.0f, pos.z);

			// The platform controller is not active while on a ladder, but our ladder controller is
			_IsClimbing = true;
		}

		public void GotoPlatformState()
		{
			// Platform controller is now enabled. Other player controllers are disabled.
			_IsClimbing = false;
		}
	}

	public static class InputControlsManager
	{
		public static bool TestInput(RuntimePlatform currentPlatform)
		{
			if (Application.platform == currentPlatform)
			{
				return true;
			}
			
			return false;
		}
		
		public static bool TestInput(RuntimePlatform[] currentPlatform)
		{
			if (Array.Exists(currentPlatform, element => element == Application.platform))
			{
				return true;
			}
			
			return false;
		}
	}
}