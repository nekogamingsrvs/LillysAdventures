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
		public float Gravity = -25.0f;
		public float RunMultiplierSpeed = 2.0f;
		public float WalkSpeed = 16.0f;
		public float GroundDamping = 20.0f;
		public float InAirDamping = 5.0f;
		public float JumpHeight = 48.0f;
		public static readonly float LadderVelocityPerSecond = 1.0f * 60.0f;

		[HideInInspector]
		private float NormalizedHorizontalSpeed = 0.0f;

		private CharacterController2D _Controller;
		private Animator _Animator;
		private RaycastHit2D _LastControllerColliderHit;
		private Vector3 _Velocity;
		private bool _OnLadder;
		private bool _IsRunning;
		private float _Climbing;

		#region Platforms
		private RuntimePlatform[] _PCPlatforms =
		{
			RuntimePlatform.OSXEditor,
			RuntimePlatform.OSXPlayer,
			RuntimePlatform.WindowsPlayer,
			RuntimePlatform.OSXWebPlayer,
			RuntimePlatform.OSXDashboardPlayer,
			RuntimePlatform.WindowsWebPlayer,
			RuntimePlatform.WindowsEditor,
			RuntimePlatform.LinuxPlayer,
			RuntimePlatform.WebGLPlayer,
			RuntimePlatform.WSAPlayerX86,
			RuntimePlatform.WSAPlayerX64,
			RuntimePlatform.WSAPlayerARM,
			RuntimePlatform.TizenPlayer
		};

		private RuntimePlatform[] _MobilePlatforms =
		{
			RuntimePlatform.IPhonePlayer,
			RuntimePlatform.Android,
			RuntimePlatform.WP8Player
		};
		#endregion

		// If we encounter a ladder top between these points then we are "bending over" a ladder
		private Transform _Linecast1 { get; set; }
		private Transform _Linecast2 { get; set; }
		private TriggerTest _LadderGrabTest;
		private TriggerTest _LadderDownTest;

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

			// Set the line cast components.
			_Linecast1 = this.transform.Find("LadderLinecast1");
			_Linecast2 = this.transform.Find("LadderLinecast2");
			_LadderGrabTest = this.transform.Find("LadderGrabTest").GetComponent<TriggerTest>();
			_LadderDownTest = this.transform.Find("LadderDownTest").GetComponent<TriggerTest>();

			if (GameObject.FindObjectOfType<GameManager>().isDebugActive)
			{
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("VelocityX", _Velocity.x);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("VelocityY", _Velocity.y);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("OnLadder", _OnLadder);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("IsRunning", _IsRunning);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("Climbing", _Climbing);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("IsGrounded", _Controller.isGrounded);
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

			if (_OnLadder && hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
			{
				_Animator.SetTrigger("ClimbedOffLadder");
				_OnLadder = false;
			}

			// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
			//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
		}
		
		// Update when the trigger enters the event.
		void onTriggerEnterEvent(Collider2D col)
		{
			// If the trigger collided with Coin1 layer then remove the coin and add score.
			if (col.gameObject.layer == LayerMask.NameToLayer("Coins1"))
			{
				col.gameObject.GetComponent<CoinIdentifier>().RemoveCoin();
			}
			// If the trigger collided with Coin5 layer then remove the coin and add score.
			if (col.gameObject.layer == LayerMask.NameToLayer("Coins5"))
			{
				col.gameObject.GetComponent<CoinIdentifier>().RemoveCoin();
			}
			// If the trigger collided with Coin10 layer then remove the coin and add score.
			if (col.gameObject.layer == LayerMask.NameToLayer("Coins10"))
			{
				col.gameObject.GetComponent<CoinIdentifier>().RemoveCoin();
			}
			// If the trigger collided with Gems layer then remove the gem and add to the gem amount.
			if (col.gameObject.layer == LayerMask.NameToLayer("Gems"))
			{
				col.gameObject.GetComponent<GemIdentifier>().RemoveGem();
			}
			// If the trigger collided with Keys layer then remove the key and add to the key amount.
			if (col.gameObject.layer == LayerMask.NameToLayer("Keys"))
			{
				col.gameObject.GetComponent<KeyIdentifier>().RemoveKey();
			}
			// If the trigger collided with Locks layer then check if we have a key and unlock it if we do.
			if (col.gameObject.layer == LayerMask.NameToLayer("Locks"))
			{
				col.gameObject.GetComponent<LockIdentifier>().CheckLock();
			}

			Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
		}

		// Update when the trigger exits the event.
		void onTriggerExitEvent(Collider2D col)
		{
			Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
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

			// Get if the application is on a desktop platform or if it is on a mobile platform
			if (InputControlsManager.TestInput(_PCPlatforms))
			{
				// Get if the player is not holding the run key down.
				if (!Input.GetButton("RunKey"))
				{
					// If the player is not running then move at a normal rate.
					NormalizedHorizontalSpeed = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
					_Animator.SetBool("Running", false);
					// Set the value of the float Speed in the Animator
					_Animator.SetFloat("Speed", 1);
				}
				else
				{
					// If the player is running then move at the running modifier speed.
					NormalizedHorizontalSpeed = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1) * RunMultiplierSpeed;
					_Animator.SetBool("Running", true);
					// Set the value of the float Speed in the Animator
					_Animator.SetFloat("Speed", 1);
				}
				
				// Get if the controller is grounded and the jump key is down then jump.
				if (_Controller.isGrounded && Input.GetButtonDown("Jump"))
				{
					_Velocity.y = Mathf.Sqrt(2.0f * JumpHeight * -Gravity);
				}
				
				// If the player is pressing the vertical movement keys then set climbing to the value.
				_Climbing = Input.GetAxis("Vertical");

				// If holding down bump up our movement amount and turn off one way platform detection for a frame.
				// This lets us jump down through one way platforms.
				if (_Controller.isGrounded && Input.GetAxis("Vertical") < 0 && !_OnLadder)
				{
					_Velocity.y *= 3f;
					_Controller.ignoreOneWayPlatformsThisFrame = true;
				}

				// Are we jumping? If so, we leave this state.
				if (_Climbing == 0 && Input.GetButtonDown("Jump"))
				{
					_Animator.SetTrigger("JumpedOffLadder");
				}
			}
			else if (InputControlsManager.TestInput(_MobilePlatforms))
			{
				// Get if the player has toggled the RunKey button
				if (CnInputManager.GetButtonDown("RunKey"))
				{
					_IsRunning = !_IsRunning;
				}

				// Get if the player is not running.
				if (!_IsRunning)
				{
					// If the player is not running then move at a normal rate.
					NormalizedHorizontalSpeed = Mathf.Clamp(CnInputManager.GetAxis("Horizontal"), -1, 1);
					_Animator.SetBool("Running", false);
					// Set the value of the float Speed in the Animator
					_Animator.SetFloat("Speed", 1);
				}
				else
				{
					// If the player is running then move at the running multiplier speed.
					NormalizedHorizontalSpeed = Mathf.Clamp(CnInputManager.GetAxis("Horizontal"), -1, 1) * RunMultiplierSpeed;
					_Animator.SetBool("Running", true);
					// Set the value of the float Speed in the Animator
					_Animator.SetFloat("Speed", 1);
				}
				
				// Get if the controller is grounded and the jump button is down then jump.
				if (_Controller.isGrounded && CnInputManager.GetButtonDown("Jump"))
				{
					_Velocity.y = Mathf.Sqrt(2.0f * JumpHeight * -Gravity);
				}
				
				// If the player is pressing the vertical movement buttons then set climbing to the value.
				_Climbing = CnInputManager.GetAxis("Vertical");

				// If holding down bump up our movement amount and turn off one way platform detection for a frame.
				// This lets us jump down through one way platforms.
				if (_Controller.isGrounded && CnInputManager.GetAxis("Vertical") < 0 && !_OnLadder)
				{
					_Velocity.y *= 3f;
					_Controller.ignoreOneWayPlatformsThisFrame = true;
				}

				// Are we jumping? If so, we leave this state.
				if (_Climbing == 0 && CnInputManager.GetButtonDown("Jump"))
				{
					_Animator.SetTrigger("JumpedOffLadder");
				}
			}
			else
			{
				Debug.LogError("Not supported platform");
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

			#region Movement Controls
			// Apply horizontal speed smoothing it. don't really do this with Lerp. Use SmoothDamp or something that provides more control.
			var smoothedMovementFactor = _Controller.isGrounded ? GroundDamping : InAirDamping; // How fast do we change direction?
			_Velocity.x = Mathf.Lerp(_Velocity.x, NormalizedHorizontalSpeed * WalkSpeed, Time.deltaTime * smoothedMovementFactor);

			// Apply gravity before moving.
			_Velocity.y += Gravity * Time.deltaTime;

			// Move the controller at the velocity and fluctuate it with time.
			_Controller.move(_Velocity * Time.deltaTime);

			// grab our current _velocity to use as a base for all calculations
			_Velocity = _Controller.velocity;
			#endregion

			#region Ladder Controls
			if (_OnLadder)
			{
				// If the trigger collided with Ladder layer then set it so that we are on a ladder.
				_Animator.SetTrigger("OnLadder");
				
				// If we linecast across a ladder top than we are either climbing off of or onto a ladder (bending over a ladder)
				RaycastHit2D ladderTopRay = Physics2D.Linecast(_Linecast1.position, _Linecast2.position, 1 << LayerMask.NameToLayer("LadderTops"));
				_Animator.SetFloat("LadderLinecast", ladderTopRay ? 1.0f : 0.0f);
				
				// If we're moving up a ladder and our "top half" no longer crosses a ladder top then we climb off the ladder
				if (ladderTopRay && _Climbing > 0)
				{
					if (!Physics2D.Linecast(_Linecast1.position, transform.position, 1 << LayerMask.NameToLayer("LadderTops")))
					{
						// Set the animation trigger to climb off ladder.
						_Animator.SetTrigger("ClimbedOffLadder");

						// To "pop off" the ladder we have to abruptly move to our "foot" 
						// The bottom of our linecast is made to represent our foot position
						// And our foot needs to rest on the ladder top we're crossing.
						Vector3 position = transform.position;
						float feet_y = _Linecast2.transform.position.y;
						float ladderTop_y = ladderTopRay.point.y;

						position.y = ladderTop_y + (position.y - feet_y);
						transform.position = position;

						// We may be just above the ladder top and we don't want to fall for a split second
						// So, move our character controller back down to the ground. He should collide with the ladder top we just popped past.
						_Controller.move(new Vector3(0, -0.25f, 0));
						_OnLadder = false;
					}
				}
				
				// Get if the climbing variable is less the 0 so that we can set that we jumped off the ladder.
				if (_Climbing < 0)
				{
					if (Physics2D.Linecast(_Linecast1.position, transform.position, 1 << LayerMask.NameToLayer("LadderBottoms")))
					{
						_Animator.SetTrigger("JumpedOffLadder");
						_OnLadder = false;
					}
				}
				
				// Get if we are on the ladder so that we don't fall off by negating gravity.
				if (_Climbing != 0)
				{
					Vector2 velocity = new Vector2(0, Mathf.Sign(_Climbing) * LadderVelocityPerSecond * Time.deltaTime);
					_Controller.move(velocity);
				}
			}
			#endregion

			if (GameObject.FindObjectOfType<GameManager>().isDebugActive)
			{
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("VelocityX", _Velocity.x);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("VelocityY", _Velocity.y);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("OnLadder", _OnLadder);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("IsRunning", _IsRunning);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("Climbing", _Climbing);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("IsGrounded", _Controller.isGrounded);
			}
		}
		
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
			
			_OnLadder = true;
		}

		/// <summary>
		/// Sets the player to the ladder state from the top.
		/// </summary>
		/// <param name="ladderSpine">The EdgeCollder2D of the ladder's top.</param>
		public void GotoLadderState_FromTop(EdgeCollider2D ladderTop)
		{
			// We need to snap to the ladder plus move down past the ladder top so we don't collide with it
			// (We take for granted that the edge collider points for the spine go from down-to-up)
			Vector3 pos = this.transform.position;
			float ladder_x = ladderTop.points[0].x;
			float ladder_y = ladderTop.points[1].y;
			this.transform.position = new Vector3(ladder_x, ladder_y - 2.0f, pos.z);
			
			_OnLadder = true;
		}

		private bool LadderCheck()
		{
			if (_Climbing != 0)
			{
				// Do we have a ladder to grab?
				EdgeCollider2D ladderGrabEdgeSpine = _LadderGrabTest.GetTriggerCollider<EdgeCollider2D>();
				EdgeCollider2D ladderDownEdgeSpine = _LadderDownTest.GetTriggerCollider<EdgeCollider2D>();

				if (_Climbing > 0)
				{
					if (ladderGrabEdgeSpine != null)
					{
						GotoLadderState_FromSpine(ladderGrabEdgeSpine);
						return true;
					}
				}
				else if (_Climbing < 0)
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