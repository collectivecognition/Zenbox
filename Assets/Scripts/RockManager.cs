using UnityEngine;
using System.Collections;

public class RockManager : MonoBehaviour {
	
	public Transform[] rockPrefabs;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		// Drop a rock (FIXME)
		// if(Input.GetKeyDown (KeyCode.R))
		//	DropRock ();
	}
	
	public void DropRock(){
		Vector3 rockPos = Vector3.zero;
		Transform prefab = rockPrefabs[Random.Range (0, rockPrefabs.Length)];
		GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");
		bool foundSpot = false;
		int maxIterations = 100;
		int curIteration = 0;
		while(!foundSpot && curIteration < maxIterations){
			rockPos = new Vector3(Random.Range (2, Game.width - 2), 0.0f, Random.Range (2, Game.height - 2));
			foundSpot = true;
			for(var ii = 0; ii < rocks.Length; ii++){
				if(Vector3.Distance (rocks[ii].transform.position, rockPos) < 2.5f){
					foundSpot = false;
					break;
				}
			}
			curIteration ++;
		}
		if(curIteration < maxIterations){
			// FIXME: Ugly, should be generic
			switch(prefab.name){
				case "RockBig":
						Instantiate (prefab, new Vector3(rockPos.x + 0.5f, 0.0f, rockPos.z + 0.5f), Quaternion.identity);
						Game.board[rockPos.x+"x"+rockPos.z].SendMessage ("DropRock");
						Game.board[(rockPos.x+1)+"x"+rockPos.z].SendMessage ("DropRock");
						Game.board[rockPos.x+"x"+(rockPos.z+1)].SendMessage ("DropRock");
						Game.board[(rockPos.x+1)+"x"+(rockPos.z+1)].SendMessage ("DropRock");
					break;
				case "RockSmall":
						Instantiate (prefab, new Vector3(rockPos.x, 0.0f, rockPos.z), Quaternion.identity);
						Game.board[rockPos.x+"x"+rockPos.z].SendMessage ("DropRock");
					break;
				case "RockOblong":
						Instantiate (prefab, new Vector3(rockPos.x, 0.0f, rockPos.z + 0.5f), Quaternion.identity);
						Game.board[rockPos.x+"x"+rockPos.z].SendMessage ("DropRock");
						Game.board[rockPos.x+"x"+(rockPos.z+1)].SendMessage ("DropRock");
					break;
				case "RockOblongRotated":
						Instantiate (prefab, new Vector3(rockPos.x + 0.5f, 0.0f, rockPos.z), Quaternion.identity);
						Game.board[rockPos.x+"x"+rockPos.z].SendMessage ("DropRock");
						Game.board[(rockPos.x+1)+"x"+rockPos.z].SendMessage ("DropRock");
					break;
			}
		}	
	}
}
