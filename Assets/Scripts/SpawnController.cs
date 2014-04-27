using UnityEngine;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour {
	public GameObject friendlySoldierPrefab;
	public GameObject dronePrefab;

	public static List<GameObject> missingFriendlySoldiers;

	public static List<GameObject> foundFriendlySoldiers;

	public static int friendlySoldierCount = 0;

	// Constants
	public const int NUM_DRONES = 10;
	public const int MIN_SOLDIERS_PER_SQUAD = 8;
	public const int MAX_SOLDIERS_PER_SQUAD = 12;
	public const float MAX_SQUAD_RADIUS = 10f;
	
	
	// Use this for initialization
	void Start () {
		missingFriendlySoldiers = new List<GameObject>();
		foundFriendlySoldiers = new List<GameObject>();
		
		// Initialize friendly soldiers in the buildings
		foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint"))
		{
			int numSoldiers = Random.Range (MIN_SOLDIERS_PER_SQUAD, MAX_SOLDIERS_PER_SQUAD);
			float deltaDegrees = 360f / numSoldiers;

			Vector3 platoonCentroid = spawnPoint.transform.position;
			
			for (float degree = 0f; degree < 360f; degree += deltaDegrees)
			{
				float radius = Random.Range(1, MAX_SQUAD_RADIUS);
				Vector3 spawnPosition = new Vector3(
					platoonCentroid.x + (radius * Mathf.Sin(degree * Mathf.Deg2Rad)),
					platoonCentroid.y,
					platoonCentroid.z + (radius * Mathf.Cos(degree * Mathf.Deg2Rad)));

				GameObject soldier = (GameObject) Instantiate(friendlySoldierPrefab, spawnPosition, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
				missingFriendlySoldiers.Add (soldier);
				friendlySoldierCount++;
			}
		}
		
		// Initialize search drones
		Vector3 helicopterPosition = GameObject.FindGameObjectWithTag ("Helicopter").transform.position;
		
		for (int i = 0; i < NUM_DRONES; i++)
		{		
			// Show the first drone camera in the corner of the screen
			if (i == 0)
			{
				var drone = Instantiate (dronePrefab, helicopterPosition, Quaternion.identity);
				
				((GameObject)drone).camera.depth = 1;
			}
			else
			{
				Instantiate (dronePrefab, helicopterPosition, Quaternion.identity);
			}
		}
	}
}
