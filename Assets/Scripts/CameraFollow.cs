using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public GameObject player;
    public GameObject heliboxcat;

    public static string currentForm = "Player"; //set this back to Player later
	// Use this for initialization
	void Start () {
        //player = GameObject.FindGameObjectWithTag("Player"); 
        //heliboxcat = GameObject.FindGameObjectWithTag("Heliboxcat"); scratch these lines, heliboxcat will be disabled and this won't work
	}
	
	// Update is called once per frame
	void Update () {
        if (currentForm == "Player")
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 18f, player.transform.position.z - 18f);
        if (currentForm == "Heliboxcat")
            transform.position = new Vector3(heliboxcat.transform.position.x, heliboxcat.transform.position.y + 25f, heliboxcat.transform.position.z - 45f);
	}

}
