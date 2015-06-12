using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public enum AttackType
    {
        ShootFire,
        Swipe,
        InSwipeRange,
		InSwipeDamage,
        Idle
    }

    public Text text1;
    public float speed = 25f;
    public float shootFireWait = .5f;
    public float SwipeWait = .25f;
    public GameObject Fireball;
    public GameObject Player;
    public GameObject Heliboxcat;

    float counter = 0f;
    Animator anim;
    AttackType currentAttackState = AttackType.Idle;
    bool inRange = false;
    bool currentlyAttacking = false;
    bool canWalk = true;
	bool inSwipeDamageRange = false;
    

    // Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        text1.text = currentAttackState.ToString();
        counter += Time.deltaTime;
        //Attack (Or don't) based on currentAttackState and whether the timer for that attack allows it.
        TurnTowardsPlayer();
        switch (currentAttackState)
        {
            case AttackType.ShootFire:
                if (counter >= shootFireWait)
                    StartCoroutine("ShootFire");
                break;
            case AttackType.Swipe:
                float DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
                //check if we're allowed to walk, and not too close already
                if (canWalk)
                {
                    anim.SetBool("isWalking", true);
                    WalkTowardPlayer();
                }
                else
                {
                    anim.SetBool("isWalking", false);
                }
                //must be within striking distance

                break;
            case AttackType.InSwipeRange:
                if (counter >= SwipeWait && !currentlyAttacking)
                {
                    StartCoroutine("Swipe");
                    canWalk = false;
                }
                break;
            case AttackType.Idle:
                break;
            default:
                break;
        }

	}

    void WalkTowardPlayer()
    {
        Vector3 Direction = (Player.transform.position - transform.position).normalized * speed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + Direction);
    }

    void TurnTowardsPlayer()
    {
        if (CameraFollow.currentForm == "Player")
            transform.LookAt(Player.transform.position);
        if (CameraFollow.currentForm == "Heliboxcat")
            transform.LookAt(Heliboxcat.transform.position, Vector3.up);
    }

    //Controls currentAttackState. Only go Idle while exiting shoot range.
    //looks like we don't need isPlayerInside, remove it and simplify this.

    public void AttackState(AttackType type, bool isPlayerInside)
    {
        //toggle swipe damage bool, using same method but kinda separate. So lazy
		if (type == AttackType.InSwipeDamage && isPlayerInside) 
		{
			inSwipeDamageRange = true;
			return;
		}
		if (type == AttackType.InSwipeDamage && !isPlayerInside) 
		{
			inSwipeDamageRange = false;
			return;
		}

		//the actual "state machine"
		currentAttackState = type;
        if (isPlayerInside)
        {
            inRange = true;
        }
        else
        {
            //if isPlayerInside was false, choose which state we should be in
            switch (type)
            {
                case AttackType.InSwipeRange:
                    currentAttackState = AttackType.Swipe;
                    break;
                case AttackType.Swipe:
                    currentAttackState = AttackType.ShootFire;
                    break;
                case AttackType.ShootFire:
                    currentAttackState = AttackType.Idle;
                    break;
            }
            //currentAttackState = AttackType.ShootFire;
            //if (type == AttackType.ShootFire)
            //{
            //    currentAttackState = AttackType.Idle;
            //    inRange = false;
            //}
        }
    }

    //subscribe to the bomb explosion event
    void OnEnable()
    {
        BombController.Boom += ExplosionPush;
    }

    //unsubscribe to the bomb explosion event
    void OnDisable()
    {
        BombController.Boom -= ExplosionPush;
    }
    
    void ExplosionPush(Vector3 bombPos)
    {
        float distance = Vector3.Distance(bombPos, transform.position);
        rigidbody.AddForce(Vector3.up * (Random.Range(75,150) - distance), ForceMode.Impulse);
        //rigidbody.AddExplosionForce(Mathf.Pow(150f - distance, 2)/10f, bombPos, 300f, 3f); 
        //rigidbody.AddExplosionForce(Random.Range(25000,35000), bombPos, 300f);
        rigidbody.AddForce((rigidbody.transform.position - bombPos).normalized * Random.Range(50,150), ForceMode.Impulse);
    }
    
    IEnumerator ShootFire()
    {
        //double check we are not attacking now
        if (!currentlyAttacking)
        {
                anim.SetTrigger("isShooting");
                currentlyAttacking = true;
        }
        else
        {
            yield break;
        }
        yield return null;
    }

    IEnumerator Swipe()
    {
        if (!currentlyAttacking)
        {
            anim.SetTrigger("isSwiping");
            currentlyAttacking = true;
        }
        else
        {
            yield break;
        }
        yield return null;

    }

	IEnumerator SwipeDash()
	{
		//bool forwardDashCompleted = false;
		Vector3 dashDirection = (Player.transform.position - transform.position).normalized;
		int dashRange = 20;
		int i = 0;
		float modifier = 1.5f;
		bool alreadyDamaged = false;
		while (i <= dashRange) 
		{
			rigidbody.MovePosition(transform.position + dashDirection * modifier);
			if (inSwipeDamageRange && !alreadyDamaged)
			{
				alreadyDamaged = true;
				Player.SendMessage("TakeDamage", 10);
			}
			i++;
			yield return null;
		}
		//move one last time to even it up
		//rigidbody.MovePosition (transform.position + Vector3.forward * modifier);
		//yield return null;

	}

    void EndSwipe()
    {
        currentlyAttacking = false;
        canWalk = true;
        counter = 0f;
        Debug.Log("Swipe Ended");
    }
    
    void EndShootFire()
    {
        currentlyAttacking = false;
        counter = 0f;
    }

    void ShootFireball()
    {
        //create fireball, offset puts it above boxwolf
        Vector3 FireballOffset = new Vector3(0, 12.5f, 0);
        GameObject FireballClone = (GameObject)Instantiate(Fireball,
            transform.position + FireballOffset,
            transform.rotation);

        
    }

}
