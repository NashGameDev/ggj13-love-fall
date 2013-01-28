using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public GameObject WinPanel;
	public GameObject DeadPanel;
	public GameObject DeadPanel2;
	public GameObject TalkPanel;
	public UILabel	  HerText;
	public UILabel	  HisText;
	public UILabel	  yourText;
	
	public CameraFollow mainCamera;
	public Transform cameraBenchPoint;
	public Transform cameraCloudPoint;
	public Transform player;
	public PlayerControls playerControls;
	
	float restartTimer = 5;
	bool restart = false;
	
	// Use this for initialization
	void Awake () {
	
		NotificationCenter.DefaultCenter.AddObserver( this, "Win" );
		NotificationCenter.DefaultCenter.AddObserver( this, "Dead" );
		
		ScoreController.score = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		if( restart ) {
		
			restartTimer -= Time.deltaTime;
			
			if( restartTimer < 0 )
				Application.LoadLevel( 0 );
		}
			
	}
	
	void Win() {
		
		// WinPanel.animation.Play();
		mainCamera.followObject = cameraBenchPoint;
		mainCamera.maxSpeed = 1;
		
		if( ScoreController.score > 5 )
			WinPanel.SetActive( true );
		else {
			DeadPanel.SetActive( true );
			DeadPanel2.SetActive( true );
		}
		restart = true;
		
		if( ScoreController.score > 15 )
			HerText.text = "I love you! <3";
		else if( ScoreController.score > 10 )
			HerText.text = "Here's my number!!!";
		else if( ScoreController.score > 5 )
			HerText.text = "You're cute ;D";
		else if( ScoreController.score > 1 )
			HerText.text = "Hi...";
		else
			HerText.text = "...";
		
	}
	
	void Dead() {
		
		mainCamera.followObject = cameraBenchPoint;
		mainCamera.maxSpeed = 1;
		
		HerText.text = "... I'm gonna go. Cya";
		
		
		
		//DeadPanel.animation.Play();
		DeadPanel.SetActive( true );
		DeadPanel2.SetActive( true );
		restart = true;
	}
	
	void OpeningCine( int state ) {
	
		switch( state ) {
		
			// Go to bench, show text
			case 1:
			
			mainCamera.followObject = cameraBenchPoint;
			mainCamera.isFollowing = true;
			
			break;
			
			// Show his text
			case 2:
			
			HisText.text = "Hi...";
			
			break;
			
			// Show her text
			case 3:
			
			HerText.text = "...";
			
			break;
			
			// Go to player
			case 4:
			
			mainCamera.followObject = cameraCloudPoint;
			//mainCamera.followObject = player;
			break;

			// Drop player, start game
			case 5:
			
			HisText.text = "";
			HerText.text = "";
			yourText.text = "This guy needs some help...";
			
			mainCamera.followObject = player;
			mainCamera.maxSpeed = 8;
			playerControls.start = false;
			
			break;
		}
	}
}
