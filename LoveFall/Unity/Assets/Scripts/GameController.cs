using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public GameObject WinPanel;
	public GameObject DeadPanel;
	
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
		
		WinPanel.SetActive( true );
		restart = true;
		
	}
	
	void Dead() {

		//DeadPanel.animation.Play();
		DeadPanel.SetActive( true );
		restart = true;
	}
}
