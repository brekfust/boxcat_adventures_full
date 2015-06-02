using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeliboxcatController : MonoBehaviour {

    public float upThrust;
    public float hoverThrust;
    public float thrust;
    public float rotateSpeedMax = 10;
    public float rotateSpeedMin = 3;
    public float stability = 0.3f;
    public float speed = 2.0f;

    //temporary stuff
    public GameObject rope;
    public Text text1;
    public Text text2;

    Rigidbody heliRB;
    Animator heliAnim;
	// Use this for initialization
    void Start()
    {
        heliRB = GetComponent<Rigidbody>();
        heliAnim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float j = Input.GetAxis("Jump");
        float f = Input.GetAxis("Fire1");

        text1.text = heliRB.transform.position.ToString();
        text2.text = transform.rotation.ToString();
        

        heliRB.AddForce(Vector3.up * hoverThrust * Time.deltaTime); //add force so it goes down slowly instead of falling

        Move(h, v);
        LiftAndStabilize(j);
        Animate(j, v);

        if (f > 0)
            DropPayload();
    }

    void Move(float h, float v)
    {
        heliRB.AddRelativeForce(transform.forward * -v * thrust * Time.deltaTime); //move forwards and back. -v? Lol I dunno
        heliRB.AddRelativeTorque(Vector3.forward * thrust * Time.deltaTime * h); //rotate
    }
    void Animate(float j, float v)
    {
        if (j > 0 || Mathf.Abs(v) > 0) //speed up rotor spin if going up and down, or moving forwards and backwards
        {
            heliAnim.speed = 10; //lerp these or something later
        }
        else
        {
            heliAnim.speed = 3;
        }
    }

    //Generic Lerp in coroutine form, use this for animations or whatever later, needs work
    IEnumerator LerpUp(float min, float max, float multiplier)
    {
        float i = 0f;
        while( i < 1 )
        {
            Mathf.Lerp(min, max, Time.deltaTime * multiplier);
            yield return null;
        }

    }
    void LiftAndStabilize(float j)
    {
        if (j > 0)
        {
            heliRB.AddForce(Vector3.up * upThrust * Time.deltaTime); //move up when space bar is pressed
        }
            //some stabilize code I modified from stackexchange. might work on this.    
            Vector3 predictedUp = Quaternion.AngleAxis(
                heliRB.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
                heliRB.angularVelocity
            ) * transform.forward;

                Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
                heliRB.AddTorque(torqueVector * speed * speed);
    }

    void DropPayload()
    {
        rope.SetActive(false);
    }

    
    //add listener for bombcontroller boom event. this won't be called by sendmessage anymore.
    void ExplosionPush(Vector3 bombPos)
    {
        float distance = Vector3.Distance(bombPos, transform.position);
        heliRB.AddExplosionForce(Mathf.Pow(300f - distance,2), bombPos, 300); 
        
        

        //my attempt at doing this, close but not quite there
        //Quaternion rotate = Quaternion.Inverse(Quaternion.LookRotation(bombPos));
        //heliRB.AddTorque(rotate.eulerAngles * distance * 60, ForceMode.Impulse);
        //heliRB.AddForce(rotate.eulerAngles * distance  * 60, ForceMode.Impulse);
        //Debug.Log(rotate.eulerAngles);
        //Debug.Log(distance);
    }
}
