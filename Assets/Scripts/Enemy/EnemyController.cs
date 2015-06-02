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
        rigidbody.AddForce(Vector3.up * (150f - distance), ForceMode.Impulse);
        rigidbody.AddExplosionForce(Mathf.Pow(150f - distance, 2)/10f, bombPos, 300f, 3f); 
        
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

    void FadeInFireball()
    {

    }

    void ShootFireball()
    {

    }
}
