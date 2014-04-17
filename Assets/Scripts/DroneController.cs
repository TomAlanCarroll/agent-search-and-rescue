using UnityEngine;
using System.Collections;

public class DroneController : MonoBehaviour {
	public enum Strategy {
		SPREAD_OUT
	}

	// Constants
	public const float TURN_RATE = 200f;
	public const float FLYING_SPEED = 4.47f; // 10mph is approx 4.47 m/s

	public Strategy strategy;

		
	void Start () {
		strategy = Strategy.SPREAD_OUT;
	}
	
	void Update () {	
		// Look for friendly soldiers


//		Travel (strategy);
	}

	private void Travel()
	{
//		float speed = 0f;
//		Vector3 moveDirection;
//
//		// Only apply gravity if we are not moving
//		if (!isMoving)
//		{
//			// Only apply gravity if we are falling
//			moveDirection = Vector3.zero;
//			
//			// Apply gravity
//			moveDirection.y -= GRAVITY * Time.deltaTime;
//			
//			// Move towards the position
//			controller.Move(moveDirection);
//			return;
//		}
//
//		if (state == State.RUNNING)
//		{
//			speed = RUNNING_SPEED;
//		}
//		else if (state == State.WALKING)
//		{
//			speed = WALKING_SPEED;
//		}		
//		
//		// Rotate towards destination
//		Vector3 rotation = Vector3.RotateTowards(transform.forward, (transform.position - destination), 
//		                                         Mathf.Deg2Rad * TURN_RATE * Time.deltaTime, 1);
//		transform.rotation = Quaternion.LookRotation(rotation);
//		
//		moveDirection = speed * Vector3.Normalize(transform.position - destination) * Time.deltaTime;
//		
//		// Apply gravity
//		moveDirection.y -= GRAVITY * Time.deltaTime;
//		
//		// Move towards the position
//		controller.Move(moveDirection);
	}
}
