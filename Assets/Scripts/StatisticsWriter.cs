using UnityEngine;

public class StatisticsWriter : MonoBehaviour
{
	// Use this for initialization
	void Start () {
		
	}

	// Writes statistics to the file
	public static void Write()
	{
		Debug.Log("Found missing soldier #" + SpawnController.foundFriendlySoldiers.Count + 
		          " of " + SpawnController.friendlySoldierCount);
	}
}

