using UnityEngine;
using System.Collections;

public class Rake : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	enum Direction {None, Up, Down, Left, Right};
	private Direction lastDirection = Direction.None;
	private Direction direction = Direction.None;
	private Vector2 oldMousePosition;
	private float turnSpeed = 30.0f;
	private Quaternion targetRotation = Quaternion.identity;
	public Transform highlight;
	public Transform sandContainer;
	public Transform game;
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector2 mousePosition = Vector3.zero;
		bool hovering = false;
		if(Physics.Raycast(ray, out hit, 100)){
			if(hit.collider.gameObject.GetComponent<Renderer>() && hit.collider.gameObject.name == "Sand(Clone)"){
				mousePosition = new Vector2(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.z);
				hovering = true;
				highlight.transform.position = hit.collider.gameObject.transform.position;
				// hit.collider.gameObject.renderer.material.color = Color.white;
			}
		}
		// Rotate rake
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation , turnSpeed * Time.deltaTime);
		if(hovering){
			transform.position = hit.point; // new Vector3(mousePosition.x, 1, mousePosition.y);
			if(mousePosition.x > oldMousePosition.x){
				targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 270, 0);
				direction = Direction.Right;
			}
			if(mousePosition.x < oldMousePosition.x){
				targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, 0);
				direction = Direction.Left;
			}
			if(mousePosition.y > oldMousePosition.y){
				targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180, 0);
				direction = Direction.Up;
			}
			if(mousePosition.y < oldMousePosition.y){
				targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
				direction = Direction.Down;
			}
			if(Input.GetMouseButton (0) && lastDirection != Direction.None && mousePosition != oldMousePosition && Vector2.Distance(mousePosition, oldMousePosition) < 2.0f){
				/*
				if(Game.board[mousePosition.x+"x"+mousePosition.y].GetComponent<Sand>().state != Sand.State.Rock){
					// Horizontal / vertical lines
					if(direction == Direction.Left || direction == Direction.Right)
						Game.board[mousePosition.x+"x"+mousePosition.y].SendMessage("RakeHorizontal");
					else
						Game.board[mousePosition.x+"x"+mousePosition.y].SendMessage("RakeVertical");
				}
				*/
				if(Game.board[oldMousePosition.x+"x"+oldMousePosition.y].GetComponent<Sand>().state != Sand.State.Rock){
					// Horizontal lines
					if((direction == Direction.Right || direction == Direction.Left) && (lastDirection == Direction.Right || lastDirection == Direction.Left))
						Game.board[oldMousePosition.x+"x"+oldMousePosition.y].SendMessage ("RakeHorizontal");
					// Vertical lines
					if((direction == Direction.Up || direction == Direction.Down) && (lastDirection == Direction.Up || lastDirection == Direction.Down))
						Game.board[oldMousePosition.x+"x"+oldMousePosition.y].SendMessage("RakeVertical");
					// Angled lines:
					// -- Left/Down
					// |
					if((direction == Direction.Right && lastDirection == Direction.Up) || (direction == Direction.Down && lastDirection == Direction.Left))
						Game.board[oldMousePosition.x+"x"+oldMousePosition.y].SendMessage ("RakeAngled", Sand.State.LeftDown);
					// -- Right/Down
					//  |
					if((direction == Direction.Left && lastDirection == Direction.Up) || (direction == Direction.Down && lastDirection == Direction.Right))
						Game.board[oldMousePosition.x+"x"+oldMousePosition.y].SendMessage ("RakeAngled", Sand.State.RightDown);
					// |  Left/Up
					// --
					if((direction == Direction.Right && lastDirection == Direction.Down) || (direction == Direction.Up && lastDirection == Direction.Left))
						Game.board[oldMousePosition.x+"x"+oldMousePosition.y].SendMessage ("RakeAngled", Sand.State.LeftUp);
					//  | Right/Up
					// --
					if((direction == Direction.Left && lastDirection == Direction.Down) || (direction == Direction.Up && lastDirection == Direction.Right))
						Game.board[oldMousePosition.x+"x"+oldMousePosition.y].SendMessage ("RakeAngled", Sand.State.RightUp);
				}
			}
			if(Input.GetMouseButton (1)){
				if(Game.board[mousePosition.x+"x"+mousePosition.y].GetComponent<Sand>().state != Sand.State.Rock){
					Game.board[mousePosition.x+"x"+mousePosition.y].SendMessage("Clear");
				}
			}
			lastDirection = direction;
			sandContainer.BroadcastMessage("Link");
			game.GetComponent<Game>().CheckWin ();
		}
		oldMousePosition = mousePosition;
	}
}
