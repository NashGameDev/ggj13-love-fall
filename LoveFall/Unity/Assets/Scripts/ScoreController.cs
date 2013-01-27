using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {
	
	public static int score = 0;
	
	public UILabel scoreLabel;
	
	void Update() {
	
		string heartScore = "" + score;
		scoreLabel.text = heartScore;
		
		
	}
}
