using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class PlayerMovement : MonoBehaviour
{
	public float jumpForce;
	public float jumpForceLadder;
	public float moveForce;
	public float maxSpeed;
	public float maxRunSpeed;
	public Transform groundCheck;
	public GameObject gameController;

	[HideInInspector]
	public bool facingRight = false;
	[HideInInspector]
	public bool jump = true;

	private float currentMaxSpeed;
	private bool grounded;
	private bool touchingLadder;
	private float currentJumpForce;
	private Animator anim;
	private Rigidbody2D rb2d;
	private UnityEngine.UI.Text debugText;
	private Rect levelBoundries;

	void Awake()
	{
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		debugText = gameController.GetComponent<GameManager>().debugTextPanel.GetComponentInChildren<UnityEngine.UI.Text>();
		levelBoundries = gameController.GetComponent<GameManager>().levelBoundries;
	}

	void Update()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("tiles"));
		touchingLadder = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ladders"));

		if (Input.GetButtonDown("Jump") && grounded)
		{
			jump = true;
			currentJumpForce = jumpForce;
		}
		else if (Input.GetButtonDown("Jump") && grounded && touchingLadder)
		{
			jump = true;
			currentJumpForce = jumpForceLadder;
		}
	}

	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");

		if (Input.GetButton("RunKey"))
		{
			currentMaxSpeed = maxRunSpeed;
			anim.SetBool("Running", true);
		}
		else
		{
			currentMaxSpeed = maxSpeed;
			anim.SetBool("Running", false);
		}

		if (h * rb2d.velocity.x < currentMaxSpeed)
		{
			rb2d.AddForce(Vector2.right * h * moveForce);
		}

		if (Mathf.Abs(rb2d.velocity.x) > currentMaxSpeed)
		{
			rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * currentMaxSpeed, rb2d.velocity.y);
		}

		if (h < 0 && !facingRight)
		{
			Flip();
		}
		else if (h > 0 && facingRight)
		{
			Flip();
		}

		if (jump)
		{
			rb2d.AddForce(new Vector2(0f, currentJumpForce));
			jump = false;
		}

		if (rb2d.velocity.y > 0)
		{
			anim.SetBool("Jumping", true);
		}
		else
		{
			anim.SetBool("Jumping", false);
		}

		if (grounded)
		{
			anim.SetBool("Grounded", true);
		}
		else
		{
			anim.SetBool("Grounded", false);
		}

		anim.SetFloat("Speed", Mathf.Abs(h));

		if (debugText != null)
		{
			debugText.text = "Jump=" + jump + " Grounded=" + grounded + " Speed=" + rb2d.velocity.x;
		}
	}

	void LateUpdate()
	{
		Vector3 vectorClamp = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		vectorClamp.x = Mathf.Clamp(vectorClamp.x, levelBoundries.xMin, levelBoundries.xMax);
		vectorClamp.y = Mathf.Clamp(vectorClamp.y, -levelBoundries.yMax, levelBoundries.yMin);


		transform.position = vectorClamp;
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
