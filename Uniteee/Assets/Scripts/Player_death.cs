using UnityEngine;
using System.Collections;

public class Player_death : MonoBehaviour {

    private Animator anim;
    private Rigidbody rb;
	public AudioClip Audio;

	// Use this for initialization
	void Start () {
        anim = transform.Find("sprite").GetComponent<Animator>(); // Take the sprite child of the parent
        rb = GetComponent<Rigidbody>();
		Camera.main.GetComponent<CameraController> ().PlayAudio(Audio); // Play audio als de speler dood gaat
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("Velocity", rb.velocity.y);
	}
}
