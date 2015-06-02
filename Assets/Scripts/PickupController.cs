using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour {
    bool isTriggered = false;
    Collider player;
    float t = 0f;
    Vector3 startPos;
    Vector3 endPos;
	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isTriggered == true)
        {
            if (endPos.x != player.transform.position.x || endPos.z != player.transform.position.z)
            {
                t = 0.01f; //reset lerp timer if player moves. Allows player to "run" from pickup
                startPos = Vector3.Lerp(transform.position, endPos, .01f);
            }
            endPos = new Vector3(player.transform.position.x, 3.4f, player.transform.position.z);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += .05f;
            if (transform.position.x == player.transform.position.x && transform.position.z == player.transform.position.z)
            { //create child object with another collider and remove this if statement. Silliness.
                PlayerTransform.PickupStatus = true;
                Destroy(gameObject);
            }
        }

	}

    void OnTriggerEnter (Collider other)
    {
        isTriggered = true;
        player = other;
        startPos = new Vector3(transform.position.x, 3.4f, transform.position.z);
        
    }
}
