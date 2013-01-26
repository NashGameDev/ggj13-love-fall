using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class ProjectTools : MonoBehaviour {

	// Creates folders for our project
	[MenuItem ("Project Tools / Make Folders" )]
	
	static void MakeFolders() {
		
		// Cache the project path
		string project_path = Application.dataPath + "/";
		
		// Folders to create
		Directory.CreateDirectory( project_path + "Animations" );
		Directory.CreateDirectory( project_path + "Audio" );
		Directory.CreateDirectory( project_path + "Fonts" );
		Directory.CreateDirectory( project_path + "Materials" );
		Directory.CreateDirectory( project_path + "Meshes" );
		Directory.CreateDirectory( project_path + "Plugins" );
		Directory.CreateDirectory( project_path + "Physics" );
		Directory.CreateDirectory( project_path + "Prefabs" );
		Directory.CreateDirectory( project_path + "Resources" );
		Directory.CreateDirectory( project_path + "Scenes" );
		Directory.CreateDirectory( project_path + "Scripts" );
		Directory.CreateDirectory( project_path + "Shaders" );
		Directory.CreateDirectory( project_path + "Textures" );
		
		// Refresh the assets
		AssetDatabase.Refresh();
		
		Debug.Log ("Folders created!");
	}
	
	// Create prefab from selection
	[MenuItem ("Project Tools / Make Prefab" )]
	
	static void MakePrefab() {
	
		// Grab the list of game objects selected
		GameObject[] activeGOs = Selection.gameObjects;
		
		// Go through each selected game object and create a new prefab
		foreach( GameObject curGO in activeGOs ) {
		
			string localPath = "Assets/" + curGO.name + ".prefab"; 
			
			// Check to see if the asset already exists
			if( AssetDatabase.LoadAssetAtPath( localPath, typeof(GameObject) ) ) {
				
				// Ask to replace the prefab
				if( EditorUtility.DisplayDialog( "Are you sure?", 
												 "The prefab already exists.  Do you want to overwrite it?", 
												 "Yes", "No" ) )
					
					CreateNewPrefab( curGO, localPath );
			} else
				CreateNewPrefab( curGO, localPath );
		}
		
		Debug.Log ("Prefab created!");
	}
	
	// Create new prefab
	static void CreateNewPrefab( GameObject obj, string localPath ) {
	
		// Create the empty prefab
		UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab( localPath );
		
		// Replace it with a object
		PrefabUtility.ReplacePrefab( obj, prefab, ReplacePrefabOptions.ConnectToPrefab );
	}
}

