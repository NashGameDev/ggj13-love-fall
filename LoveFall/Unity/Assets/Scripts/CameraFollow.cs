using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public bool allowDebug = true;
	
	public bool isFollowing;
	
	public Transform followObject;
	
	public Vector2 maxBounds;
	public Vector2 minBounds;
	
	public float   moveSpeed;
	public float   maxSpeed;
	public float   camDistance;
	
	public Vector3 movement;
	
	private Transform thisTransform;
	
	// Use this for initialization
	void Awake () {
		
		thisTransform = transform;
	
		if( followObject == null )
			isFollowing = false;
		
	}
	
	// Update is called once per frame
	void Update () {
				
		// Follow the object if the object is farther than the bounds
		if( isFollowing && followObject != null ) {
			
			Vector3 normalDistance = followObject.position - thisTransform.position;
			
			DebugMessage( "X dist: " + normalDistance.x + " Y dist: " + normalDistance.y );
				

			if( normalDistance.y > maxBounds.y || normalDistance.y < minBounds.y ) {
				
				movement.y = followObject.position.y;

			}
			if( normalDistance.x > maxBounds.x || normalDistance.x < minBounds.x ) {
				
				movement.x = followObject.position.x;

			}
			
			moveSpeed = normalDistance.magnitude - camDistance;
			
			if( moveSpeed > maxSpeed )
				moveSpeed = maxSpeed;
			
			movement.z = camDistance;
		
			thisTransform.position = Vector3.Slerp( thisTransform.position, movement, Time.deltaTime * moveSpeed );
		}
		
	}
	
	void DebugMessage( string message ) {
	
		if( allowDebug )
			Debug.Log ( message );
	}
}
