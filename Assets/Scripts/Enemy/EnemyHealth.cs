using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    //public GameObject pounceColliderObject;
    //public GameObject whipColliderObject;
    public float currentHealth = 100f;

    //Collider pounceCollider;
    //Collider whipCollider;
    //bool alreadyPounced = false;
    //bool alreadyWhipped = false;
    //bool inPounceRange = false;
    //bool inWhipRange = false;

	// Use this for initialization
	void Start () {
        //pounceCollider = pounceColliderObject.GetComponent<Collider>();
        //whipCollider = whipColliderObject.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter(Collision other)
    {

    }

    void onCollisionExit(Collision other)
    {

        
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void TakeDamage(float hurt)
    {
        currentHealth -= hurt;
        Debug.Log(currentHealth);
        if (currentHealth <= 0f)
            Die();
    }
}
