using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {

	
	GravityAttractor planet;
	public GameObject[] planets;
	public GameObject mainPlanet;
	public float biggestAttraction = 0;
	public Vector3 motion;
	CharacterController controller;
	Vector3 GravMotion;
	public float jumpPower;
	public Transform grounded;
	public bool g;
	Animator anim;
	public bool paused;
	public float lel;
	float dist;
	public GameObject model;
	public float angle = 0;
	public GameObject camera;
	Vector3 jumpMotion;
	public bool debug = false;
	public GameObject debugCam;

	public bool x;
	public float lookSensitivityX = 250f;
	public float lookSensitivityY = 250f;
	public Transform cameraT;
	float verticalLookRotation;
	public float horizontalLookRotation;

	void Awake(){
		controller = this.GetComponent<CharacterController>();
		anim = this.GetComponentInChildren<Animator>();
		debugCam.gameObject.SetActive(false);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Q)){
			debug = !debug;
		}

		if(Input.GetKeyDown(KeyCode.Tab)){
			paused = !paused;
		}
		if(!paused){
			Screen.lockCursor = true;
		}else{
			Screen.lockCursor = false;
		}

		biggestAttraction = 0;
		planets = GameObject.FindGameObjectsWithTag("Planet");

		foreach (GameObject plan in planets){
			Vector3 targetDir1 = (transform.position - plan.transform.position).normalized;
			RaycastHit hit;
			if(Physics.Raycast(grounded.position,-targetDir1, out hit, 1000)){
				if(hit.transform.gameObject.GetInstanceID() == plan.gameObject.GetInstanceID()){
					dist = hit.distance+10;
				}else{
					dist = Vector3.Distance(transform.position,plan.transform.position)+10;
				}
			}else{
				dist = Vector3.Distance(transform.position,plan.transform.position)+10;
			}
			float grav = plan.gameObject.GetComponent<GravityAttractor>().gravity*50/Mathf.Pow(dist,2);
			Debug.DrawRay(transform.position, -targetDir1*dist, new Color(1-(grav/lel),(grav/lel),0));
			if(debug){
				debugCam.gameObject.SetActive(true);
			}else{
				debugCam.gameObject.SetActive(false);
			}
			if (Input.GetButton("GravBoots")) {
				GravMotion -= targetDir1* grav *10;
			}else{
				GravMotion -= targetDir1* grav;
			}

			if(grav > biggestAttraction){
				biggestAttraction = grav;
				mainPlanet = plan;
			}
		}

		transform.parent = mainPlanet.transform;

		//CameraControll();

		Movement();
	}

	void Movement(){

		if(Screen.lockCursor == true){
			verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * lookSensitivityY;
			verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90,90);
			cameraT.localEulerAngles = Vector3.left * verticalLookRotation;
			Vector3 targetDir = (transform.position - mainPlanet.transform.position).normalized;
			Vector3 bodyp = transform.up;
			Quaternion rott;
			 //rott = Quaternion.FromToRotation(bodyp, -GravMotion) * transform.rotation;
			if(Grounded()){
				rott = Quaternion.FromToRotation(bodyp, targetDir) * transform.rotation;
			}else{
				rott = Quaternion.FromToRotation(bodyp, -GravMotion) * transform.rotation;
			}
			transform.rotation = Quaternion.Slerp(transform.rotation, rott,0.1f);
		}

		if (Input.GetButtonDown ("Jump") && Grounded ()) {
			jumpMotion = transform.TransformDirection(Vector3.up*jumpPower);
		} else if (Grounded ()) {
			g = true;
			jumpMotion = Vector3.zero;
			GravMotion = Vector3.zero;
		} else {
			g = false;
		}

		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
		anim.SetFloat("Speed",input.magnitude*8);	

		if(input != Vector3.zero){
			horizontalLookRotation = Input.GetAxis("Mouse X") * Time.deltaTime * lookSensitivityX;
			transform.Rotate(Vector3.up * horizontalLookRotation);
			Quaternion targetRotation = Quaternion.LookRotation(input);
			model.transform.localEulerAngles = Vector3.up * Mathf.MoveTowardsAngle (model.transform.localEulerAngles.y, targetRotation.eulerAngles.y , 200 * Time.deltaTime);
		}else{
			if(Screen.lockCursor == true){
				horizontalLookRotation += Input.GetAxis("Mouse X") * Time.deltaTime * lookSensitivityX;
				cameraT.localEulerAngles += Vector3.up * horizontalLookRotation;
			}
		}

		motion = model.transform.TransformDirection(Vector3.forward*input.magnitude);

		motion += (GravMotion * Time.deltaTime) + (jumpMotion * Time.deltaTime);

		rigidbody.velocity = (motion * Time.deltaTime  * 400);

		//jumpMotion = Vector3.zero;
	
	}

	public bool Grounded(){
		if (Physics.Raycast (new Vector3 (grounded.position.x, grounded.position.y, grounded.position.z), transform.TransformDirection(Vector3.down), 0.6f)) {
			return true;
		}
		return false;
	}


}
























