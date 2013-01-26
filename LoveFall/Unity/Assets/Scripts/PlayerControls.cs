using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	
	public bool allowDebug = true;
	public float rayDist;
	public float radiusDist;
	public LayerMask platformMask;
	
	public bool onGround;
	public bool bJump;
		
	Transform thisTransform;
	
	public float jumpAccel;
	public float jumpTimer;
	public float jumpMaxTimer;
	
	public float gravityPull;
	
	public float inAirMod;
	
	public float moveSpeed;
	
	// Use this for initialization
	void Awake () {
		
		
		thisTransform = transform;	
	}
	
	// Update is called once per frame
	void Update () {
	
		// Check for collision
		Ray ray = new Ray( thisTransform.position, Vector3.down );
	
		DebugRay( ray, rayDist, Color.white );
		
		onGround = false;
		
		RaycastHit[] allHits = Physics.SphereCastAll( ray, radiusDist, rayDist, platformMask );
						
		foreach( RaycastHit hit in allHits ) {
		
			DebugMessage( hit.collider.name );
			
			switch( hit.collider.tag ) {
				
				case "ground":
					// End of game!
				onGround = true;
					break;
				
				case "platform":
					onGround = true;
					break;				
				
			}			
			
		}		
		
		
		// Move left
		if( Input.GetKey( KeyCode.LeftArrow ) ) {
			
			if( onGround || bJump ) {
			
				Vector3 oldPosition = thisTransform.position;
				thisTransform.position = oldPosition + (((Vector3.left * moveSpeed) * Time.deltaTime) * (bJump ? inAirMod : 1));
			}
		}
		
		// Move right
		if( Input.GetKey( KeyCode.RightArrow ) ) {
	
			if( onGround || bJump ) {
			
				Vector3 oldPosition = thisTransform.position;
				thisTransform.position = oldPosition + (((Vector3.right * moveSpeed) * Time.deltaTime) * (bJump ? inAirMod : 1));
			}
		}
		
		// Tilt up
		if( Input.GetKey( KeyCode.UpArrow ) && !onGround ) {
			
		}
				
		// Tilt down
		if( Input.GetKey( KeyCode.DownArrow ) && !onGround ) {
		
			
		}
		
		// Jump
		if( Input.GetKey( KeyCode.Space ) && onGround ) {
			
			if( !bJump ) {
			
				bJump = true;
				jumpTimer = jumpMaxTimer;
			}
		}						
		
		// Make the player jump
		if( bJump ) { 
					
			if( jumpTimer > 0 ) {
				
				Vector3 oldPosition = thisTransform.position;
				
				jumpTimer -= Time.deltaTime;
				
				thisTransform.position = oldPosition + ((Vector3.up * jumpAccel) * Time.deltaTime);
				
			} else {
				
				bJump = false;
			}
		}
		
		// FALL!
		if( !onGround && !bJump ) {
			
			Vector3 oldPosition = thisTransform.position;
			
			thisTransform.position = oldPosition + ((Vector3.down * gravityPull) * Time.deltaTime);
		}

	}
	
	void DebugMessage( string message ) {
	
		if( allowDebug )
			Debug.Log ( message );
	}
	
	void DebugRay( Ray ray, float distance, Color color ) {
	
		if( allowDebug ) {
			Vector3 endPoint = ray.origin + (ray.direction * rayDist );
			Debug.DrawLine( ray.origin, endPoint, color, 0.5f );
		}
	}
}
