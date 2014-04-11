using UnityEngine;
using System.Collections;

public class RotorController : MonoBehaviour {
	private float rotationsPerMinute =180f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,0,6 * rotationsPerMinute * Time.deltaTime);
	}
}
