using UnityEngine;
using System.Collections;

public class FB_Enemy : MonoBehaviour {

    public GameObject Ball;
    public float Speed = 1f;
    private float BallY;
    private Vector3 TargetPos;
    private Vector3 velocity = Vector3.zero;
    public float Difficulty = 0.18f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        // Get ball y axis
        BallY = Ball.transform.position.y;

        TargetPos = new Vector3(transform.position.x, Mathf.Clamp(BallY, -6.25f, 6.25f), 0);
        transform.position = Vector3.SmoothDamp(transform.position, TargetPos, ref velocity, Difficulty);
	}
}
