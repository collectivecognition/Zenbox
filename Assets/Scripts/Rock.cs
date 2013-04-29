using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	private float startHeight = 5.0f;
	private float fallSpeed = 10.0f;
	private bool dustTriggered = false;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 target = new Vector3(transform.position.x, 1.0f, transform.position.z);
		transform.position = Vector3.Lerp (transform.position, target, fallSpeed * Time.deltaTime);
		if(Vector3.Distance(transform.position, target) < 0.2f && !dustTriggered){
			dustTriggered = true;
			GetComponent<ParticleSystem>().Play();
		}
	}
}
