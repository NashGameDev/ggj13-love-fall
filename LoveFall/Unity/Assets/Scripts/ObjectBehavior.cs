using UnityEngine;
using System.Collections;

public class ObjectBehavior : MonoBehaviour {
	
	public Transform DestinationSpot;
	public Transform OriginSpot;
	public float Speed;
	public bool Switch = false;
	
	void FixedUpdate () {
		//Movement of object between two points
		if(transform.position == DestinationSpot.position) {
			transform.Rotate (Vector3.up * Time.deltaTime, 180);
			Switch = true;
		}
		if(transform.position == OriginSpot.position) {
			transform.Rotate (Vector3.up * Time.deltaTime, 180);
			Switch = false;
		}
		//If it's at the destination, move towards the origin, else move towards the destination.
		if(Switch){
			transform.position = Vector3.MoveTowards(transform.position, OriginSpot.position, Speed);
		}
		else{
			transform.position = Vector3.MoveTowards(transform.position, DestinationSpot.position, Speed);
		}
	}
}