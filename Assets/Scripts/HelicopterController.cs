using UnityEngine;
using System.Collections;

public class HelicopterController : MonoBehaviour {
	public enum RescueStrategy 
	{
		RESCUE_CLOSEST
	}
	
	// Constants
	public const float TURN_RATE = 200f;
	public const float FLYING_SPEED = 12f;
	public const float INITIAL_RADIUS = 10f;

	public RescueStrategy rescueStrategy;

	public CharacterController controller;
	public static bool isGrounded;
	public static bool isAscending;

	private Vector3 startPosition;
	private Vector3 destination;

	// Use this for initialization
	void Start () {
		rescueStrategy = RescueStrategy.RESCUE_CLOSEST;
	}
	
	// Update is called once per frame
	void Update () {
		// Check for found soldiers 
		if (SpawnController.foundFriendlySoldiers.Count > 0)
		{
			switch(rescueStrategy)
			{
				case RescueStrategy.RESCUE_CLOSEST:
				// Find the closest soldier that has not yet been rescued
				GameObject closestSoldier = null;
				float minDistance = float.PositiveInfinity;
				
				foreach (GameObject soldier in SpawnController.foundFriendlySoldiers)
				{
					if (Vector3.Distance(transform.position, soldier.transform.position) < minDistance)
					{
						closestSoldier = soldier;
						minDistance = Vector3.Distance(transform.position, soldier.transform.position);
					}
				}

				if (closestSoldier != null)
				{
					destination = closestSoldier.transform.position;
					startPosition = transform.position;
				}
					break;
			}
		}
		
		Travel();		
	}
	
	private void Travel()
	{
		if (destination != null && startPosition != null && SpawnController.foundFriendlySoldiers.Count > 0)
		{			
			// Maintain height
			Vector3 actualDestination = new Vector3(destination.x, transform.position.y, destination.z);
			
			transform.LookAt (actualDestination, Vector3.up);

			if (Vector3.Distance(transform.position, actualDestination) > 3f && transform.position.y > 139)
			{
				isGrounded = false;
				isAscending = false;

				// Move towards the destination
				transform.position = Vector3.Lerp(startPosition, actualDestination, 0.3f * Time.deltaTime);
			}
			else if (!controller.isGrounded && !isAscending)
			{
				isGrounded = false;

				// Descend
				controller.Move (new Vector3(0, -1, 0));
			}
			else
			{
				isGrounded = true;

				// Check for soldiers in the rescue range
				// If there are any, wait
				// else ascend to cruising altitude
				GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Friendly");
				for (int i = 0; i < soldiers.Length; i++)
				{
					if (Vector3.Distance(transform.position, soldiers[i].transform.position) < 20f)
					{
						// Wait for in range soldiers
						return;
					}
				}

				// No nearby soldiers; ascend
				isAscending = true;
				controller.Move (new Vector3(0, 1, 0));
			}
		}
	}
}
