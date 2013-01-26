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
	public float fallMoveSpeed;
	
	public GameObject playerMesh;
	
	public float rotateZ;
	public float rotateZAccel;
	public float rotateTilt;
	
	public float rotateSpeed;
	public float tiltSpeed;
	
	public AnimationClip animIdle;
	public AnimationClip animFall;

	
	float fallTimer;
	
	
	// Use this for initialization
	void Awake () {
			
		thisTransform = transform;	

	}
	
	// Update is called once per frame
	void Update () {
	
		// Check for collision
		Ray ray = new Ray( thisTransform.position, Vector3.down );
	
		
		// Move left
		if( Input.GetKey( KeyCode.LeftArrow ) ) {
			
			if( onGround || bJump ) {
			
				Vector3 oldPosition = thisTransform.position;
				thisTransform.position = oldPosition + ((Vector3.left * moveSpeed) * Time.deltaTime);
			} else {
				
				//Vector3 oldPosition = thisTransform.position;
				//thisTransform.position = oldPosition + (((Vector3.left * moveSpeed) * Time.deltaTime) * inAirMod);
				
				//rotateZ += (rotateZ + rotateSpeed ) * Time.deltaTime;
				
				rotateZAccel += rotateSpeed * Time.deltaTime;
			}			
			
		}
		
		// Move right
		if( Input.GetKey( KeyCode.RightArrow ) ) {
	
			if( onGround ) {
			
				Vector3 oldPosition = thisTransform.position;
				thisTransform.position = oldPosition + ((Vector3.right * moveSpeed) * Time.deltaTime);
			} else {
				
				//Vector3 oldPosition = thisTransform.position;
				//thisTransform.position = oldPosition + (((Vector3.right * moveSpeed) * Time.deltaTime) * inAirMod);
				
				//rotateZ -= (rotateZ + rotateSpeed ) * Time.deltaTime;
				
				rotateZAccel -= rotateSpeed * Time.deltaTime;
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
		
		// Player rotation
		if( !onGround ) {
			
			rotateZ += rotateZAccel;
			
			if( rotateZ > 360 )
				rotateZ -= 360;
			else if( rotateZ < 0 )
				rotateZ += 360;
			
			
			float moveDirection = 0;
			
			// Move right
			if( rotateZ > 0 && rotateZ < 150 ) {
				
				moveDirection = rotateZ/90.0f;
				
			}
			
			// Move  left
			if( rotateZ < 360 && rotateZ > 210 ) {
			
				moveDirection = -(360.0f-rotateZ)/90.0f;
			}
			
			
			DebugMessage ( "RotateZ: " + rotateZ );
			
			playerMesh.transform.rotation = Quaternion.Euler( 0, 0, rotateZ );
			thisTransform.Translate( (Vector3.right * moveDirection * fallMoveSpeed) * Time.deltaTime );
			
		} else {
		
			// Rotate player upright on platform
			playerMesh.transform.rotation = Quaternion.identity;
		}
		
		// Animations
		
		if( !onGround ) {
			
			fallTimer += Time.deltaTime;
			
			if( fallTimer > 0.5f ) {
			
				if( !playerMesh.animation.IsPlaying( animFall.name ) )
					playerMesh.animation.Blend( animFall.name, 1, 0.1f );
			}
		} else {
			
			fallTimer = 0;
			
			if( !playerMesh.animation.IsPlaying( animIdle.name ) )
				playerMesh.animation.Blend( animIdle.name, 1, 0.1f );
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
	
	void OnCollisionEnter( Collision col ) {
	
		Collider hit = col.collider;
		
		DebugMessage( hit.collider.name );
			
		switch( hit.collider.tag ) {
			
			case "ground":
				// End of game!
				onGround = true;
				
				rotateZ = 0;
				rotateZAccel = 0;
				rotateTilt = 0;
			
				playerMesh.animation.Blend( animIdle.name, 1, 0.1f );
				break;
			
			case "platform":
				onGround = true;
				
				rotateZ = 0;
				rotateZAccel = 0;
				rotateTilt = 0;
			
				playerMesh.animation.Blend( animIdle.name, 1, 0.1f );
				break;				
			
		}			
	}
	
	void OnCollisionExit( Collision col ) {
	
		
		Collider hit = col.collider;
		
		DebugMessage( hit.collider.name );
			
		switch( hit.collider.tag ) {
			
			case "ground":
				onGround = false;
				break;				
		
		
			case "platform":
				onGround = false;
			
				break;				
		
		}
	}
}
