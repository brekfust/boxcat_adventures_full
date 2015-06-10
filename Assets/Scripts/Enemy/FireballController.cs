using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {

    private bool alreadyDamaged = false; //flag to see if this fireball has already hurt the player
    GameObject Player;
	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("MoveTowardPlayer");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && alreadyDamaged == false)
        {
            other.SendMessage("TakeDamage", 25f);
            Destroy(transform.parent.gameObject);
            
        }
    }

    IEnumerator MoveTowardPlayer()
    {
        //wait 30?? frames until animation is "launching" the fireball
        int i = 0;
        while (i <= 30)
        {
            i++;
            yield return new WaitForEndOfFrame();
        }

        //launch fireball towards player, 20 second life. keep going in same direction within 20 units to give space to dodge.
        float FireballCounter = 0f;
        Vector3 Direction = (Player.transform.position - transform.parent.transform.position).normalized;
        bool stopTrackingPlayer = false;
        while (FireballCounter < 20f)
        {
            if (Vector3.Distance(transform.parent.transform.position, Player.transform.position) > 20 && !stopTrackingPlayer)
            {
                //this .5f is fireball speed. Make this public when you stop being lazy and separate the attack script
                Direction = (Player.transform.position - transform.parent.transform.position).normalized * .5f;
            }
            else
            {
                stopTrackingPlayer = true;
            }
            //this if statment brings fireball down to boxcats level and keeps it there.
            float PlayerHeight = Player.transform.position.y + 4f;
            //Hacky if statment to fix heliboxcat's position. Z is actually Y for heliboxcat.
            //...such a mess. Just rewrite your whole damn game shit.
            if (CameraFollow.currentForm == "Heliboxcat")
                PlayerHeight = Player.transform.position.z +4f;
            if (transform.parent.transform.position.y < PlayerHeight -1f || transform.parent.transform.position.y > PlayerHeight +1f)
            {
                //float x = Direction.x;
                //float y = Direction.y - .25f;
                //float z = Direction.z;
                //Direction = new Vector3(x, y, z);
            }
            else
            {
                float x = Direction.x;
                float y = 0f;
                float z = Direction.z;
                Direction = new Vector3(x, y, z);
            }
            //update position and increment fireball lifetime
            transform.parent.transform.position = (transform.parent.transform.position + Direction);
            FireballCounter += Time.deltaTime;
            yield return null;
        }
        //Destroy(transform.parent.gameObject);
    }
}
