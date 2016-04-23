using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private Rigidbody rb;
    private int BounceCounter;

    public float Distance_Ground = 1f;
    public float Distance_Walls = 0.5f;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.tag == "Map" || coll.tag == "TransparantMap") {
            Vector3 veloCity = rb.velocity;
            BounceCounter++;

            if (BounceCounter == 2)
                Destroy(gameObject);

            // Wall collision - Return bounce
            if (hitWall())
                veloCity.x = -veloCity.x * 0.8f;

            // Floor collision - Return bounce
            if (hitGround())
                veloCity.y = -veloCity.y * 0.8f;

            rb.velocity = veloCity;
        }
    }

    public bool hitGround() {

        RaycastHit Ground_below;
        // Need Vector3.up because Trigger updates too slow
        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out Ground_below))
            if (Ground_below.distance <= Distance_Ground)
                return true;

        return false;
    }

    public bool hitWall() {

        RaycastHit Leftwall;
        RaycastHit Rightwall;

        // Check for left wall
        if (Physics.Raycast(transform.position + (Vector3.right * 2), -Vector3.right, out Leftwall))
            if (Leftwall.collider.tag == "TransparantMap" || Leftwall.collider.tag == "Map")
                if (Leftwall.distance <= Distance_Walls)
                    return true;

        if (Physics.Raycast(transform.position - (Vector3.right * 2), Vector3.right, out Rightwall))
            if (Rightwall.collider.tag == "TransparantMap" || Rightwall.collider.tag == "Map")
                if (Rightwall.distance <= Distance_Walls)
                    return true;


        return false;
    }
}
