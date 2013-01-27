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
	public AnimationClip animBounce;
	public AnimationClip animRun;
	public AnimationClip animJumpDive;
	public AnimationClip animJump;
	
	public float animRunSpeed;
	public float animJumpSpeed;
	
	public float bounceTimer = 5.0f;
	
	public int runDirection=0;
	
	public float fallTimer;
	public float maxFallTime = 0.5f;
	
	public float currentSpeed;
	
	Vector3 lastPosition;
	
	public float minTilt;
	public float maxTilt;
	
	public AudioClip[] audioFallYells;
	public AudioClip[] audioHitYells;
	public AudioClip[] audioHits;
	public AudioClip audioWind;
	
	public bool audioYell = false;
	
	public float worldXMin;
	public float worldXMax;
	
	public bool Win;
	public bool Dead;
	
	// Use this for initialization
	void Awake () {
			
		thisTransform = transform;	
		
		runDirection = 1;
		playerMesh.transform.forward = Vector3.right;
		
	}
	
	// Update is called once per frame
	void Update () {
	
		bool moving = false;
		Vector3 oldPosition = thisTransform.position;
		
		if( !Dead && !Win ) {
			
			
			// Check for collision
			Ray ray = new Ray( thisTransform.position, Vector3.down );
		
			
			
			if( !playerMesh.animation.IsPlaying( animBounce.name ) ) {
				
				// Move left
				if( Input.GetKey( KeyCode.LeftArrow ) ) {
					
					runDirection = 2;
					if( onGround || bJump ) {
					
						thisTransform.position = oldPosition + ((Vector3.left * moveSpeed) * Time.deltaTime);
						
						if( playerMesh.animation.IsPlaying( animIdle.name ) ) {
							playerMesh.animation.CrossFade( animRun.name );
							playerMesh.animation[ animRun.name ].speed = animRunSpeed;
						
						}
						playerMesh.transform.forward = Vector3.right;
						
						moving = true;
						
					} else {
						
						if( fallTimer < maxFallTime ) {
							
							thisTransform.position = oldPosition + ((Vector3.left * moveSpeed) * Time.deltaTime);
							
							
							
							if( playerMesh.animation.IsPlaying( animIdle.name ) ) {
								playerMesh.animation.CrossFade( animRun.name );
								playerMesh.animation[ animRun.name ].speed = animRunSpeed;
								
							}
							playerMesh.transform.forward = Vector3.right;
								
							moving = true;
						} else {
										
							rotateZAccel -= rotateSpeed * Time.deltaTime;
						}
					}			
					
				}
				
				
				
				// Move right
				if( Input.GetKey( KeyCode.RightArrow ) ) {
			
					runDirection = 1;
					
					if( onGround ) {
					
						thisTransform.position = oldPosition + ((Vector3.right * moveSpeed) * Time.deltaTime);
						
						if( playerMesh.animation.IsPlaying( animIdle.name ) ) {
							playerMesh.animation.CrossFade( animRun.name );
							playerMesh.animation[ animRun.name ].speed = animRunSpeed;
							
						} 
						playerMesh.transform.forward = Vector3.left;
				
						moving = true;
						
					} else {
						
						if( fallTimer < maxFallTime ) {
							
							thisTransform.position = oldPosition + ((Vector3.right * moveSpeed) * Time.deltaTime);
							if( playerMesh.animation.IsPlaying( animIdle.name ) ) {
								playerMesh.animation.CrossFade( animRun.name );
								playerMesh.animation[ animRun.name ].speed = animRunSpeed;
								
															
								moving = true;
							} 
							playerMesh.transform.forward = Vector3.left;
						} else {
						
							rotateZAccel += rotateSpeed * Time.deltaTime;
						}
						
					}
				}
			} else {
			
				// Bounce right
				if( runDirection == 1 ) {
					
					thisTransform.position = oldPosition + ((Vector3.right * fallMoveSpeed) * Time.deltaTime);
					DebugMessage( "Bounce right" );
				} else if( runDirection == 2 ) {
				
					thisTransform.position = oldPosition + ((Vector3.left * fallMoveSpeed) * Time.deltaTime);
					DebugMessage( "Bounce left" );
				}
				
			}
			
			// Tilt up
			if( Input.GetKey( KeyCode.UpArrow ) && !onGround ) {
				
				rotateTilt -= tiltSpeed * Time.deltaTime;
				
				if( rotateTilt < minTilt )
					rotateTilt = minTilt;
			}
					
			// Tilt down
			if( Input.GetKey( KeyCode.DownArrow ) && !onGround ) {
			
				rotateTilt += tiltSpeed * Time.deltaTime;
				
				if( rotateTilt > maxTilt )
					rotateTilt = maxTilt;
			}
			
			// Jump
			if( Input.GetKey( KeyCode.Space ) && onGround ) {
				
				if( !bJump ) {
				
					bJump = true;
					jumpTimer = jumpMaxTimer;
					
					playerMesh.animation[ animJump.name ].speed = animJumpSpeed;
					playerMesh.animation.CrossFade( animJump.name );
					
					DebugMessage("Jump Anim" );
				}
			}						
			
			// Make the player jump
			if( bJump ) { 
						
				if( jumpTimer > 0 ) {
									
					jumpTimer -= Time.deltaTime;
					
					thisTransform.position = oldPosition + ((Vector3.up * jumpAccel) * Time.deltaTime);
					
					moving = true;
									
				} else {
					
					bJump = false;
					onGround = false;
				}
			} 
		}
		
		// FALL!
		if( !onGround && !bJump ) {
			
			thisTransform.position += ((Vector3.down * (gravityPull + rotateTilt)) * Time.deltaTime);
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
				
			playerMesh.transform.rotation = Quaternion.Euler( ( onGround ? 0 : -rotateTilt * 10 ), 0, rotateZ );
			thisTransform.Translate( (Vector3.right * moveDirection * fallMoveSpeed) * Time.deltaTime );
			
		} else {
		
			// Rotate player upright on platform
			//playerMesh.transform.rotation = Quaternion.identity;
		}
		
		// Animations
		if( !onGround || bJump ) {
			
			fallTimer += Time.deltaTime;
			
			if( fallTimer > maxFallTime ) {
				
				AudioFallYell();
			
				if( !playerMesh.animation.IsPlaying( animFall.name ) )
					playerMesh.animation.CrossFade( animFall.name );
			}
			
		} else {
						
			fallTimer = 0;
		}
		
		if( !moving && onGround && !bJump ) {
			
			// Idle animation
			
			if( !playerMesh.animation.IsPlaying( animJump.name ) && !playerMesh.animation.IsPlaying( animBounce.name ) ) {
				playerMesh.animation.CrossFade( animIdle.name );
				
				//playerMesh.transform.forward = Vector3.forward;
			}
			
		}

				
		if( bJump || playerMesh.animation.IsPlaying( animJump.name ) ) {
			if( runDirection == 2 )
				playerMesh.transform.forward = Vector3.right;
			else
				playerMesh.transform.forward = Vector3.left;
		}

		
				
	}
	
	void FixedUpdate() {
	
		// Calculate the speed
		currentSpeed = Vector3.Distance( thisTransform.position, lastPosition ) * 100;

		lastPosition = thisTransform.position;
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
	
		if( col.transform.tag == "platform" ) {
			Vector3 point = col.contacts[0].point;
		
			point.x = thisTransform.position.x;
			point.z = thisTransform.position.z;
			thisTransform.position = point;
		}
	}
	
	
	void OnTriggerEnter( Collider hit ) {
	
		//Collider hit = col.collider;
		
		DebugMessage( hit.collider.name );
			
		switch( hit.collider.tag ) {
		
		case "Heart":
			ScoreController.score++;
			Destroy(hit.gameObject);
			
			break;
			
			case "Goal":
				
				DebugMessage("You win");
				Win = true;
				NotificationCenter.DefaultCenter.PostNotification( this, "Win" );
			
				NotificationCenter.DefaultCenter.PostNotification( this, "Win" );
				
				break;
			
			case "ground":
			
				if( !Win ) {
					Dead = true;
			
					NotificationCenter.DefaultCenter.PostNotification( this, "Dead" );
				}
			
				if( !playerMesh.animation.IsPlaying( animBounce.name ) ) {
					
					AudioHitPlatform();
					onGround = true;
					
					rotateZ = 0;
					rotateZAccel = 0;
					rotateTilt = 0;
				
					if( fallTimer > bounceTimer ) {
						
						playerMesh.animation.CrossFade( animBounce.name );
						DebugMessage( "Bounce anim" );
					
						// Bounce right
						if( rotateZ > 0 && rotateZ < 150 ) {
							
							runDirection = 2;
						} else if( rotateZ < 360 && rotateZ > 210 ) {
						
							runDirection = 1;
						} else {
						
							runDirection = 0;
						}
					
					} else {
					
						if( !playerMesh.animation.IsPlaying( animIdle.name ) ) {
							playerMesh.animation.CrossFade( animIdle.name );
							
						}
					}
					
					fallTimer = 0;
				}
							
				break;				
			case "platform":
			
				if( !playerMesh.animation.IsPlaying( animBounce.name ) ) {
					
					AudioHitPlatform();
				
					onGround = true;
					
					rotateZ = 0;
					rotateZAccel = 0;
					rotateTilt = 0;
				
					if( fallTimer > bounceTimer ) {
						
						playerMesh.animation.CrossFade( animBounce.name );
						DebugMessage( "Bounce anim" );
					
						// Bounce right
						if( rotateZ > 0 && rotateZ < 150 ) {
							
							runDirection = 2;
						} else if( rotateZ < 360 && rotateZ > 210 ) {
						
							runDirection = 1;
						} 
					
					} else {
					
						if( !playerMesh.animation.IsPlaying( animIdle.name ) ) {
							playerMesh.animation.CrossFade( animIdle.name );
							
						}
					}
					
					fallTimer = 0;
				}
							
				break;				
			
		}			
	}
	
	void OnTriggerExit( Collider hit ) {
	
		
		//Collider hit = col.collider;
		
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
	
	void AudioFallYell( ) {
		
		if( !audioYell ) {
			audio.Stop();
			int randomInt = Random.Range( 0, audioFallYells.Length );
			
			audio.PlayOneShot( audioWind );
			audio.PlayOneShot( audioFallYells[ randomInt ] );
			audioYell = true;
		}	
	}
	
	void AudioHitPlatform() {
		
		audioYell = false;
		
		audio.Stop();
		
		int randomInt = Random.Range( 0, audioHits.Length );
		int randomInt2 = Random.Range( 0, audioHitYells.Length );
		
		audio.PlayOneShot( audioHits[ randomInt ] );
		audio.PlayOneShot( audioHitYells[randomInt2] );
	}
	

	
}
