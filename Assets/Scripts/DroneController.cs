using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneController : MonoBehaviour {
	// Constants
	public const float TURN_RATE = 200f;
	public const float FLYING_SPEED = 40f;
	public const float INITIAL_RADIUS = 10f;
	public const float GRAVITY = 25f;
	public const float AOI_ROTATION_SPEED = 5f;

	public CharacterController controller;

	// State flags
	private bool initial = true;
	private bool areaOfInterestSearch = false;

	private float initialTravelAngle;
	private float initialLength;
	private Vector3 initialPosition;
	private float xOffset;
	private float zOffset;

	// The current destination of the drones
	private Vector3 destination;

	// The current area of interest to circle around (if any)
	private Vector3 areaOfInterest;

	// The degree counter for AOI rotation
	private float areaOfInterestRotationCounter = 0f;

	private Vector3 previousPosition;

	private float velocity;
		
	void Start () {

		// Determine a random angle to travel towards relative to the helicopter
		initialTravelAngle = UnityEngine.Random.Range(0f, 360f);

		initialPosition = transform.position;
		
		destination = transform.position;
		
		xOffset = INITIAL_RADIUS * Mathf.Cos(initialTravelAngle * Mathf.Deg2Rad);
		zOffset = INITIAL_RADIUS * Mathf.Sin(initialTravelAngle * Mathf.Deg2Rad);
		
		destination.x += xOffset;
		destination.z += zOffset;

		initialLength = Vector3.Distance(initialPosition, destination);
	}
	
	void Update () {	
		// Update the destination y to the current y (as it will depend on the terrain)
		destination.y = transform.position.y;

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
				destination.z = transform.position.z + zOffset;
			}
		}
		else
		{
			// Check if this drone has found a target
			if (Vector3.Distance(transform.position, destination) < 1f)
			{
				
				switch (SpawnController.droneStrategy) 
				{
				case SpawnController.DroneStrategy.RANDOM:
					// Pick a random point within the terrain to be the next destination
					destination = new Vector3(Random.Range (SpawnController.MIN_X, SpawnController.MAX_X), 0f, Random.Range (SpawnController.MIN_Z, SpawnController.MAX_Z)); 
					break;
				case SpawnController.DroneStrategy.ZONES:
					destination = new Vector3(Random.Range (SpawnController.currentZone.MIN_X, SpawnController.currentZone.MAX_X), 
					                          0f, 
					                          Random.Range (SpawnController.currentZone.MIN_Z, SpawnController.currentZone.MAX_Z)); 
					break;
				}
			}
			// Check if the velocity of the drone has decreased below the allowed threshold
			else if (velocity < 1f)
			{
				switch (SpawnController.droneStrategy) 
				{
				case SpawnController.DroneStrategy.RANDOM:
					// Pick a random point within the terrain to be the next destination
					destination = new Vector3(Random.Range (SpawnController.MIN_X, SpawnController.MAX_X), 0f, Random.Range (SpawnController.MIN_Z, SpawnController.MAX_Z)); 
					break;
				case SpawnController.DroneStrategy.ZONES:
//					destination = new Vector3(Random.Range (SpawnController.currentZone.MIN_X, SpawnController.currentZone.MAX_X), 
//					                          0f, 
//					                          Random.Range (SpawnController.currentZone.MIN_Z, SpawnController.currentZone.MAX_Z)); 

					int currentZoneIndex = SpawnController.zones.IndexOf(SpawnController.currentZone);

					float minX = 0;
					float maxX = 0;
					float minZ = 0;
					float maxZ = 0;

					if (currentZoneIndex > 0)
					{
						minX = SpawnController.zones[currentZoneIndex - 1].MIN_X;
					}
					else
					{
						minX = SpawnController.zones[SpawnController.zones.Count - 1].MIN_X;
					}

					if (currentZoneIndex < SpawnController.zones.Count - 1)
					{
						maxX = SpawnController.zones[currentZoneIndex + 1].MAX_X;
					}
					else
					{
						maxX = SpawnController.zones[0].MAX_X;
					}


					minZ = SpawnController.MIN_Z;
					maxZ = SpawnController.MAX_Z;

					destination = new Vector3(Random.Range (minX, maxX), 0f, Random.Range (minZ, maxZ)); 
					break;
				}
			}

			// Check for soldiers in the cameras view
			GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Friendly");
			for (int i = 0; i < soldiers.Length; i++)
			{
				SoldierController soldierController = soldiers[i].GetComponent<SoldierController>();
				if (soldierController.status == SoldierController.Status.NOT_FOUND)
				{
					if (CheckLineOfSight(soldiers[i]))
					{
						soldierController.status = SoldierController.Status.FOUND;

						SpawnController.missingFriendlySoldiers.Remove(soldiers[i]);
						SpawnController.foundFriendlySoldiers.Add(soldiers[i]);

						SpawnController.foundFriendlySoldierCount++;

						// Mark this as an area of interest to search around
						areaOfInterest = soldiers[i].transform.position;
						areaOfInterestSearch = true;

						// Update Statistics
						StatisticsWriter.Found();
					}
				}
			}
		}
		
		previousPosition = transform.position;

		Travel();

		// Calculate the current velocity
		velocity = (Vector3.Distance (transform.position, previousPosition)) / Time.deltaTime;
	}

	private void Travel()
	{
		// Check for a current area of interest
		if (!areaOfInterestSearch)
		{
			// No area of interest, travel to the next destination
			Vector3 moveDirection;

			transform.LookAt (destination, Vector3.up);
			
			moveDirection = FLYING_SPEED * Vector3.Normalize(destination - transform.position) * Time.deltaTime;

			// Apply gravity
			moveDirection.y -= GRAVITY * Time.deltaTime;
			
			// Move towards the position
			controller.Move(moveDirection);
		}
		else
		{
			// There is an area of interest
			// Rotate the drone around the area of interest with a radius of the camera's far clipping plane
			Vector3 currentPosition = transform.position - areaOfInterest;

			float angle = 5f * FLYING_SPEED * Time.deltaTime;

			Vector3 newPosition = Quaternion.Euler(0, angle, 0) * currentPosition;

			Vector3 displacement = newPosition - currentPosition;

			// Apply gravity
			displacement.y -= GRAVITY * Time.deltaTime;

			// Move the controller
			controller.Move(displacement);

			// Keep looking at the area of interest
			transform.LookAt (areaOfInterest, Vector3.up);

			areaOfInterestRotationCounter += angle;

			// If the drone has rotated in a full circle, end area of interest rotation
			if (areaOfInterestRotationCounter > 360f)
			{
				areaOfInterestSearch = false;
				areaOfInterestRotationCounter = 0;
				areaOfInterest = Vector3.zero;
			}
		}
	}

	/// <summary>
	/// Checks the line of sight for the target within the cameras far clip plan and field of view
	/// </summary>
	/// <returns><c>true</c>, if the target is within the cameras far clip plan and field of view, <c>false</c> otherwise.</returns>
	/// <param name="target">Target. Any GameObject in the environment</param>
	private bool CheckLineOfSight (GameObject target) 
	{
		if (Vector3.Distance(transform.position, target.transform.position) < camera.farClipPlane) // target is within camera far clip plane
		{
			if (Vector3.Angle(target.transform.position - transform.position, transform.forward) <= camera.fieldOfView) // target is within FOV
			{
				if(Physics.Linecast(transform.position, target.transform.position, Physics.DefaultRaycastLayers) == false) // target can be raycast (is visible) from the camera
				{
					// therefore, target is viewable by this drone
					return true;
				}
			}
		}

		return false;
	}
}
