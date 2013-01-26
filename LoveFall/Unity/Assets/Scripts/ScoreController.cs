using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {
	
	public static int score = 0;
	
	void OnGUI() { 
		string heartScore = "Score: " + score;
		
		GUI.Box (new Rect(Screen.width - 150, 20, 130, 20), heartScore);
	
	}
}
