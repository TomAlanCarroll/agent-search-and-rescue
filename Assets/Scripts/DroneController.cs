using UnityEngine;
using System.Collections;

public class DroneController : MonoBehaviour {
	public enum Strategy {
		RANDOM,
		SPREAD_OUT
	}

	// Constants
	public const float TURN_RATE = 200f;
	public const float FLYING_SPEED = 4.47f; // 10mph is approx 4.47 m/s
	public const float INITIAL_RADIUS = 5f;

	public Strategy strategy;

	public CharacterController controller;

	private bool initial = true;

	private float initialTravelAngle;

	private Vector3 destination;
		
	void Start () {
		strategy = Strategy.RANDOM;
	}
	
	void Update () {	
		// Look for friendly soldiers

		// If we have just spawned at the helicopter
		if (initial)
		{
			// Determine a random angle to travel towards relative to the helicopter
			initialTravelAngle = Random.Range(0, 360);

			Vector3 startPosition = GameObject.FindGameObjectWithTag ("Helicopter").transform.position;

			float xOffset = INITIAL_RADIUS * Mathf.Cos(initialTravelAngle * Mathf.Deg2Rad);
			float zOffset = INITIAL_RADIUS * Mathf.Sin(initialTravelAngle * Mathf.Deg2Rad);
			
			startPosition.x += xOffset;
			startPosition.z += zOffset;

			initial = false;
		}


		Travel (strategy);
	}

	private void Travel(Strategy strategy)
	{
		float speed = 0f;
		Vector3 moveDirection;

		switch (strategy) 
		{
			case Strategy.RANDOM:
				// TODO: Implement				
				break;
			case Strategy.SPREAD_OUT:
				// TODO: Implement
				break;
		}
		
		// Rotate towards destination
		Vector3 rotation = Vector3.RotateTowards(transform.forward, (transform.position - destination), 
		                                         Mathf.Deg2Rad * TURN_RATE * Time.deltaTime, 1);
		transform.rotation = Quaternion.LookRotation(rotation);
		
		moveDirection = speed * Vector3.Normalize(transform.position - destination) * Time.deltaTime;
		
		// Move towards the position
		controller.Move(moveDirection);
	}
}
