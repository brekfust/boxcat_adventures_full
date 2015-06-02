using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

    //event delegate stuff. testing.
    public delegate void Explode(Vector3 bombPos);
    public static event Explode Boom;
    
    public ParticleSystem explosion;
    public float splashRadius = 150f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        //move and play explosion effect
        explosion.transform.position = transform.position;
        explosion.Play();
        //ExplosionDamage(transform.position, splashRadius);
        if (Boom != null)
            Boom(transform.position);
        gameObject.SetActive(false);
    }

    //modified from unity doc example
    //this was ridiculously slow to send to a lot of gameobjects. switched to events.
    //void ExplosionDamage(Vector3 center, float radius) {
    //    Collider[] hitColliders = Physics.OverlapSphere(center, radius);
    //    int i = 0;
    //    while (i < hitColliders.Length) {
    //        hitColliders[i].SendMessage("ExplosionPush", transform.position, SendMessageOptions.DontRequireReceiver);
    //        hitColliders[i].SendMessage("TakeDamage", 90, SendMessageOptions.DontRequireReceiver);
    //        Debug.Log(hitColliders[i].tag);
    //        i++;
    //    }
    //}
}
