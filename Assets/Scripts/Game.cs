// TODO: Rocks "bouncing"
// TODO: Lighting glitch on torii

// NICE: Proper normal maps for rake trails
// NICE: Better game end
// NICE: Raise / lower rake
// NICE: Animate rake (dust too?)
// NICE: Animate rocks falling (dust)
// NICE: Rotatable board
// NICE: Potato
// NICE: Different game modes / board sizes / difficulties

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	
	public Transform sandPrefab;
	public Transform stonePrefab;
	public Transform rake;
	public Transform highlight;
	public Transform sandContainer;
	public AudioClip gongClip;
	public AudioClip clickClip;
	
	public static int width = 10;
	public static int height = 10;
	
	private bool paused = false;
	private bool help = false;
	private bool won = false;
	
	public static Dictionary<string, Transform> board = new Dictionary<string, Transform>();
	
	void Start () {
		// Build board
		for(int ii = 0; ii < width; ii++){
			for(int jj = 0; jj < height; jj++){
					Transform sand = (Transform)Instantiate(sandPrefab, new Vector3(ii, 0, jj), Quaternion.identity);
					sand.parent = sandContainer;
					board.Add(ii + "x" + jj, sand);
			}
		}
		// Build walls around board
		for(int ii = 0; ii < width; ii++){
			Instantiate (stonePrefab, new Vector3(ii, 0, -1), Quaternion.identity);
			Instantiate (stonePrefab, new Vector3(ii, 0, height), Quaternion.identity);
		}
		for(int ii = -1; ii < height + 1; ii++){
			Instantiate (stonePrefab, new Vector3(-1, 0, ii), Quaternion.identity);
			Instantiate (stonePrefab, new Vector3(width, 0, ii), Quaternion.identity);
		}
		// Position rake
		rake.position = new Vector3(0, 1, 0);
		// Pause game to start
		PauseGame();
	}
	
	// Update is called once per frame
	void Update(){
		if(Input.GetKeyDown (KeyCode.Escape)){
			if(help){
				help = false;
			}else{
				if(paused){
					paused = false;
					UnpauseGame();
				}else{
					paused = true;
					PauseGame ();
				}	
			}
		}
		if(Input.GetMouseButtonDown (0) && help){
			GetComponents<AudioSource>()[1].PlayOneShot(clickClip);
			help = false;
		}
	}
	
	public void PauseGame(){
		paused = true;
		Time.timeScale = 0.0f;
		rake.active = false;
		highlight.active = false;
		Camera.main.GetComponent<ColorCorrectionCurves>().saturation = 0.0f;
	}
	
	void UnpauseGame(){
		paused = false;
		Time.timeScale = 1.0f;
		rake.active = true;
		highlight.active = true;
		Camera.main.GetComponent<ColorCorrectionCurves>().saturation = 1.0f;
	}
	
	void StartGame(){
		UnpauseGame();
		ClearGame();
		// Drop some rocks yo
		GetComponent<RockManager>().DropRock();
		GetComponent<RockManager>().DropRock();
		GetComponent<RockManager>().DropRock();	
	}
	
	void ClearGame(){
		GameObject[] rocks = GameObject.FindGameObjectsWithTag ("Rock");
		foreach(GameObject rock in rocks){
			Destroy (rock);	
		}
		sandContainer.BroadcastMessage("Clear");
	}
	
	public Texture logo;
	public GUISkin newGame;
	public GUISkin tutorial;
	public GUISkin quit;
	public Texture helpImage;
	
	void OnGUI(){
		if(help){
			GUI.skin = null;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), helpImage);
		}else{
			if(paused){
				GUI.BeginGroup(new Rect(Screen.width / 2 - 390 / 2, Screen.height / 2 - 200 / 2 - 50, 390, 248));
					GUI.DrawTexture(new Rect(0, 0, 390, 47), logo);
					GUI.skin = newGame;
					if(GUI.Button(new Rect(0, 70, 390, 52), "")){
						StartGame ();
						GetComponents<AudioSource>()[1].PlayOneShot(clickClip);
					}
					GUI.skin = tutorial;
					if(GUI.Button(new Rect(0, 122, 390, 52), "")){
						help = true;
						GetComponents<AudioSource>()[1].PlayOneShot(clickClip);
					}
					GUI.skin = quit;
					if(GUI.Button(new Rect(0, 196, 390, 52), "")){
						GetComponents<AudioSource>()[1].PlayOneShot(clickClip);
						Application.Quit();
					}	
				GUI.EndGroup();
			}
		}
	}
	
	// Check for end of game
	public void CheckWin(){
		GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");
		// Calculate how many blocks must be in the loop for a win
		int requiredBlocks = Game.width * Game.height;
		for(var ii = 0; ii < rocks.Length; ii++){
			switch(rocks[ii].name){
				case "RockBig(Clone)":
					requiredBlocks -= 4;
					break;
				case "RockSmall(Clone)":
					requiredBlocks -= 1;
					break;
				case "RockOblong(Clone)":
				case "RockOblongRotated(Clone)":
					requiredBlocks -= 2;
					break;
			}
		}
		// Debug.Log (requiredBlocks + " block require to win");
		// Gather blocks
		Dictionary<string, Sand> blocks = new Dictionary<string, Sand>();
		foreach(KeyValuePair<string, Transform> pair in board)
			blocks.Add(pair.Key, pair.Value.GetComponent<Sand>());
		// Debug.Log ("Checking " + blocks.Count + " blocks");
		// Loop until no blocks are pruned in an iteration
		int numRemoved = 999;
		while(numRemoved > 0){
			numRemoved = 0;
			List<string> keysToRemove = new List<string>();
			foreach(KeyValuePair<string, Sand> pair in blocks){
				var links = 0;
				Sand sand;
				if(pair.Value.up && blocks.TryGetValue(((int)pair.Value.up.transform.position.x)+"x"+((int)pair.Value.up.transform.position.z), out sand)) links++;
				if(pair.Value.down && blocks.TryGetValue(((int)pair.Value.down.transform.position.x)+"x"+((int)pair.Value.down.transform.position.z), out sand)) links++;
				if(pair.Value.left && blocks.TryGetValue(((int)pair.Value.left.transform.position.x)+"x"+((int)pair.Value.left.transform.position.z), out sand)) links++;
				if(pair.Value.right && blocks.TryGetValue(((int)pair.Value.right.transform.position.x)+"x"+((int)pair.Value.right.transform.position.z), out sand)) links++;
				if(links <= 1) keysToRemove.Add(pair.Key);
			}
			numRemoved = keysToRemove.Count;
			foreach(string key in keysToRemove){
				blocks.Remove(key);	
			}
		}
		// Highlight closed loops
		foreach(KeyValuePair<string, Transform> pair in board){
			pair.Value.renderer.material.color = Color.white;	
		}
		foreach(KeyValuePair<string, Sand> pair in blocks){
			pair.Value.renderer.material.color = new Color(0.9f, 1.0f, 0.8f, 0.1f);
		}
		// Debug.Log (blocks.Count + " blocks in loops");
		if(blocks.Count == requiredBlocks){
			// Debug.Log("You win!");
			GetComponents<AudioSource>()[1].PlayOneShot(gongClip);
			PauseGame ();
			ClearGame();
			// Application.LoadLevel (Application.loadedLevel);
		}else{
			// Debug.Log("You lose!");
		}
	}
}
