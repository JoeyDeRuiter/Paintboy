using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Player_UI : MonoBehaviour {

    
    public Sprite[] Progress_Sprites;

    private GameObject Progress;
    private int Current_Progress = 0;

    private GameObject Curtain;
    bool ShowCurtain = false; // Let the curtain auto hide on start up
    float CurtainTimer = 0;

    private GameObject Objectives;

    public GameObject DialogParent;
    public AudioClip WritingSound;


	// Use this for initialization
	void Start () {
        Progress = transform.Find("Progress").gameObject;
        Curtain = transform.Find("Curtain").gameObject;
        Objectives = transform.Find("Objectives").gameObject;
        Objectives.SetActive(false); // Set objectives to false, so you don't see it ingame

        // Set the objective
        // Start with 0 and ends with the max pick ups in the map.
        Update_Objective(0, GameObject.FindGameObjectsWithTag("Pick-Up").Length);

        Current_Progress = GameObject.Find("__PlayerData").GetComponent<PlayerData>().CurProgress;
        Set_progress(Current_Progress);
    }

    void Update() {
        // Black curtain when loading/done loading
        if (ShowCurtain == true && CurtainTimer <= 1) { // Fade color to a black screen for loading
            Color color = Color.black;
            color.a = 0;
            CurtainTimer += Time.deltaTime / 1.5f;
            Curtain.GetComponent<Image>().color = Color.Lerp(color, Color.black, CurtainTimer);
        }else
        if(ShowCurtain == false && CurtainTimer <= 1) { // Fade color to transparant
            Color color = Color.black;
            color.a = 0;
            CurtainTimer += Time.deltaTime / 1.5f;





            Curtain.GetComponent<Image>().color = Color.Lerp(Color.black, color, CurtainTimer);
        }

        // Check if the player clicks on the Progress bar to see the objectives
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) { // When the mouse is down
            PointerEventData EvtData = new PointerEventData(EventSystem.current);
            EvtData.position = Input.mousePosition;

            List<RaycastResult> hits = new List<RaycastResult>();

            EventSystem.current.RaycastAll(EvtData, hits);

            foreach (RaycastResult hitObj in hits) {

                // On progress bar
                if (hitObj.gameObject.name == "Progress" && Objectives.activeSelf == false) {
                    Show_Objectives();
                }

                if (hitObj.gameObject.name == "Close_btn")
                    Hide_Objectives();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Objectives.activeSelf == false)
            {
                Show_Objectives();
            }
            else {
                Hide_Objectives();
            }
        }
    
    }
	
    // Updates the progress bar
    public void Update_progress() {
        Current_Progress++;
        Current_Progress = Mathf.Clamp(Current_Progress, 0, Progress_Sprites.Length - 1);
        Progress.GetComponent<Image>().sprite = Progress_Sprites[Current_Progress];  // Set to the new sprite
    }

    public void Set_progress(int progress) {
        Current_Progress = progress;
        Current_Progress = Mathf.Clamp(Current_Progress, 0, Progress_Sprites.Length - 1);
        Progress.GetComponent<Image>().sprite = Progress_Sprites[Current_Progress]; // Set to the right sprite
    }

    private void Show_Objectives() {
        Debug.Log(" -- Play Audio -- ");
        Camera.main.GetComponent<AudioSource>().PlayOneShot(WritingSound, 1f);
        Objectives.SetActive(true); // Show the objectives menu
    }

    private void Hide_Objectives() {
        Objectives.SetActive(false); // Hide the objectives menu
    }

    public void Update_Objective(int CurItemCount, int MaxItemCount) {
        Text UIText = transform.Find("Objectives/Text").gameObject.GetComponent<Text>(); // Get the UI text
        UIText.text = "- Aantal pick-ups gepakt " + CurItemCount + " van de " + MaxItemCount;
        if (CurItemCount == MaxItemCount) {
            UIText.text += "\n- <b>De deur is nu open</b>";
        }
        
    }

    // Shows the loading curtain
    public void Show_Curtain() {
        CurtainTimer = 0; // Reset timer
        ShowCurtain = true;
    }

    // Hides the loading curtain
    public void Hide_Curtian() {
        CurtainTimer = 0; // Reset timer
        ShowCurtain = false;
    }

    public void PrintDialog(string text, float duration = 1f) {
        // To prevent duplication of a Dialog
        if (GameObject.Find("Dialog(Clone)") == null) {
            // Make a new dialog object
            GameObject Dialog = Instantiate(DialogParent, new Vector3(0, 10, 0), Quaternion.identity) as GameObject;
            Text DialogText = Dialog.GetComponent<Text>();
            Dialog.transform.SetParent(transform, false); // Set parent to canvas
            DialogText.text = text; // Set text to input text
            StartCoroutine(DelDialog(Dialog, duration)); // Start the deletion of the object
        }
    }

    private IEnumerator DelDialog(GameObject obj, float duration) {
        yield return new WaitForSeconds(duration);
        Destroy(obj);
    }
}
