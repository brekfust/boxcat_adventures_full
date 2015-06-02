using UnityEngine;
using System.Collections;

public class CheckForPlayer : MonoBehaviour {



    public EnemyController.AttackType attackType;
    
    EnemyController parentEnemy;

    // Use this for initialization
	void Start () {
        parentEnemy = GetComponentInParent<EnemyController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Heliboxcat")
        {
            parentEnemy.AttackState(this.attackType, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Heliboxcat")
        {
            parentEnemy.AttackState(this.attackType, false);
        }
    }
}
