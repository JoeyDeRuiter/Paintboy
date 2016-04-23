using UnityEngine;
using System.Collections;


public class Controller2D : MonoBehaviour {

	[HideInInspector]public enum JumpDirections {None, Left, Right};
	public JumpDirections JumpDirection = JumpDirections.None;

	public float Distance = 1f;
	public float DistanceBelow = 1f;
    public float DistanceAbove = 1f;

	public JumpDirections checkForJumpDirection() {

		RaycastHit Hit_right;
		RaycastHit Hit_left;


		// Raycast Left
		if (Physics.Raycast (transform.position, -Vector3.right, out Hit_left)) 
			if(Hit_left.distance <= Distance)
                if (Hit_left.collider.tag != "Pick-Up" && Hit_left.collider.tag != "Transparant" && Hit_left.collider.tag != "TransparantMap")
                {
                    return JumpDirections.Left;
                }
                else {
                    return JumpDirections.None;
                }



		// Raycast Right 
		if (Physics.Raycast (transform.position, Vector3.right, out Hit_right)) 
			if(Hit_right.distance <= Distance)
                if (Hit_right.collider.tag != "Pick-Up" && Hit_right.collider.tag != "Transparant" && Hit_right.collider.tag != "TransparantMap")
                {
                    return JumpDirections.Right;
                }
                else {
                    return JumpDirections.None;
                }



		// If there are no walls
		return JumpDirections.None;

	}

    public bool HitHead() {
        RaycastHit Hit_Above;

        if (Physics.Raycast(transform.position, Vector3.up, out Hit_Above))
            if(Hit_Above.collider.tag != "Pick-Up" && Hit_Above.collider.tag != "Transparant" && Hit_Above.collider.tag != "Player")
                if (Hit_Above.distance <= DistanceAbove)
                    return true;

        return false;
    }

	public bool onGround() {
		RaycastHit Hit_Below;
        if (Physics.Raycast (transform.position, -Vector3.up, out Hit_Below))
            if (Hit_Below.collider.tag != "Pick-Up" && Hit_Below.collider.gameObject.tag != "Transparant" && Hit_Below.collider.tag != "Player")
                if (Hit_Below.distance <= DistanceBelow)
                    return true;
				
		return false;
	}
}
