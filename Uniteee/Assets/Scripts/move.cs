using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

	private Player playerScript;
	public float Damage = 100f;

	private int Health = 75;

	public float Distance = 1f;
	public float Speed = 5f;
	private GameObject rayHitObj;
	private string rayHitString;

	private bool moveRight = false;

	void Start () {
		playerScript = GameObject.Find("player").GetComponent<Player>();
	}

	// Update is called once per frame
	void Update () {

		RaycastHit hit_left;
		RaycastHit hit_right;

		// Left hit
		if (Physics.Raycast (transform.position, Vector3.left, out hit_left))
			if (hit_left.distance <= Distance)
				if (hit_left.collider.tag == "Map")
					moveRight = true;

		if (Physics.Raycast (transform.position, -Vector3.left, out hit_right))
			if (hit_right.distance <= Distance)
				if (hit_right.collider.tag == "Map")
					moveRight = false;



		if (moveRight) {
			Move_Right();
		} else {
			Move_Left();
		}

	}

	void Move_Left() {
		// Turn character and move left
		transform.rotation = Quaternion.Euler (0, 0, 0);
		transform.position += (Vector3.left * Speed) * Time.deltaTime;
	}

	void Move_Right() {
		// Turn character and move right
		transform.rotation = Quaternion.Euler (0, 180, 0);
		transform.position += (-Vector3.left * Speed) * Time.deltaTime;
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Player") { 
			// Check if it is the player
			// Do a bit of damage to the player
			playerScript.RecieveDmg(Damage);
		}
		if (coll.gameObject.tag == "Transparant") {
			Destroy(coll.gameObject);
			Health -= 25;

			if (Health <= 0) {
				Destroy(gameObject);
			}
		}
	}
}