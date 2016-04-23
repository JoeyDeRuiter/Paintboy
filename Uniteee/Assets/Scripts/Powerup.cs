using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{

    public float health = 0f; // Geeft aantal health terug
    public float speedModifier = 1f;
    public float speedDuration = 2f;
    public float Cooldown = 5f;
	public AudioClip Audio;

    private GameManager GM;

    private Player Player;
    // Use this for initialization
    void Start()
    {
        // Search player
        Player = GameObject.Find("player").GetComponent<Player>();
        GM = GameObject.Find("__GM").GetComponent<GameManager>();
    }


    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
			// Play sound
			Camera.main.GetComponent<CameraController>().PlayAudio(Audio);

            // Set new health
            float newHealth = Player.Health + health;
            Player.Health = Mathf.Clamp(newHealth, 0, 100);

            // Set speed modifier
            Player.ChangeSpeedBuff(speedDuration, speedModifier);

            // Set the cooldown for the timer
            GM.RespawnPowerup(this.gameObject, Cooldown);
            this.gameObject.SetActive(false);
        }
    }


}
