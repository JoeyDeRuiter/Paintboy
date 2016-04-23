using UnityEngine;
using System.Collections;

public class FB_GameManager : MonoBehaviour {

    public GameObject Ball;
    private Rigidbody BallRB;
    private FB_UI UI;

    private int Player_Score = 0 , AI_Score = 0;

	// Use this for initialization
	void Start () {
        UI = GameObject.Find( "Canvas" ).GetComponent<FB_UI>();

        // Start the game
        UI.StartCountDown();
    }
	
    public void StartRound() {
        BallRB = Ball.GetComponent<Rigidbody>();
        BallRB.AddForce(new Vector3(1, 0.2f * (Random.Range(0, 2) * 2 - 1), 0) * 650);
        Debug.Log(Random.Range(0, 2) * 2 - 1);

    }

    public void NextRound(bool Player_point)
    {

        // Add points
        if (Player_point)
            Player_Score++;
        else
            AI_Score++;

        // Update the player scores to the new scores
        UI.UpdateScores(Player_Score, AI_Score);

        // Check if the player wins or if the player loses
        if (Player_Score >= 3) { FinishRound(true); return; }
        if (AI_Score >= 3) { FinishRound(false); return; }

        // Do a new round when there is no winner yet
        RestartRound();
    }

    public void RestartRound() {
        // Reset ball pos
        Ball.transform.position = Vector3.zero;
        BallRB.velocity = Vector3.zero;
        // Start countdown
        UI.StartCountDown();
    }

    public void FinishRound(bool PlayerWin) {
        // Reset ball pos
        Ball.transform.position = Vector3.zero;
        BallRB.velocity = Vector3.zero;

        // Set the end text
        if(PlayerWin)
            // Player heeft gewonnen
            UI.SetEndText("De leraar kijkt je werk na!", PlayerWin);
        else
            // Player heeft verloren
            UI.SetEndText("De leraar weigert je werk na te kijken!", PlayerWin);
    }

    
}
