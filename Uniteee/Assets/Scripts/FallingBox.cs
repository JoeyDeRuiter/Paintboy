using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class FallingBox : MonoBehaviour {

    private Player playerScript;
    private Vector3 BoxSpawnPos;
    private float ClimbTimer;
	private Rigidbody rb;
	private bool Collided = false;

    public float Damage = 100f;
    public float WaitDuration = 1f;
    public float BlastRadius = 10f;
	private float ClimbSpeed = 20f;

    /*
    ** Script for letting the box fall and reset
    ** Also damages player
    */

	// Use this for initialization
	void Start () {
        // Get the player script
        playerScript = GameObject.Find("player").GetComponent<Player>();

        // Get the box spawn pos
        BoxSpawnPos = transform.position;

		// Get this object RB
		rb = gameObject.GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update () {
		if (Time.time >= ClimbTimer && Collided) {
            rb.useGravity = false;
			float step = ClimbSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, BoxSpawnPos, step);
		}

        if (transform.position == BoxSpawnPos) {
            Collided = false; // Reset the collider
            StartCoroutine(FallDown(WaitDuration));
        }

    }

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag != "Player") { 
            // On an other object start the timer
            ClimbTimer = Time.time + WaitDuration;
			Collided = true;

            CameraShake(BlastRadius, 0.2f, 0.05f);
        }
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Player") { 
            // Check if it is the player
            // Do a bit of damage to the player
            playerScript.RecieveDmg(Damage);
        }
    }

    IEnumerator FallDown(float _WaitDuration) {
        // Count down to the seconds
        yield return new WaitForSeconds(_WaitDuration);
        // Turn gravity up
        rb.useGravity = true;
    }

    void CameraShake(float radius, float Duration, float Magnitude) {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        // Get all the colliders in range
        for (int i = 0; i < hitColliders.Length; i++){
            // See if player is in range
            if (hitColliders[i].gameObject.tag == "Player")
                // Get camera object to shake the camera
                Camera.main.GetComponent<CameraController>().Shake(Duration, Magnitude);
        }
    }
}