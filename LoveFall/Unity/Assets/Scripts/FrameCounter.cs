using UnityEngine;
using System.Collections;

public class FrameCounter : MonoBehaviour {
	
	public float fUpdateInterval = 0.5f;
	
	private float fAccumFrames = 0.0f;
	private int	iRenderedFrames = 0;
	private float fTimeLeft;
	
	// Use this for initialization
	void Start () {
	
		// Check to see if a guiText is attach to this component
		if( !guiText ) {
			Debug.Log( "Frame counter requires GUIText component" );
			
			enabled = false;
			return;
		}
		
		// Set the remaining time to the update interval
		fTimeLeft = fUpdateInterval;
	}
	
	// Update is called once per frame
	void Update () {
		
		fTimeLeft -= Time.deltaTime;	// Subtract the delta time
		
		fAccumFrames += Time.timeScale / Time.deltaTime;	// Accumlate the FPS over the interval
		
		iRenderedFrames++;
		
		// If the timer has run out, reset the variables and display the FPS
		if( fTimeLeft <= 0.0f ) {
			
			guiText.text = "FPS: " + (fAccumFrames/iRenderedFrames).ToString("f2");	
			fTimeLeft = fUpdateInterval;
			fAccumFrames = 0.0f;
			iRenderedFrames = 0;
		}
	
	}
}
