using UnityEngine;
using System.Collections;

public class HeartBehavior: MonoBehaviour {
	
	public bool allowDebug = true;

	// Update is called once per frame
	void Update () {
		
		//Rotation
		transform.Rotate(Vector3.up * Time.deltaTime * 150);

		
	}
	/*
	void OnTriggerEnter(Collider collider){
		switch(collider.gameObject.tag){
			case "Player":
			ScoreController.score++;
			Destroy(this.gameObject);
			
			break;
		}
		
	}
	*/
	

	
}
