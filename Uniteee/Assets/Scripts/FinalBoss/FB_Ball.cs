using UnityEngine;
using System.Collections;

public class FB_Ball : MonoBehaviour {

    private Rigidbody RB;
    public float MaxSpeed = 25;
    private FB_GameManager GM;

    public AudioClip BallSound;

	// Use this for initialization
	void Start () {
        RB = GetComponent<Rigidbody>();
        GM = GameObject.Find("__GM").GetComponent<FB_GameManager>();

    }
	


    void OnTriggerEnter(Collider coll) {

        if (coll.gameObject.tag == "Map") {
            RB.velocity = new Vector3(RB.velocity.x, -RB.velocity.y, 0);
        }

        if (coll.gameObject.tag == "Player") {
            RB.velocity = new Vector3(Mathf.Clamp(-RB.velocity.x * 1.1f, -MaxSpeed, MaxSpeed),
                                        Mathf.Clamp(RB.velocity.y * Random.Range(0.8f, 1.6f), -15, 15), 0);
            Debug.Log(RB.velocity);
            // Play sound
            Camera.main.GetComponent<AudioSource>().PlayOneShot(BallSound, 1f);
        }

        if (coll.gameObject.name == "Wall_Player")
            GM.NextRound(false);

        if (coll.gameObject.name == "Wall_Enemy")
            GM.NextRound(true);
    }


}
