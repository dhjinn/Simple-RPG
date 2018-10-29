using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	public Interactable focus;
	public LayerMask movementMask;
	Camera cam;
	PlayerMotor motor;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		motor = GetComponent<PlayerMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit, 100, movementMask))
			{
				//Move player to hit 
				//Stop focusing any objects
				motor.MoveToPoint(hit.point);
				RemoveFocus();
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit, 100))
			{
				//Check if hit interactable, focus on it
				Interactable interactable = hit.collider.GetComponent<Interactable>();
				if (interactable != null){
					SetFocus(interactable);
				}
			}
		}
	}

	void SetFocus(Interactable newFocus) {
		if (newFocus != focus) {
			if (focus != null) {
				focus.OnDefocused();
			}
			focus = newFocus;
			newFocus.OnFocused(transform);
		}
		
		motor.FollowTarget(newFocus);
	}

	void RemoveFocus() {
		if (focus != null) {
			focus.OnDefocused();
		}
		focus = null;
		motor.StopFollowingTarget();
	}
}
