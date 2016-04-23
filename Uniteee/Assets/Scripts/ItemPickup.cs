using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {

    //declare variables
    private GameObject game;
    private GameManager GM;
    private Player_UI UI;
	public AudioClip Audio;

	// Use this for initialization
	void Start () {
        //Find the Game Manager
        game = GameObject.Find("__GM"); // Zoek de GM ingame
        UI = GameObject.Find("Canvas").GetComponent<Player_UI>();
        if (game != null) {
            GM = game.GetComponent<GameManager>();
        }
	}

    //Check if the player picks up the item
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player")
        {
            // Delete this objective
			Camera.main.GetComponent<CameraController>().PlayAudio(Audio);
            Destroy(this.gameObject);

            // Update overall UI
            UI.Update_progress();
            GM.Count();

            // Objective UI
            int CurrentPickup = -(GM.counter - GM.pick_ups.Length);
            UI.Update_Objective(CurrentPickup, GM.pick_ups.Length);
        }
    }
}
