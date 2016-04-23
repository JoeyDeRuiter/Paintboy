using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    // Player settings
    public float Health = 100f;
    public float MovementspeedBuff = 1.0f;

	public float MaxMovementspeed = 10f;
	public float MaxWallslidingspeed = 5f;
	public float JumpHeight = 20f;
	public float JumpRate = 0.2f;
    public bool isFacingRight = true;

    public Vector3 lastCheckpoint = Vector3.zero;

    // Jumping variable
	public Vector2 JumpHop = Vector2.zero;
	public Vector2 JumpJump = Vector2.zero;
	[HideInInspector]public bool isInAir = false;
	private float NextJump;
    private float LastJumpVelocity;

    // Fireing variable
    public GameObject VerfKogel;
    public float FireRate = 0.2f;
    private float NextFire = 0f;

	
    // Misc variable
	public Vector3 movement = Vector3.zero;
	private Vector3 velocity = Vector3.zero;
	private CharacterController cc;
	private Controller2D cc2D;
	private CameraController mCam;
    private Animator anim;
    private GameManager GM;
    private bool isWallsliding = false;
    private AudioSource Audio;
    public AudioClip JumpSound;

    void Start () {
		cc = GetComponent<CharacterController> ();
		cc2D = GetComponent<Controller2D> ();
        Audio = GetComponent<AudioSource>();
        mCam = GameObject.FindGameObjectWithTag( "MainCamera" ).GetComponent<CameraController> ();
        GM = GameObject.Find("__GM").GetComponent<GameManager>();
        lastCheckpoint = transform.position;
        anim = transform.Find("sprite").GetComponent<Animator>(); // Take the sprite child of the parent
    }
	
	// Update is called once per frame
	void Update () {
        // Animation
        anim.SetBool("Moving", Input.GetAxis("Horizontal") != 0 ? true : false);
        anim.SetBool("OnGround", cc2D.onGround());
        anim.SetBool("IsSliding", isWallsliding);

        // Animation rotation
        if (Input.GetAxis("Horizontal") < -0.1) {
            isFacingRight = false;
        }else
        if (Input.GetAxis("Horizontal") > 0.1) {
            isFacingRight = true;
        }

        transform.Find("sprite").transform.rotation = Quaternion.Euler(0, 180 * ((isFacingRight == true) ? 0 : 1), 0);

		// Movement
		movement = new Vector3 (velocity.x + (Input.GetAxis( "Horizontal" ) * (MaxMovementspeed * MovementspeedBuff)), movement.y, 0);
		movement = transform.TransformDirection (movement);

		// Gravity
		if (!cc.isGrounded) {
			movement.y += Physics.gravity.y * Time.deltaTime;
		} else {
			movement.y = 0;
		}

		// Velocity
		if(velocity.x >= 1)
		{
			velocity.x -= 60 * Time.deltaTime;
		}else
		if(velocity.x <= -1)
		{
			velocity.x += 60 * Time.deltaTime;
		}else{
			velocity.x = 0;
		}


		// Jumping
		if(Input.GetButton ("Jump")) {
			if(cc.isGrounded && Time.time >= NextJump) { // Normal jump

                // Play jump sound
                Audio.PlayOneShot(JumpSound, 0.03f);
				movement.y = JumpHeight;
				NextJump = Time.time + JumpRate;
			}
		}

		// Landing
        if (!cc2D.onGround()) {
            isInAir = true;
            LastJumpVelocity = movement.y;
        }

		if(cc2D.onGround() && isInAir)
		{
			isInAir = false;
            mCam.Shake(.2f, LastJumpVelocity/100);
        }

        // Walljumping

        // Get what wall we are next to
        string NextToWall = cc2D.checkForJumpDirection().ToString();
        // Confirm if we can do a walljump
        if ((NextToWall == "Right" || NextToWall == "Left") && Input.GetButtonDown ("Jump") && !cc.isGrounded) {

            int JumpMultiplier = (NextToWall == "Right") ? 1 : -1;

            // Check if player can jump
            if (Time.time >= NextJump) {

                // Play jump sound
                Audio.PlayOneShot(JumpSound, 0.03f);

                // Wallhop
                if ((Input.GetAxis("Horizontal") == 1 && NextToWall == "Right") || (Input.GetAxis("Horizontal") == -1 && NextToWall == "Left")) {
                    Debug.Log("Do wall hop");
                    velocity.x = (JumpHop.x * -JumpMultiplier) * MovementspeedBuff;
                    movement.y = JumpHop.y;
                }else
                { // Normal jump
                    Debug.Log("Do normal jump");
                    velocity.x = (JumpJump.x * -JumpMultiplier) * MovementspeedBuff;
                    movement.y = JumpJump.y;
                }

                NextJump = Time.time + JumpRate;
            }
        }

        // Head bump
        if (cc2D.HitHead()) { // Reset the velocity if the head is hit
            movement.y = -0.9f; // Reset velocity
        }


        // Wallsliding
        if ((Input.GetAxis("Horizontal") == 1 && NextToWall == "Right") || (Input.GetAxis("Horizontal") == -1 && NextToWall == "Left") && movement.y < 0 && !cc.isGrounded)
            if (movement.y < -MaxWallslidingspeed)
            {
                movement.y = -MaxWallslidingspeed;
            }

        // Wallsliding for animations
        if ((Input.GetAxis("Horizontal") == 1 && NextToWall == "Right") || (Input.GetAxis("Horizontal") == -1 && NextToWall == "Left"))
             isWallsliding = true;

        if (!((Input.GetAxis("Horizontal") == 1 && NextToWall == "Right") || (Input.GetAxis("Horizontal") == -1 && NextToWall == "Left")))
            isWallsliding = false;


        // Move the player through the character controller
		cc.Move (movement * Time.deltaTime);

        // Click on Left mouse button
        if (Input.GetMouseButton(0) && Time.time >= NextFire)
        {
            NextFire = Time.time + FireRate;
            FirePaint();
        }

    }

    // Make the player fire a bullet
    private void FirePaint() {
        // Calculate the angle of player to mouse
        GameObject Bullet = Instantiate(VerfKogel, transform.position, Quaternion.identity) as GameObject; // Instantiate bullet and add force

        // Calculate mousePos
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos); // Change the mouse pos to a world coordinate

        // Get the delta coords for the angle calc
        float DeltaY = transform.position.y - mousePos.y;
        float DeltaX = transform.position.x - mousePos.x;
        
        // Do the angle calc
        float Angle = Mathf.Atan2(DeltaY, DeltaX) * 180 / Mathf.PI;

        // Apply the angle and force to the bullet
        Vector3 ForceAngle = Quaternion.Euler(0, 0, Angle) * -Vector3.right;
        Bullet.GetComponent<Rigidbody>().AddForce(ForceAngle * 800);
    }

    public void KnockBack(float Force) {
        this.velocity.x = Force;
    }

    // Movementspeed modifier
    public void ChangeSpeedBuff(float Duration, float Modifier) {
        // Start the private method CoChangeSpeedBuff
        StartCoroutine(CoChangeSpeedBuff(Duration, Modifier));
    }

    private IEnumerator CoChangeSpeedBuff(float Duration, float Modifier) {
        // Change the original value without removing the other speed buffs
        MovementspeedBuff *= Modifier;

        yield return new WaitForSeconds(Duration);

        // Set it back to the original value without removing the other speed buffs
        MovementspeedBuff /= Modifier;
    }

    public void RecieveDmg(float Amount) {

        Health -= Amount;

        if (Health <= 0) {
            Debug.Log("Player has died");
            //StartCoroutine(Die());
            GM.PlayerDie(this.gameObject, lastCheckpoint);
        }
    }

    private IEnumerator Die() {
        
        // Play death animation
        // Let the player die and respawn, if out of lives go to main screen
        mCam.Frozen = true; // Freeze camera
        cc.detectCollisions = false; // Disable collision

        // Push the player forward
        Vector3 newPos = transform.position;
        newPos.z = -1;
        //Destroy(gameObject);

        transform.position = newPos; // Push the player of the map
        movement.y = JumpHop.y; // Reset velocity
        yield return new WaitForSeconds(2);

        // Respawn player
        mCam.Frozen = false;
        transform.position = lastCheckpoint; // Set the player to the last saved check point
        cc.detectCollisions = true; // Enable collision
        
    }
}
