using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerTransform : MonoBehaviour {

    public static bool PickupStatus = false; //senses if object is picked up. Set to enum or something cool later
    public Text text1;
    public float transformTimer;
    public GameObject mainPlayer;
    public GameObject heliboxcat;

    Vector3 transformPosition;
    float currentTime = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (PickupStatus)
        {
            transformPosition = mainPlayer.transform.position; //get player position before deactivating to give to the transform
            mainPlayer.SetActive(false);
            heliboxcat.SetActive(true);
            heliboxcat.transform.position = new Vector3(transformPosition.x, transformPosition.y + 6, transformPosition.z); //spawn higher than player because helicat is bigger
            PickupStatus = false;
            CameraFollow.currentForm = "Heliboxcat";
            StartCoroutine(TransformTimer(transformTimer));
        }
	}


    IEnumerator TransformTimer (float time)
    {
        
        while (currentTime <= time)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        transformPosition = heliboxcat.transform.position;
        mainPlayer.SetActive(true);
        //mainPlayer.transform.position = transformPosition; //set player to transforms position. Not wanted right now.
        heliboxcat.SetActive(false);
        CameraFollow.currentForm = "Player";
        currentTime = 0f;

    }
}
