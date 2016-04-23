using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class Dialog : MonoBehaviour {

    public string DialogText;

    private GameObject GameObject;
    private Vector3 viewPos;
    private GUIText GUI;

    void OnTriggerEnter(Collider coll) {

        // If the collider is a player so it doesn't spawn on any other object
        if (coll.tag == "Player") {
            // Make a new object to hold the text
            GameObject = new GameObject("GUIText");
            GUI = GameObject.AddComponent<GUIText>();

            // Set the text and style
            GUI.text = DialogText;
            GUI.alignment = TextAlignment.Center;
            GUI.anchor = TextAnchor.MiddleCenter;
        }
    }

	void OnTriggerStay(Collider coll) {

        // Check if it is the player
        if (coll.tag == "Player")
            // If the object is set
            if(GameObject != null)
                // Set text pos relative to the player camera
                GameObject.transform.position = Camera.main.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y + 1, 0));
    }

	void OnTriggerExit(Collider coll) {

        // Check if it is the player
		if(coll.tag == "Player")
            // If the object is set
        	if (GameObject != null)
           		// Destroy the object
            	Destroy(GameObject);
    }
}
