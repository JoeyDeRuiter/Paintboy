using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class FB_UI : MonoBehaviour {

    private FB_GameManager GM;
    private Text CountDown, Score_player, Score_enemy;
    private GameObject Replay_game, Replay_boss;
    private Image Curtain;
    float CurtainTimer = 0;

    // Use this for initialization
    void Start () {
        // Get the game manager
        GM = GameObject.Find("__GM").GetComponent<FB_GameManager>();
        // Get UI components
        CountDown = transform.Find("Countdown").gameObject.GetComponent<Text>();
        Score_player = transform.Find("Score_player").gameObject.GetComponent<Text>();
        Score_enemy = transform.Find("Score_enemy").gameObject.GetComponent<Text>();
        Replay_game = transform.Find("Replay game").gameObject;
        Replay_boss = transform.Find("Replay boss").gameObject;

        Curtain = transform.Find("Curtain").gameObject.GetComponent<Image>();
	}
	// Needed for the curtain
    void Update ()
    {
        // Black curtain at start up
        if (CurtainTimer <= 1)
        {
            // Hide the curtain at start up
            Color color = Color.black;
            color.a = 0;
            CurtainTimer += Time.deltaTime / 1.5f;
            Curtain.GetComponent<Image>().color = Color.Lerp(Color.black, color, CurtainTimer);
        }

        // Restart button Boss/Game
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        { // When the mouse is down
            PointerEventData EvtData = new PointerEventData(EventSystem.current);
            EvtData.position = Input.mousePosition;

            List<RaycastResult> hits = new List<RaycastResult>();

            EventSystem.current.RaycastAll(EvtData, hits);

            foreach (RaycastResult hitObj in hits) {
                // Laad level bij de 
                if (hitObj.gameObject.name == "Replay boss")
                    Application.LoadLevel("final_boss_pong");

                if (hitObj.gameObject.name == "Replay game")
                    Application.LoadLevel("Main_Menu");
            }
        }

    }

    public void StartCountDown() {
        StartCoroutine(coCountDown());
    }

    IEnumerator coCountDown() {
        CountDown.text = "3";
        yield return new WaitForSeconds(1);
        CountDown.text = "2";
        yield return new WaitForSeconds(1);
        CountDown.text = "1";
        yield return new WaitForSeconds(1);
        CountDown.text = "DUEL!";
        yield return new WaitForSeconds(1);
        CountDown.text = ""; // Empty text so it isn't visible anymore
        GM.StartRound(); // Start round
    }

    public void UpdateScores(int Player_score, int Enemy_score) {
        Score_player.text = "Student : " + Player_score.ToString();
        Score_enemy.text = "Leraar : " + Enemy_score.ToString();
    }

    public void SetEndText(string EndText, bool PlayerWin) {

        if (PlayerWin) { // Player win show only the game reset button
            Replay_game.SetActive(true);
        } else { // Player loss show both reset button 
            Replay_boss.SetActive(true);
            Replay_game.SetActive(true);
        }

        CountDown.text = EndText;
    }
}
