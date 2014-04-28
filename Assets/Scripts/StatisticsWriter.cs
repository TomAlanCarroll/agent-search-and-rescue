using UnityEngine;

public class StatisticsWriter : MonoBehaviour
{
	private static int rescueCount = 0;

	// Use this for initialization
	void Start () {
		
	}

	// Writes statistics to the file
	public static void Found()
	{
		Debug.Log("Found missing soldier #" + SpawnController.foundFriendlySoldierCount + 
		          " of " + SpawnController.friendlySoldierCount);
	}
	public static void Rescued()
	{
		rescueCount++;

		Debug.Log("Rescued missing soldier #" + rescueCount + " of " + SpawnController.friendlySoldierCount);
	}
}

