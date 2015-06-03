using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public enum AttackType
    {
        ShootFire,
        Swipe,
        Idle
    }

    public Text text1;
    public Text text2;
    public float shootFireWait = .5f;
    public float SwipeWait = .25f;
    public GameObject Fireball;
    public GameObject Player;

    float counter = 0f;
    Animator anim;
    AttackType currentAttackState = AttackType.Idle;
    bool inRange = false;
    bool currentlyAttacking = false;

    // Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        text2.text = currentlyAttacking.ToString();
        //Attack (Or don't) based on currentAttackState and whether the timer for that attack allows it.
        switch (currentAttackState)
        {
            case AttackType.ShootFire:
                text1.text = "ShootFire";
                if (counter >= shootFireWait)
                    StartCoroutine("ShootFire");
                break;
            case AttackType.Swipe:
                text1.text = "Swipe";
                if (counter >= SwipeWait)
                    StartCoroutine("Swipe");
                break;
            case AttackType.Idle:
                text1.text = "Idle";
                break;
            default:
                break;
        }

	}

    //Controls currentAttackState. Only go Idle while exiting shoot range.
    //looks like we don't need isPlayerInside, remove it and simplify this.
    public void AttackState(AttackType type, bool isPlayerInside)
    {
        currentAttackState = type;
        if (isPlayerInside)
        {
            inRange = true;
        }
        else
        {
            currentAttackState = AttackType.ShootFire;
            if (type == AttackType.ShootFire)
            {
                currentAttackState = AttackType.Idle;
                inRange = false;
            }
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

    void EndSwipe()
    {
        currentlyAttacking = false;
        counter = 0f;
    }
    
    void EndShootFire()
    {
        currentlyAttacking = false;
        counter = 0f;
    }

    IEnumerator ShootFireball()
    {
        //create fireball, offset puts it above boxwolf
        Vector3 FireballOffset = new Vector3(0, 12.5f, 0);
        GameObject FireballClone = (GameObject)Instantiate(Fireball,
            transform.position + FireballOffset,
            transform.rotation);

        //wait 30?? frames until animation is "launching" the fireball
        int i = 0;
        while (i <= 30)
        {
            i++;
            yield return new WaitForEndOfFrame();
        }

        //launch fireball towards player, 20 second life. keep going in same direction within 20 units to give space to dodge.
        float FireballCounter = 0f;
        Vector3 Direction = (Player.transform.position - FireballClone.transform.position).normalized;
        bool stopTrackingPlayer = false;
        while (FireballCounter < 20f)
        {
            if (Vector3.Distance(FireballClone.transform.position, Player.transform.position) > 20 && !stopTrackingPlayer)
            {
                //this .5f is fireball speed. Make this public when you stop being lazy and separate the attack script
                Direction = (Player.transform.position - FireballClone.transform.position).normalized * .5f;
            }
            else
            {
                stopTrackingPlayer = true;
            }
            //this if statment brings fireball down to boxcats level and keeps it there.
            if (FireballClone.transform.position.y >= 4f)
            {
                float x = Direction.x;
                float y = Direction.y - .25f;
                float z = Direction.z;
                Direction = new Vector3(x, y, z);
            }
            else
            {
                float x = Direction.x;
                float y = 0f;
                float z = Direction.z;
                Direction = new Vector3(x, y, z);
            }
            //update position and increment fireball lifetime
            FireballClone.transform.position = (FireballClone.transform.position + Direction);
            FireballCounter += Time.deltaTime;
            yield return null;
        }
        Destroy(FireballClone);
    }

}
