using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

    public PlayerMover.AttackType attackType;
    public float damage;

    private bool inAttackRange = false;

    private GameObject enemy;


    //try this, create struct, create struct array that will hold status info of each collision. coroutine should foreach the array to
    //decide whether to stop looping or not.

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision from " + attackType.ToString() + " to " + other.gameObject.tag + " at " + Time.time);
        if (other.gameObject.CompareTag("Enemy"))
        {
            inAttackRange = true;
            enemy = other.gameObject;
            StartCoroutine("ReadyToSendAttack", enemy);
            Debug.Log("In " + attackType + " Range of " + other.gameObject.name);
        }
    }

    void OnTriggerExit(Collider other)
    {

        //Debug.Log("Left Trigger from " + other.gameObject + " at " + Time.time);
        if (other.gameObject == enemy) //this will not scale. update struct  array described on top instead.
            //also this doesn't seem to be being called. wai? 
        {
            inAttackRange = false;
            Debug.Log("Out of " + attackType + " range of " + other.gameObject.name + enemy.name);
        }

    }

    IEnumerator ReadyToSendAttack(GameObject _enemy)
    {
        bool alreadyAttacked = false;
        bool attackTypeState = false;
        while(inAttackRange == true && _enemy) //may not scale, keeps coroutine alive is targeted enemy exists and is in range
        {
            //poll the correct bool from PlayerMoverscript every frame. Get this bool out of PlayerMover eventually
            if (attackType == PlayerMover.AttackType.Pounce)
                attackTypeState = PlayerMover.isPouncing;

            if (attackType == PlayerMover.AttackType.Whip)
                attackTypeState = PlayerMover.isWhipping;
            
            //take pounce damage if we're attacking, and we haven't already done it this attack
            if (attackTypeState && !alreadyAttacked) 
            {
                _enemy.SendMessage("TakeDamage", damage);
                Debug.Log("attacked " + _enemy.name + Time.time);
                alreadyAttacked = true;
            }
            //
            if (!attackTypeState)
                alreadyAttacked = false;
            yield return null;
        }
        //Debug.Log("Exited " + attackType + " coroutine from " + _enemy.gameObject.name + Time.time);
    }
}
