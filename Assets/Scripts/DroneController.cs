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
	public const float INITIAL_RADIUS = 10f;
	public const float GRAVITY = 9.8f;

	public Strategy strategy;

	public CharacterController controller;

	private bool initial = true;

	private float initialTravelAngle;
	private float initialLength;
	private Vector3 initialPosition;
	private float xOffset;
	private float zOffset;

	private Vector3 destination;
		
	void Start () {
		strategy = Strategy.RANDOM;

		// Determine a random angle to travel towards relative to the helicopter
		initialTravelAngle = Random.Range(0f, 360f);

		initialPosition = transform.position;
		
		destination = transform.position;
		
		xOffset = INITIAL_RADIUS * Mathf.Cos(initialTravelAngle * Mathf.Deg2Rad);
		zOffset = INITIAL_RADIUS * Mathf.Sin(initialTravelAngle * Mathf.Deg2Rad);
		
		destination.x += xOffset;
		destination.z += zOffset;

		initialLength = Vector3.Distance(initialPosition, destination);
	}
	
	void Update () {	
		// If we have just spawned at the helicopter
		if (initial)
		{
			if (controller.isGrounded)
			{
				initial = false;
			}
			else
			{
				destination.x = transform.position.x + xOffset;
				destination.y = transform.position.y;
				destination.z = transform.position.z + zOffset;
			}
		}

		Travel (strategy);
	}

	private void Travel(Strategy strategy)
	{
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

		transform.LookAt (destination, Vector3.up);
		
		moveDirection = FLYING_SPEED * Vector3.Normalize(destination - transform.position) * Time.deltaTime;

		// Apply gravity
		moveDirection.y -= GRAVITY * Time.deltaTime;
		
		// Move towards the position
		controller.Move(moveDirection);
	}
}
