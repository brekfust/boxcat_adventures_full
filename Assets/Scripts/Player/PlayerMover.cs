using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {

    public enum AttackType
    {
        Pounce, Whip
    }


    public static bool isPouncing;
    public static bool isWhipping;
    
    public float speed = 6f;
    public float gravity = 5f;
    public float pounceSpeed = 30f;
    public Text text1;
    public Text text2;
    //public GameObject pounceColliderObject;
    //public GameObject whipColliderObject;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
    Collider pounceCollider;
    Collider whipCollider;
    bool canWalk;
    bool isAttacking = false;
    float pounceTimer = 0f;



	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
        //pounceCollider = pounceColliderObject.GetComponent<Collider>();
        //whipCollider = whipColliderObject.GetComponent<Collider>();
        Debug.Log("Changed script!");
	}
	
	// Update is called once per frame
	void Update () {
	
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
        float pounce = Input.GetAxisRaw("Fire1");
        float whip = Input.GetAxisRaw("Jump");

        text1.text = isWhipping.ToString();
        text2.text = isAttacking.ToString();
        //determine if we're allowed to move
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") || isAttacking == true) //let intro animation play before allowing movement
        {
            canWalk = false;
        }
        else
        {
            canWalk = true; 
        }

        if (canWalk == true)
        {
            Move(h, v);
            Turning(h, v);
            WalkingAnimation(h, v, pounce, whip); //always call this before attacking to prevent conflict
            //Gravity(gravity);
            Attacking(pounce, whip);
        }
        pounceTimer += Time.deltaTime; //keep pouncetimer going outside of Attacking() so timer doesn't stop while isAttacking = true

	}

	void Move (float h, float v)
	{
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition(playerRigidbody.position + movement);
	}

    void Turning(float h, float v)
    {
        if (h == 0 && v == 0)
            return;
        
        Vector3 lookSpot;
        lookSpot = new Vector3(h * 10, 0, v * 10); //take movement direction, add 10 to give distance from player

        Quaternion lookDirection = new Quaternion();
        //lookDirection = Quaternion.LookRotation(playerRigidbody.position + lookSpot);
        lookDirection.SetLookRotation(lookSpot);
        lookDirection *= Quaternion.Euler(0f, 90f, 0f); //compensates for original rotation. Mesh was 90 degrees off.
        playerRigidbody.MoveRotation(lookDirection);
    }

    //void Gravity(float gravity)
    //{ //not working, and not needed with rigidbody gravity
    //    Vector3 smoothedGravity = new Vector3(0, -(gravity * Time.deltaTime), 0);
    //    playerRigidbody.MovePosition(smoothedGravity);
    //}

	void WalkingAnimation(float h, float v, float pounce, float whip)
	{
		bool walking = h!= 0f || v != 0f;
		anim.SetBool ("IsWalking", walking);
	}

    void Attacking(float pounce, float whip)
    {
        
        
        
        if (pounce != 0 && isAttacking == false && pounceTimer > 3f)
        {
            pounceTimer = 0f;
            isAttacking = true;
            isPouncing = true;
            anim.SetTrigger("IsPouncing");
            StartCoroutine("Pounce");
        }
        if (whip != 0 && isAttacking == false && isWhipping == false)
        {
            isAttacking = true;
            isWhipping = true;
            anim.SetTrigger("IsWhipping");
            StartCoroutine("Whip");
        }
    }

    IEnumerator Pounce()
    {

        Vector3 actualForward;
        actualForward = -Vector3.Cross(playerRigidbody.transform.up, playerRigidbody.transform.forward); //hack to fix models incorrect rotation
        bool isAnimStarted = false;
        bool stillPouncing = true;
        while(stillPouncing == true)
        {
            AnimatorStateInfo pounceAnim;
            pounceAnim = anim.GetCurrentAnimatorStateInfo(0);
            canWalk = false;

            //move up in the air for half the animation
            Vector3 pounceMovement = actualForward * pounceSpeed * Time.deltaTime;
            if (pounceAnim.normalizedTime < .5f)
                pounceMovement = pounceMovement + Vector3.up * pounceSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + pounceMovement);

            //Can't check for pounce animation right away, so check name and isAnimStarted bool
            if (pounceAnim.IsName("Pounce"))
                isAnimStarted = true;
            if (isAnimStarted && !pounceAnim.IsName("Pounce"))
            {
                isAttacking = false;
                isPouncing = false;
                stillPouncing = false;
            }
            yield return null;
        }
    }

    IEnumerator Whip()
    {
        bool isAnimStarted = false;
        bool stillWhipping = true;
        while (stillWhipping == true)
        {
            AnimatorStateInfo whipAnim;
            whipAnim = anim.GetCurrentAnimatorStateInfo(0);
            canWalk = false;

            if (whipAnim.IsName("TailWhip"))
                isAnimStarted = true;
            if (isAnimStarted && !whipAnim.IsName("TailWhip"))
            {
                isAttacking = false;
                isWhipping = false;
                stillWhipping = false;
            }
            yield return null;
        }
    }

    void ExplosionPush(Vector3 bombPos)
    {
        //old bad code. Fix this.
        float distance = Vector3.Distance(bombPos, transform.position);
        Quaternion rotate = Quaternion.Inverse(Quaternion.LookRotation(bombPos));
        playerRigidbody.AddTorque(rotate.eulerAngles);
        playerRigidbody.AddForce(rotate.eulerAngles * distance * 100000);
    }
}
