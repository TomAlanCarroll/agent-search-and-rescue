using UnityEngine;

public class SpawnController : MonoBehaviour {
	public GameObject friendlySoldierPrefab;
	public GameObject dronePrefab;

	// Constants
	public const int NUM_DRONES = 10;


	// Use this for initialization
	void Start () {

		// Initialize friendly soldiers in the buildings
		foreach (GameObject building in GameObject.FindGameObjectsWithTag("BuildingSkeleton"))
		{
			int numSoldiers = Random.Range (10, 15);
			for (int i = 0; i < numSoldiers; i++)
			{
				Vector3 spawnPosition = new Vector3(
					Random.Range (building.renderer.bounds.min.x, building.renderer.bounds.max.x),
					Random.Range (building.renderer.bounds.min.y, 20),
					Random.Range (building.renderer.bounds.min.z, building.renderer.bounds.max.z));

				Instantiate (friendlySoldierPrefab, spawnPosition, Quaternion.identity);
			}
		}

		// Initialize search drones
		Vector3 helicopterPosition = GameObject.FindGameObjectWithTag ("Helicopter").transform.position;

		for (int i = 0; i < NUM_DRONES; i++)
		{			
			Instantiate (dronePrefab, helicopterPosition, Quaternion.identity);
		}
	}
}
