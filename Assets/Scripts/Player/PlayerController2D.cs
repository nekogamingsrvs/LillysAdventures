using CnControls;
using Prime31;
using UnityEngine;

namespace VoidInc.LWA
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
		
		public float AbsorbGroundedInputTime;
		public float LadderVelocityPerSecond;

		private static readonly float GravityModifierInWater = 0.375f;
		private float _NormalizedHorizontalSpeed = 0.0f;

		public CharacterController2D Controller;
		public Animator Animator;
		public SpriteRenderer SpriteRenderer;
		public AudioSource AudioSource;
		public Transform Linecast1;
		public Transform Linecast2;

		public Vector3 Velocity;
		public bool IsRunning;
		public bool IsJumping;
		public bool IsClimbing;
		public float Climb;
		public float Modifier;

		public string NameOfCollision = "None";

		// Use this for initialization.
		void Awake()
		{
			// Set as a singleton
			DontDestroyOnLoad(this);

			if (FindObjectsOfType(GetType()).Length > 1)
			{
				Destroy(gameObject);
			}

			// Listen to some events for illustration purposes.
			Controller.onControllerCollidedEvent += onControllerCollider;
			Controller.onTriggerEnterEvent += onTriggerEnterEvent;
			Controller.onTriggerExitEvent += onTriggerExitEvent;
		}

		#region Event Listeners
		void onControllerCollider(RaycastHit2D hit)
		{
			NameOfCollision = hit.collider.gameObject.name;

			// Bail out on plain old ground hits cause they aren't very interesting.
			if (hit.normal.y == 1f)
			{
				IsJumping = false;
				return;
			}

			// Check ladder collision, then go to platform state.
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ladders"))
			{
				GotoPlatformState();
			}
		}

		// Update when the trigger enters the event.
		void onTriggerEnterEvent(Collider2D col)
		{
			NameOfCollision = col.gameObject.name;

			if (col.gameObject.GetComponentInParent<ItemManager>() != null)
			{
				col.gameObject.GetComponentInParent<ItemManager>().SendMessage("RemoveItem", SendMessageOptions.DontRequireReceiver);
			}
			else if (col.gameObject.GetComponentInParent<ObjectManager>() != null)
			{
				col.gameObject.GetComponentInParent<ObjectManager>().SendMessage("ActivateObject", this, SendMessageOptions.DontRequireReceiver);
			}

			if (col.gameObject.layer == LayerMask.NameToLayer("Water"))
			{
				Gravity *= GravityModifierInWater;
			}
		}

		// Update when the trigger exits the event.
		void onTriggerExitEvent(Collider2D col)
		{
			// Check of the player has exited the LadderSpines layer and go to platform state.
			if (col.gameObject.layer == LayerMask.NameToLayer("LadderSpines"))
			{
				GotoPlatformState();
			}

			if (col.gameObject.layer == LayerMask.NameToLayer("Water"))
			{
				Gravity /= GravityModifierInWater;
			}
		}
		#endregion

		// Update is called once per frame.
		void Update()
		{
			#region Get or set input values
			// Get if the CharacterController2D is grounded and set the velocity to 0.
			Animator.SetBool("Grounded", Controller.isGrounded);
			if (Controller.isGrounded)
			{
				Velocity.y = 0;
			}

			// Get if the CharacterController2D velocity is greater than 0 so that we can play the jumping animation.
			Animator.SetBool("Jumping", Controller.velocity.y > 0 ? true : false);

			// Play the meowing sound of Lilly.
			if (Controller.isGrounded && (Input.GetButton("Meow") || CnInputManager.GetButton("Meow")))
			{
				AudioSource.Play();
				Animator.SetTrigger("Meowing");
			}

			// Get button down if the player has running checked.
			if (InputCheck.IsPCPlatforms)
			{
				if (!IsJumping)
				{
					IsRunning = InputCheck.GetAxisMinMax(Input.GetAxis("Run"));
					IsRunning = Input.GetButton("Run");
				}
			}
			if (InputCheck.IsConsolePlatforms)
			{
				if (!IsJumping)
				{
					IsRunning = InputCheck.GetAxisMinMax(Input.GetAxis("Run"));
				}
			}

			// Set the animators boolean of if the player is running.
			Animator.SetBool("Running", IsRunning);

			// Set the normalized horizontal speed of the player.
			if (!IsClimbing)
			{
				_NormalizedHorizontalSpeed = Input.GetAxis("Horizontal") + CnInputManager.GetAxis("Horizontal");
			}

			// Set the animators float of the speed of the player.
			Animator.SetFloat("Speed", Mathf.Abs(_NormalizedHorizontalSpeed));


			if (Controller.isGrounded && (Input.GetButton("Jump") || CnInputManager.GetButton("Jump")) && (Input.GetAxis("Vertical") < 0 || CnInputManager.GetAxis("Vertical") < 0))
			{
				Velocity.y *= 16.0f;
				IsClimbing = false;
				Controller.ignoreOneWayPlatformsThisFrame = true;
			}
			// Get if the player is jumping.
			else if (Controller.isGrounded && (Input.GetButton("Jump") || CnInputManager.GetButton("Jump")))
			{
				// Jumping.
				Velocity.y = JumpHeight;

				// Raise our Lilly kitty up a bit to start the jump off
				// Otherwise, his leg starts off a bit too deep into the ground
				this.transform.Translate(0, 4, 0);

				IsJumping = true;
			}

			// Flip the character based off of his speed.
			if (_NormalizedHorizontalSpeed > 0)
			{
				SpriteRenderer.flipX = false;
			}
			else if (_NormalizedHorizontalSpeed < 0)
			{
				SpriteRenderer.flipX = true;
			}
			#endregion

			#region Ladder Controls
			// Check if the player is climbing a ladder.
			if (LadderCheck())
			{
				IsClimbing = true;
			}

			// Check if the player is climbing.
			if (IsClimbing)
			{
				// We only animate the player while he is moving up/down the ladder
				Climb = Input.GetAxisRaw("Vertical"); //+ CnInputManager.GetAxis("Vertical");

				Velocity.x = 0;

				// Are we jumping? If so, we leave this state.
				if (Climb == 0 && (Input.GetButtonDown("Jump") || CnInputManager.GetButtonDown("Jump")))
				{
					GotoPlatformState();
					return;
				}

				// If we linecast across a ladder top than we are either climbing off of or onto a ladder (bending over a ladder)
				RaycastHit2D ladderTopRay = Physics2D.Linecast(Linecast1.position, Linecast2.position, 1 << LayerMask.NameToLayer("LadderTops"));

				// If we're moving up a ladder and our "top half" no longer crosses a ladder top then we climb off the ladder
				if (ladderTopRay && Climb > 0)
				{
					if (!Physics2D.Linecast(Linecast1.position, this.transform.position, 1 << LayerMask.NameToLayer("LadderTops")))
					{
						// To "pop off" the ladder we have to abruptly move to our "foot" 
						// The bottom of our linecast is made to represent our foot position
						// And our foot needs to rest on the ladder top we're crossing.
						Vector3 position = this.transform.position;
						float feet_y = Linecast2.transform.position.y;
						float ladderTop_y = ladderTopRay.point.y;

						position.y = ladderTop_y + (position.y - feet_y);
						this.transform.position = position;

						// We may be just above the ladder top and we don't want to fall for a split second
						// So, move our character controller back down to the ground. He should collide with the ladder top we just popped past.
						Controller.move(new Vector3(0, 14, 0));

						// Go back to the platform state
						GotoPlatformState();
					}
				}

				// If we're moving down a ladder then it's possible to fall off it (because it comes to an end)
				if (Climb < 0)
				{
					// Check if the "top half" of our player is crossing a "LadderBottom"
					RaycastHit2D ladderBottomRay = Physics2D.Linecast(Linecast2.position, this.transform.position, 1 << LayerMask.NameToLayer("LadderBottoms"));

					if (ladderBottomRay)
					{
						GotoPlatformState();
					}
				}

				// Move our character up/down the ladder
				// Note: There is no graivty applied while on the ladder
				if (Climb != 0)
				{
					Vector2 velocity = new Vector2(0, Mathf.Sign(Climb) * LadderVelocityPerSecond * Time.deltaTime);
					Controller.move(velocity);
				}
			}
			#endregion

			#region Movement Controls
			// Check if the player is running.
			if (IsRunning)
			{
				Modifier = Controller.isGrounded ? WalkSpeed * RunModifierSpeed : InAirDamping * RunModifierSpeed;
			}
			else
			{
				Modifier = Controller.isGrounded ? WalkSpeed : InAirDamping;
			}

			// Set the product of the horizontal speed and the modifier to the players velocity.

			// Apply gravity before moving.
			if (!IsClimbing)
			{
				Velocity.x = _NormalizedHorizontalSpeed * Modifier;
				Velocity.y += Gravity * Time.deltaTime;

				// Move the controller at the velocity and fluctuate it with time.
				Controller.move(Velocity * Time.deltaTime);
			}

			// grab our current _velocity to use as a base for all calculations
			Velocity = Controller.velocity;
			#endregion
		}

		private bool LadderCheck()
		{
			float climbing = CnInputManager.GetAxis("Vertical") + Input.GetAxisRaw("Vertical");

			if (climbing != 0)
			{
				EdgeCollider2D ladderGrabEdgeSpine = transform.FindChild("LadderGrabTest").GetComponent<TriggerTest>().GetTriggerCollider<EdgeCollider2D>();
				EdgeCollider2D ladderDownEdgeSpine = transform.FindChild("LadderDownTest").GetComponent<TriggerTest>().GetTriggerCollider<EdgeCollider2D>();

				if (climbing > 0)
				{
					if (ladderGrabEdgeSpine != null && ladderGrabEdgeSpine.gameObject.layer == LayerMask.NameToLayer("LadderSpines"))
					{
						GotoLadderState_FromSpine(ladderGrabEdgeSpine);
						return true;
					}
				}
				else if (climbing < 0)
				{
					// If there is already a ladder for us to grab onto without looking past a ladder top, than grab it
					// (In that case, we require the same ladder to be below us too)
					if (ladderGrabEdgeSpine != null && ladderDownEdgeSpine != null && ladderGrabEdgeSpine.gameObject.layer == LayerMask.NameToLayer("LadderSpines"))
					{
						GotoLadderState_FromSpine(ladderGrabEdgeSpine);
						return true;
					}

					// Try to climb down a ladder (past a ladder top edge)
					if (ladderDownEdgeSpine != null && ladderDownEdgeSpine.gameObject.layer == LayerMask.NameToLayer("LadderTops"))
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
			Vector3 pos = transform.position;
			float ladder_x = ladderSpine.points[0].x;
			transform.position = new Vector3(ladder_x, pos.y, pos.z);

			// The platform controller is not active while on a ladder, but our ladder controller is
			IsClimbing = true;
		}

		public void GotoLadderState_FromTop(EdgeCollider2D ladderSpine)
		{
			// We need to snap to the ladder plus move down past the ladder top so we don't collide with it
			// (We take for granted that the edge collider points for the spine go from down-to-up)
			Vector3 pos = transform.position;
			float ladder_x = ladderSpine.points[0].x;
			float ladder_y = ladderSpine.points[1].y;
			transform.position = new Vector3(ladder_x + 16, ladder_y, pos.z);

			// The platform controller is not active while on a ladder, but our ladder controller is
			IsClimbing = true;
		}

		public void GotoPlatformState()
		{
			// Platform controller is now enabled. Other player controllers are disabled.
			IsClimbing = false;
		}
	}
}