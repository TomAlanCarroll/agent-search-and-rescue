using UnityEngine;

public class SpawnController : MonoBehaviour {
	public GameObject friendlySoldierPrefab;


	// Use this for initialization
	void Start () {

		// Initializer friendly soldiers in the buildings
		foreach (GameObject building in GameObject.FindGameObjectsWithTag("BuildingSkeleton"))
		{
			int numSoldiers = Random.Range (10, 15);
			for (int i = 0; i < numSoldiers; i++)
			{
				Vector3 spawnPosition = new Vector3(
					Random.Range (building.renderer.bounds.min.x, building.renderer.bounds.max.x),
					Random.Range (building.renderer.bounds.min.y, building.renderer.bounds.max.y),
					Random.Range (building.renderer.bounds.min.z, building.renderer.bounds.max.z));

				Instantiate (friendlySoldierPrefab, spawnPosition, Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
