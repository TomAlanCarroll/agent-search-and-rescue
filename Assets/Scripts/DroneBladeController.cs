using UnityEngine;
using System.Collections;

public class DroneBladeController : MonoBehaviour {
	private float rotationsPerMinute =3000f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,6 * rotationsPerMinute * Time.deltaTime,0);
	}
}
