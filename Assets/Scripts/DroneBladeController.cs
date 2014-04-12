using UnityEngine;
using System.Collections;

public class DroneBladeController : MonoBehaviour {
	private float rotationsPerMinute = 3000f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 verticalAxis = transform.TransformDirection(Vector3.up);
		transform.RotateAround(gameObject.transform.position, verticalAxis, 6 * rotationsPerMinute * Time.deltaTime);
	}
}
