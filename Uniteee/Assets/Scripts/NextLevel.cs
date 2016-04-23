using UnityEngine;
using System.Collections;

public class NextLevel : MonoBehaviour {

    //declare variables
    public string levelNaam;
    private bool Elevator = false;
    private bool DoorOpen = true;
    private bool isLoadingNextLevel = false;
    private Player_UI UI;
    private PlayerData PD;
    private GameManager GM;

    void Start() {
        UI = GameObject.Find("Canvas").GetComponent<Player_UI>();
        PD = GameObject.Find("__PlayerData").GetComponent<PlayerData>();
        GM = GameObject.Find("__GM").GetComponent<GameManager>();
    }

    //Check if the player enters the elevator
    void OnTriggerStay(Collider col) {
        if (col.gameObject.tag == "Player" && Elevator == true && Input.GetKey(KeyCode.E) && DoorOpen)
        {
            DoorOpen = false;
            UI.Show_Curtain();
            Debug.Log("De deur gaat magisch open");
            isLoadingNextLevel = true;
            StartCoroutine(coLoadLevel(levelNaam, 1.5f));
        }
        else if (col.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && isLoadingNextLevel == false)
        {
            Debug.Log("De deur zit nog dicht");
            UI.PrintDialog("De deur zit nog dicht!", 2);
        }
    }

    public void loadLevel()
    {
        Debug.Log("Dit is f-ing great");
        StartCoroutine(coLoadLevel(levelNaam, 0f));
    }

    IEnumerator coLoadLevel(string LevelNaam, float Delay) {
        yield return new WaitForSeconds(Delay);
        print("Laad level " + LevelNaam);

        // Sla de data op in PlayerData
        PD.CurProgress += GM.pick_ups.Length;

        Application.LoadLevel(LevelNaam);
        isLoadingNextLevel = true;
    }

    //Set bool to true if all the pick-ups are collected
    public void SetOpen() {
        Elevator = true;
    }
}
