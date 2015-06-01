using UnityEngine;
using UnityEditor;
using System.Collections;

public class TestRevert : Editor {

    [MenuItem ("Tools/Revert to Prefab %r")]

 static void Revert() {

     GameObject[] selection = Selection.gameObjects;
        
     if (selection.Length > 0) {
         for (int i = 0; i < selection.Length; i++) {
             //PrefabUtility.ResetToPrefabState(selection[i]);
             PrefabUtility.RevertPrefabInstance(selection[i]);
             Debug.Log("Attempted reset");
             
         }
     } else {
         Debug.Log("Cannot revert to prefab - nothing selected");
     }
 }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
