using UnityEngine;
using System;
using System.IO;
public class StatisticsWriter : MonoBehaviour
{
	public static int rescueCount = 0;

	private static string directory = "Results";
	private static string foundResultFilename;
	private static string rescuedResultFilename;

	private static DateTime startTime;
	private static string startTimeStr;

	// Use this for initialization
	void Start () {
		// Reset all static variables so the scene reset will work correctly
		rescueCount = 0;
		foundResultFilename = null;
		rescuedResultFilename = null;
		startTimeStr = null;

		// Verify the Results directory exists and create it if it doesn't
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}

		// Record the simulation start time
		startTime = DateTime.Now;
		startTimeStr = startTime.ToString("yyyy-MM-dd HHmmss");

		// Setup the filenames
		foundResultFilename = startTimeStr + " " + SpawnController.droneStrategy.ToString() + " Found Results.csv";
		rescuedResultFilename = startTimeStr + " " + HelicopterController.rescueStrategy.ToString() + " Rescued Results.csv";

		// Add column headers to the CSV files
		File.WriteAllText(directory + @"\" + foundResultFilename, "# Found,# Total,Time");
		File.WriteAllText(directory + @"\" + rescuedResultFilename, "# Rescued,# Total,Time");
	}

	// Writes drone statistics to the results file
	public static void Found()
	{
		// Update the GUI text
		GameObject rescuedText = GameObject.FindGameObjectWithTag("FoundText");
		rescuedText.guiText.text = SpawnController.foundFriendlySoldierCount.ToString();

		// Calculate the current simulation time
		TimeSpan timeSpan = DateTime.Now - startTime;

		// Write to the results file
		File.AppendAllText(directory + @"\" + foundResultFilename, 
		                   Environment.NewLine +
		                   SpawnController.foundFriendlySoldierCount + "," +
		                   SpawnController.friendlySoldierCount + "," +
		                   timeSpan.ToString());

		// Log to the console
		Debug.Log("Found missing soldier #" + SpawnController.foundFriendlySoldierCount + 
		          " of " + SpawnController.friendlySoldierCount);
	}

	// Writes helicopter statistics to the results file
	public static void Rescued()
	{
		// Calculate the current simulation time
		TimeSpan timeSpan = DateTime.Now - startTime;

		rescueCount++;
		
		// Update the GUI text
		GameObject rescuedText = GameObject.FindGameObjectWithTag("RescuedText");
		rescuedText.guiText.text = rescueCount.ToString();

		// Write to the results file
		File.AppendAllText(directory + @"\" + rescuedResultFilename, 
		                   Environment.NewLine +
		                   rescueCount + "," +
		                   SpawnController.friendlySoldierCount + "," +
		                   timeSpan.ToString());

		// Log to the console
		Debug.Log("Rescued missing soldier #" + rescueCount + " of " + SpawnController.friendlySoldierCount);

		if (rescueCount == SpawnController.friendlySoldierCount)
		{
			// All soldiers have been rescued
			// Record overall statistics
			File.AppendAllText(directory + @"\" + rescuedResultFilename, 
			                   Environment.NewLine +
			                   Environment.NewLine +
			                   "Average Rescue Time:," +
			                   (timeSpan.TotalSeconds / SpawnController.friendlySoldierCount) + " Seconds");

			// Show the mission complete text
			GameObject missionCompleteText = GameObject.FindGameObjectWithTag("MissionCompleteText");
			missionCompleteText.guiText.pixelOffset = Vector2.zero;

			Application.LoadLevel (Application.loadedLevelName);
		}
	}
}

