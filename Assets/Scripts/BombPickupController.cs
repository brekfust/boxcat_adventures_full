using UnityEngine;
using System.Collections;

public class BombPickupController : MonoBehaviour
{
    //temporary script, combine with this PickupController and create a generic pickup script

    public GameObject rope;
    public GameObject bomb;

    Collider bombCollider;
    InteractiveCloth ropeCloth;
    bool isTriggered = false;
    Collider player;
    float t = 0f;
    Vector3 startPos;
    Vector3 endPos;
    float clothTempHack;

    // Use this for initialization
    void Start()
    {
        ropeCloth =  rope.GetComponent<InteractiveCloth>();
        bombCollider = bomb.GetComponent<Collider>();
        rope.SetActive(false);
        bomb.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered == true)
        {
            
            //commented out chase script, needs work and wont work for helicopter
            //if (endPos.x != player.transform.position.x || endPos.z != player.transform.position.z)
            //{
            //    t = 0.01f; //reset lerp timer if player moves. Allows player to "run" from pickup
            //    startPos = Vector3.Lerp(transform.position, endPos, .01f);
            //}
            endPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += .05f;
            if (transform.position.x == player.transform.position.x && transform.position.z == player.transform.position.z)
            { //create child object with another collider and remove this if statement. Silliness.
                rope.SetActive(true);
                rope.transform.position = new Vector3(transform.position.x, transform.position.y - 13, transform.position.z);
                //next few lines creates change in physics simulation, otherwise position can't be changed.
                //maybe instantiate ropes instead to avoid this
                clothTempHack = ropeCloth.density;
                ropeCloth.density = 2;
                ropeCloth.density = clothTempHack;
                ropeCloth.AttachToCollider(player, false, true);
                bomb.SetActive(true);
                bomb.transform.position = new Vector3(transform.position.x, transform.position.y - 25, transform.position.z);
                ropeCloth.AttachToCollider(bombCollider, false, true);
                Destroy(gameObject);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        isTriggered = true;
        player = other;
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    }
}