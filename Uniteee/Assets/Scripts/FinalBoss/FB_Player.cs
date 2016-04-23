using UnityEngine;
using System.Collections;

public class FB_Player : MonoBehaviour {

    public float Speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float Input_ = Input.GetAxis("Vertical");

        // Set pos
        transform.position += (Vector3.up * Speed) * Input_;

        // Lock max pos
        float yPos = Mathf.Clamp(transform.position.y, -6.25f, 6.25f);
        transform.position = new Vector3(-13, yPos, 0);
	}
}
