using UnityEngine;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour {
	public enum DroneStrategy {
		RANDOM,
		ZONES
	}

	public GameObject friendlySoldierPrefab;
	public GameObject dronePrefab;

	public static List<GameObject> missingFriendlySoldiers;

	public static List<GameObject> foundFriendlySoldiers;
	
	public static List<GameObject> rescuedFriendlySoldiers;

	public static int friendlySoldierCount = 0;
	
	public static int foundFriendlySoldierCount = 0;

	// Constants
	public const int NUM_DRONES = 10;
	public const int MIN_SOLDIERS_PER_SQUAD = 8;
	public const int MAX_SOLDIERS_PER_SQUAD = 12;
	public const float MAX_SQUAD_RADIUS = 10f;
	public const float ZONE_SEARCH_SECONDS = 30f;
	
	// Spawn Ranges
	public const float MIN_X = 75f;
	public const float MAX_X = 1070f;
	public const float MIN_Z = 50f;
	public const float MAX_Z = 700f;	
	
	// Zones (including ranges)
	private List<Zone> zones;
	
	// Current zone to search
	public static Zone currentZone;
	
	// When the searching of the current zone was started
	private System.DateTime startZoneSearch;
	
	public static DroneStrategy droneStrategy;
	
	
	// Use this for initialization
	void Start () {
		droneStrategy = DroneStrategy.ZONES;

		missingFriendlySoldiers = new List<GameObject>();
		foundFriendlySoldiers = new List<GameObject>();
		rescuedFriendlySoldiers = new List<GameObject>();
		
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

		// Zone instantiation
		if (droneStrategy == DroneStrategy.ZONES)
		{
			// Initialize the zones; 1 zone for each drone
			zones = new List<Zone>(SpawnController.NUM_DRONES);
			
			int xSegments = SpawnController.NUM_DRONES / 2;
			int zSegments = 2;
			
			// Segment sizes for the X-Axis
			float xSegmentSize = (MIN_X + MAX_X) / xSegments;
			
			// Segment sizes for the Z-Axis
			float zSegmentSize = (MIN_Z + MAX_Z) / zSegments;
			
			for (int i = 0; i < zSegments; i++)
			{
				for (int j = 0; j < xSegments; j++)
				{
					float minX = MIN_X + (j * xSegmentSize);
					float maxX = minX + xSegmentSize;
					
					float minZ = MIN_Z + (i * zSegmentSize);
					float maxZ = minZ + zSegmentSize;

					Zone zone = new Zone(minX, maxX, minZ, maxZ);
					zones.Add(zone);
				}
			}

			currentZone = zones[0];
			GameObject.FindGameObjectWithTag("ZoneText").guiText.text = "0";
			startZoneSearch = System.DateTime.Now;
		}

		// Update the GUI text
		GameObject rescuedText = GameObject.FindGameObjectWithTag("TotalText");
		rescuedText.guiText.text = friendlySoldierCount.ToString();
	}

	void Update()
	{
		if (droneStrategy == DroneStrategy.ZONES)
		{
			System.TimeSpan timeSpawn = System.DateTime.Now - startZoneSearch;
			if (timeSpawn.TotalSeconds > ZONE_SEARCH_SECONDS)
			{
				// Reset the zone search timer
				startZoneSearch = System.DateTime.Now;

				int currentZoneIndex = zones.IndexOf(currentZone);
				int nextZoneIndex = 0;

				GameObject zoneText = GameObject.FindGameObjectWithTag("ZoneText");

				// move to the next zone
				if (currentZoneIndex == (zones.Count - 1))
				{
					nextZoneIndex = 0;
					currentZone = zones[nextZoneIndex];
				}
				else 
				{
					nextZoneIndex = currentZoneIndex + 1;
					currentZone = zones[nextZoneIndex];
				}

				zoneText.guiText.text = nextZoneIndex.ToString();
			}
		}
	}
}
