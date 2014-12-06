using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour {

	Vector3 rot;

	// Use this for initialization
	void Start () {
	
		rot = new Vector3(Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10));

	}
	
	// Update is called once per frame
	void Update () {
	
		transform.Rotate(rot * Time.deltaTime);

	}
}
