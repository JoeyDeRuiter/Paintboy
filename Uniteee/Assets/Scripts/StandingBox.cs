using UnityEngine;
using System.Collections;

public class StandingBox : MonoBehaviour {

    private Player playerScript;

    void Start() {
        playerScript = GameObject.Find("player").GetComponent<Player>();
    }

    void OnTriggerEnter(Collider coll) {
        if(coll.gameObject.tag == "Player")
            playerScript.RecieveDmg(100f);
    }
}
