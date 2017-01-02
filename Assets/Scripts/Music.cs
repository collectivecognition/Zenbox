using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	
	public AudioClip[] notes;
	float lastTime = 0;
	float nextTime = 0;
	AudioClip lastClip;
	
	// Use this for initialization
	void Start () {
		lastTime = Time.time;
		nextTime = 3.5f; // Random.value * 3.0f + 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - lastTime > nextTime){
			lastTime = Time.time;
			nextTime = 3.5f; // Random.value * 3.0f + 1.0f;
			AudioClip clip = lastClip;
			while(clip == lastClip){
				clip = notes[Random.Range(0, notes.Length)];	
			}
			GetComponent<AudioSource>().PlayOneShot (clip);
			// Debug.Log ("Playing");
			lastClip = clip;
		}
	}
}
