using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class OrbitBehavior : MonoBehaviour {

	public GameObject orbitThis;
	public float speed = 0;
	bool debug = false;
	LineRenderer rend;

	// Use this for initialization
	void Awake () {

		rend = this.GetComponent<LineRenderer>();
		rend.enabled = false;
		if(orbitThis == null){
			orbitThis = GameObject.FindGameObjectWithTag("BlackWhole");
		}
		if(speed == 0){
			speed = Random.Range(-5, 5);
		}

	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(orbitThis.transform.position, Vector3.one, speed * Time.deltaTime);
		Debug.DrawRay(transform.position, -transform.position + orbitThis.transform.position, Color.cyan);
		//debug = GameObject.Find("Player").GetComponent<GravityBody>().debug;
		if(GameObject.Find("Player").GetComponent<GravityBody>().debug){
			rend.enabled = true;
			rend.SetPosition(0, this.transform.position);
			rend.SetPosition(1, orbitThis.transform.position);
		}else{
			rend.enabled = false;
		}
	}
}
