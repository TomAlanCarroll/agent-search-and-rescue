using UnityEngine;
using System.Collections;

public class SoldierController : MonoBehaviour {
	public enum State {
		IDLE,
		RUNNING,
		WALKING,
		HIT,
		DEATH
	}

	[System.Serializable]
	public class  Anim {
		public AnimationClip walk;
		public AnimationClip stand_Lshot;
		public AnimationClip stand_Rshot;
		public AnimationClip seat_Lshot;
		public AnimationClip seat_Fshot;
		public AnimationClip stand_Fshot;
		public AnimationClip idle;
		public AnimationClip hit;
		public AnimationClip death;
		public AnimationClip run;
	}
	
	
	public Anim anim;
	public Animation  aniBody;
	public State state;
	public Vector3 destination;
	public CharacterController controller;
	private bool isMoving = false;

	// Constants
	public const float DEFAULT_FADE_LENGTH = 0.5f;
	public const float TURN_RATE = 200f;
	public const float RUNNING_SPEED = 4.47f; // 10mph is approx 4.47 m/s
	public const float WALKING_SPEED = 1.78f; // 4mph is approx 1.78 m/s
	public const float GRAVITY = 9.8f; // 4mph is approx 1.78 m/s
		
	void Start () {
		state = State.IDLE;

//		RunTowards (new Vector3(transform.position.x +10f, transform.position.y, transform.position.z));
	}
	
	void Update () {		
		// Go to idle state if we have reached the destination
		if (isMoving && 
		    destination.x - transform.position.x < 5 &&
		    destination.z - transform.position.z < 5)
		{
			state = State.IDLE;
			isMoving = false;
		}
		else
		{
			Travel ();
		}

		// Animate depending on state
		if (state == State.IDLE || !controller.isGrounded) 
		{
			aniBody.CrossFade (anim.idle.name, DEFAULT_FADE_LENGTH);
		}
		else if (state == State.RUNNING)
		{
			aniBody.CrossFade (anim.run.name, DEFAULT_FADE_LENGTH);
		}
		else if (state == State.WALKING)
		{
			aniBody.CrossFade (anim.walk.name, DEFAULT_FADE_LENGTH);
		}
		else if (state == State.HIT)
		{
			aniBody.CrossFade (anim.hit.name, DEFAULT_FADE_LENGTH);
		}
		else if (state == State.DEATH)
		{
			aniBody.CrossFade (anim.death.name, DEFAULT_FADE_LENGTH);
		}
	}

	public void RunTowards (Vector3 destination)
	{
		this.destination = destination;
		this.state = State.RUNNING;
		isMoving = true;
	}

	private void Travel()
	{
		float speed = 0f;
		Vector3 moveDirection;

		// Only apply gravity if we are not moving
		if (!isMoving)
		{
			// Only apply gravity if we are falling
			moveDirection = Vector3.zero;
			
			// Apply gravity
			moveDirection.y -= GRAVITY * Time.deltaTime;
			
			// Move towards the position
			controller.Move(moveDirection);
			return;
		}

		if (state == State.RUNNING)
		{
			speed = RUNNING_SPEED;
		}
		else if (state == State.WALKING)
		{
			speed = WALKING_SPEED;
		}		
		
		// Rotate towards destination
		Vector3 rotation = Vector3.RotateTowards(transform.forward, (transform.position - destination), 
		                                         Mathf.Deg2Rad * TURN_RATE * Time.deltaTime, 1);
		transform.rotation = Quaternion.LookRotation(rotation);
		
		moveDirection = speed * Vector3.Normalize(transform.position - destination) * Time.deltaTime;
		
		// Apply gravity
		moveDirection.y -= GRAVITY * Time.deltaTime;
		
		// Move towards the position
		controller.Move(moveDirection);
	}
}
