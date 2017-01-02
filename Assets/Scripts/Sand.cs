using UnityEngine;
using System.Collections;

public class Sand : MonoBehaviour {
	
	public enum State {LeftDown, RightDown, LeftUp, RightUp, Horizontal, Vertical, Empty, Rock};
	
	public Material plainMaterial;	
	public Material horizontalMaterial;
	public Material verticalMaterial;
	public Material angledMaterial;
	public Material angledMaterialFlipped;
	
	public State state = State.Empty;
	
	public Sand up;
	public Sand down;
	public Sand left;
	public Sand right;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() {
		if(up)
			Debug.DrawLine(transform.position + Vector3.up * 0.51f, Vector3.Lerp(transform.position, up.transform.position, 0.5f) + Vector3.up * 0.51f, Color.red);
		if(down)
			Debug.DrawLine(transform.position + Vector3.up * 0.51f, Vector3.Lerp(transform.position, down.transform.position, 0.5f) + Vector3.up * 0.51f, Color.green);
		if(left)
			Debug.DrawLine(transform.position + Vector3.up * 0.51f, Vector3.Lerp(transform.position, left.transform.position, 0.5f) + Vector3.up * 0.51f, Color.blue);
		if(right)
			Debug.DrawLine(transform.position + Vector3.up * 0.51f, Vector3.Lerp(transform.position, right.transform.position, 0.5f) + Vector3.up * 0.51f, Color.yellow);
		// Debug.DrawLine (transform.position + Vector3.up * 0.51f, Up().transform.position + Vector3.up * 0.51f, Color.green);
		// Debug.DrawLine (transform.position + Vector3.up * 0.51f, Down().transform.position + Vector3.up * 0.51f, Color.green);
		// Debug.DrawLine (transform.position + Vector3.up * 0.51f, Left().transform.position + Vector3.up * 0.51f, Color.green);
		// Debug.DrawLine (transform.position + Vector3.up * 0.51f, Right().transform.position + Vector3.up * 0.51f, Color.green);
	}
	
	public void Clear (){
		GetComponent<Renderer>().material = plainMaterial;
		state = State.Empty;
	}
	
	public void DropRock (){
		GetComponent<Renderer>().material = plainMaterial;	
		state = State.Rock;
	}
	
	public void RakeHorizontal (){
		GetComponent<Renderer>().material = horizontalMaterial;
		transform.rotation = Quaternion.Euler(0, 0, 0);
		state = State.Horizontal;
	}
	
	public void RakeVertical (){
		GetComponent<Renderer>().material = verticalMaterial;
		transform.rotation = Quaternion.Euler(0, 0, 0);
		state = State.Vertical;
	}
	
	public void RakeAngled (State s){
		state = s;
		switch(state){
			case State.LeftDown:
				GetComponent<Renderer>().material = angledMaterial;
				transform.rotation = Quaternion.Euler(0, 180, 0);
				break;
			case State.RightDown:
				GetComponent<Renderer>().material = angledMaterial;
				transform.rotation = Quaternion.Euler(0, 270, 0);
				break;
			case State.LeftUp:
				GetComponent<Renderer>().material = angledMaterial;
				transform.rotation = Quaternion.Euler(0, 90, 0);
				break;
			case State.RightUp:
				GetComponent<Renderer>().material = angledMaterial;
				transform.rotation = Quaternion.Euler(0, 0, 0);
				break;
		}
	}
	
	public void Link(){
		up = null;
		down = null;
		left = null;
		right = null;
		if((state == State.Vertical || state == State.LeftUp || state == State.RightUp) && Up())
			LinkUp();
		if((state == State.Vertical || state == State.LeftDown || state == State.RightDown) && Down())
			LinkDown();
		if((state == State.Horizontal || state == State.RightDown || state == State.RightUp) && Left())
			LinkLeft();
		if((state == State.Horizontal || state == State.LeftDown || state == State.LeftUp) && Right())
			LinkRight();
	}
	
	public void LinkUp(){
		if(Up().state == State.Vertical || Up().state == State.LeftDown || Up().state == State.RightDown){
			up = Up();
		}
	}
	
	public void LinkDown(){
		if(Down().state == State.Vertical || Down().state == State.LeftUp || Down().state == State.RightUp){
			down = Down();
		}
	}

	public void LinkLeft(){
		if(Left().state == State.Horizontal || Left().state == State.LeftDown || Left().state == State.LeftUp){
			left = Left();
		}
	}
	
	public void LinkRight(){
		if(Right().state == State.Horizontal || Right().state == State.RightDown || Right().state == State.RightUp){
			right = Right();
		}
	}
	
	public Sand Up(){
		return Game.board[((int)transform.position.x)+"x"+((int)transform.position.z + 1)].GetComponent<Sand>();
	}
	
	public Sand Down(){
		return Game.board[((int)transform.position.x)+"x"+((int)transform.position.z - 1)].GetComponent<Sand>();
	}
	
	public Sand Left(){
		return Game.board[((int)transform.position.x - 1)+"x"+((int)transform.position.z)].GetComponent<Sand>();
	}
	
	public Sand Right(){
		return Game.board[((int)transform.position.x + 1)+"x"+((int)transform.position.z)].GetComponent<Sand>();
	}
}
