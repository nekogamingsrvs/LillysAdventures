using CnControls;
using Prime31;
using UnityEngine;

namespace VoidInc
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CharacterController2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	public class PlayerController2D : MonoBehaviour
	{
		public float Gravity;
		public float WalkSpeed;
		public float RunModifierSpeed;
		public float InAirDamping;
		public float JumpHeight;

		//private static readonly float GravityModifierInWater = 0.375f;

		public float AbsorbGroundedInputTime;
		public float LadderVelocityPerSecond;

		private float _NormalizedHorizontalSpeed = 0.0f;

		private CharacterController2D _Controller;
		private Animator _Animator;
		private SpriteRenderer _SpriteRenderer;
		private AudioSource _AudioSource;
		private Transform _Linecast1;
		private Transform _Linecast2;

		private Vector3 _Velocity;
		private bool _IsRunning;
		private bool _IsClimbing;
		private float _Climb;
		private float _Modifier;


		// Use this for initialization.
		void Awake()
		{
			// Set the animator and controller components.
			_Animator = GetComponent<Animator>();
			_Controller = GetComponent<CharacterController2D>();
			_AudioSource = GetComponent<AudioSource>();
			_SpriteRenderer = GetComponent<SpriteRenderer>();

			// Listen to some events for illustration purposes.
			_Controller.onControllerCollidedEvent += onControllerCollider;
			_Controller.onTriggerEnterEvent += onTriggerEnterEvent;
			_Controller.onTriggerExitEvent += onTriggerExitEvent;

			_Linecast1 = transform.Find("LadderLinecast1");
			_Linecast2 = transform.Find("LadderLinecast2");
		}

		#region Event Listeners
		void onControllerCollider(RaycastHit2D hit)
		{
			// Bail out on plain old ground hits cause they aren't very interesting.
			if (hit.normal.y == 1f)
			{
				return;
			}

			// Check ladder collision, then go to platform state.
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ladders"))
			{
				GotoPlatformState();
			}

			// Logs any collider hits if uncommented. it gets noisy so it is commented out for the demo.
			//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
		}

		// Update when the trigger enters the event.
		void onTriggerEnterEvent(Collider2D col)
		{
			// If the trigger collided with an object with the tag "Coins1" then remove the coin and add score.
			col.gameObject.SendMessage("RemoveItem", SendMessageOptions.DontRequireReceiver);
			// If the trigger collided with an object with the tag "Signs" then display the information about the sign.
			col.gameObject.SendMessage("DisplayDialog", SendMessageOptions.DontRequireReceiver);

			// Debug if the player has entered a collision.
			//if (GameObject.Find("GameManager").GetComponent<GameManager>().isDebugActive)
			//{
			//	Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
			//}
		}

		// Update when the trigger exits the event.
		void onTriggerExitEvent(Collider2D col)
		{
			// Check of the player has exited the LadderSpines layer and go to platform state.
			if (col.gameObject.layer == LayerMask.NameToLayer("LadderSpines"))
			{
				GotoPlatformState();
			}

			// Debug if the player has exited a collision.
			//if (GameObject.Find("GameManager").GetComponent<GameManager>().isDebugActive)
			//{
			//	Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
			//}
		}
		#endregion

		// Update is called once per frame.
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

			// Play the meowing sound of Lilly.
			if (_Controller.isGrounded && (Input.GetButton("Meow") || CnInputManager.GetButton("Meow")))
			{
				_AudioSource.Play();

				_Animator.SetTrigger("Meowing");
			}

			// Get button down if the player has running checked. 
			if (InputCheck.IsMobilePlatforms)
			{
				_IsRunning = GameObject.Find("RunToggleButton").GetComponent<SimpleButton>().ToggleState;
			}
			if (InputCheck.IsPCPlatforms)
			{
				_IsRunning = InputCheck.GetAxisMinMax(Input.GetAxis("Run"));
				_IsRunning = Input.GetButton("Run");
			}
			if (InputCheck.IsConsolePlatforms)
			{
				_IsRunning = InputCheck.GetAxisMinMax(Input.GetAxis("Run"));
			}

			// Set the animators boolean of if the player is running.
			_Animator.SetBool("Running", _IsRunning);

			// Set the normalized horizontal speed of the player.
			_NormalizedHorizontalSpeed = Input.GetAxis("Horizontal") + CnInputManager.GetAxis("Horizontal");

			// Set the animators float of the speed of the player.
			_Animator.SetFloat("Speed", Mathf.Abs(_NormalizedHorizontalSpeed));

			// Get if the player is jumping.
			if (_Controller.isGrounded && (Input.GetButton("Jump") || CnInputManager.GetButton("Jump")))
			{
				// Jumping.
				_Velocity.y = JumpHeight;

				// Raise our MegaDad dude up a bit to start the jump off
				// Otherwise, his leg starts off a bit too deep into the ground
				this.transform.Translate(0, 4, 0);
			}

			// Flip the character based off of his speed.
			if (_NormalizedHorizontalSpeed > 0)
			{
				_SpriteRenderer.flipX = false;
			}
			else if (_NormalizedHorizontalSpeed < 0)
			{
				_SpriteRenderer.flipX = true;
			}
			#endregion

			#region Ladder Controls
			// Check if the player is climbing a ladder.
			if (LadderCheck())
			{
				_IsClimbing = true;
			}

			// Check if the player is climbing.
			if (_IsClimbing)
			{
				// We only animate the player while he is moving up/down the ladder
				float climbing = Input.GetAxisRaw("Vertical");

				// Are we jumping? If so, we leave this state.
				if (climbing == 0 && (Input.GetButtonDown("Jump") || CnInputManager.GetButtonDown("Jump")))
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
						_Controller.move(new Vector3(0, 0, 0));

						// Go back to the platform state
						GotoPlatformState();
					}
				}

				// If we're moving down a ladder then it's possible to fall off it (because it comes to an end)
				if (climbing < 0)
				{
					// Check if the "top half" of our player is crossing a "LadderBottom"
					RaycastHit2D ladderBottomRay = Physics2D.Linecast(_Linecast2.position, this.transform.position, 1 << LayerMask.NameToLayer("LadderBottoms"));

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
			// Check if the player is running.
			if (_IsRunning)
			{
				_Modifier = _Controller.isGrounded ? WalkSpeed * RunModifierSpeed : InAirDamping * RunModifierSpeed;
			}
			else
			{
				_Modifier = _Controller.isGrounded ? WalkSpeed : InAirDamping;
			}

			// Set the product of the horizontal speed and the modifier to the players velocity.
			_Velocity.x = _NormalizedHorizontalSpeed * _Modifier;

			// Apply gravity before moving.
			if (!_IsClimbing)
			{
				_Velocity.y += Gravity * Time.deltaTime * 1.0f;
			}

			// Move the controller at the velocity and fluctuate it with time.
			_Controller.move(_Velocity * Time.deltaTime);

			// grab our current _velocity to use as a base for all calculations
			_Velocity = _Controller.velocity;
			#endregion
		}

		private bool LadderCheck()
		{
			float climbing = InputCheck.IsMobilePlatforms ? CnInputManager.GetAxis("Vertical") : Input.GetAxisRaw("Vertical");

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
			this.transform.position = new Vector3(ladder_x + 16, ladder_y, pos.z);

			// The platform controller is not active while on a ladder, but our ladder controller is
			_IsClimbing = true;
		}

		public void GotoPlatformState()
		{
			// Platform controller is now enabled. Other player controllers are disabled.
			_IsClimbing = false;
		}
	}
}