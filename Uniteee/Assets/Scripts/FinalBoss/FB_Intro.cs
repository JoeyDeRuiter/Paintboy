using UnityEngine;
using System.Collections;

public class FB_Intro : MonoBehaviour {

    public string[] Dialog;
    private bool isActive = false;
    private Player_UI UI;
	public AudioClip Audio;

    // Use this for initialization
    void Start() {
        UI = GameObject.Find("Canvas").GetComponent<Player_UI>();
    }

    void OnTriggerStay(Collider coll) {
        // Check if player has completed the level, and is in the area

        // Find all pencils
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pick-Up");

        if (coll.gameObject.tag == "Player" && pickups.Length == 0 && Input.GetKey(KeyCode.E) && isActive == false) {
            isActive = true; // Set it so it can only be called once
            Debug.Log(" -- FB_Intro - Dialog -- ");

			Debug.Log(" -- Play audio -- ");
			Camera.main.GetComponent<CameraController>().PlayAudio(Audio);
            StartCoroutine(printDialog(Dialog));

        } else if (Input.GetKey(KeyCode.E) && isActive == false) {
            Debug.Log(" -- FB_Intro - Dialog -- ");
            UI.PrintDialog("De leraar wilt je werk nog niet nakijken", 1);
        }
    }

    IEnumerator printDialog(string[] text) {
        foreach (string text_ in text) {
            Debug.Log(text_);
            UI.PrintDialog(text_, 2);
            yield return new WaitForSeconds(2.1f);
        }

        // Begin het volgende level te laden
        UI.Show_Curtain(); // Wacht todat het curtain er is
        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel("final_boss_pong");
    }
}
