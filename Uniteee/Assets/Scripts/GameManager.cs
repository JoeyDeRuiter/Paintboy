using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //declare variables
    public GameObject[] pick_ups;
    public GameObject lift;
    public GameObject Player_death;
    private NextLevel NL;
    public int counter;
    private CameraController mCam;

    public float counterProcent = 0f;

	// Use this for initialization
	void Start () {
        pick_ups = GameObject.FindGameObjectsWithTag("Pick-Up");
        mCam = Camera.main.GetComponent<CameraController>();

        if (lift != null)
        {
            NL = lift.GetComponent<NextLevel>();
        }
        counter = pick_ups.Length;
	}
	
	// Update is called once per frame
	void Update () {
        //Check if the counter is 0
        if (counter == 0)
        {
            // Set the door open
            if(NL != null)
                NL.SetOpen();
            counter++;
        }
	}

    //Subtract from counter
    public void Count()
    {
        counter -= 1;
        //Debug.Log((float)(pick_ups.Length - counter) / pick_ups.Length);
        counterProcent = (float)(pick_ups.Length - counter) / pick_ups.Length;
    }

    public void RespawnPowerup(GameObject item, float cooldown) {
        StartCoroutine(coRespawnPowerup(item, cooldown));
    }

    private IEnumerator coRespawnPowerup(GameObject item, float cooldown) {
        yield return new WaitForSeconds(cooldown);
        item.SetActive(true);
    }

    // Player death
    public void PlayerDie(GameObject Player, Vector3 RespawnPos) {
        StartCoroutine(coPlayerDie(Player, RespawnPos));
    }

    private IEnumerator coPlayerDie(GameObject Player, Vector3 RespawnPos) {
        Player PScript = Player.GetComponent<Player>();
        // Zet de camera op frozen
        mCam.Frozen = true;
        // Clear de buggy Dialog
        mCam.ClearDialog();
        // Zet de player op inactive
        Player.SetActive(false);

        // Maak een nieuwe death animatie
        GameObject DPlayer = Instantiate(Player_death, Player.transform.position - Vector3.forward, Quaternion.identity) as GameObject;
        DPlayer.GetComponent<Rigidbody>().velocity = new Vector3(0, 15, 0);

        // Wacht de death cooldown
        yield return new WaitForSeconds(2.5f);

        // Verwijder de death animatie
        Destroy(DPlayer);

        Player.transform.position = RespawnPos;

        // Give the player script back
        PScript.movement = Vector3.zero;
        PScript.Health = 100f;

        // Zet de player/camera weer active
        Player.SetActive(true);
        mCam.Frozen = false;

    }
}
