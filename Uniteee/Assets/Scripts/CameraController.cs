using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CameraController : MonoBehaviour {

    public bool Frozen = false;
	private GameObject Player;
	private Vector3 Velocity = Vector3.zero;
    private Vector3 TargetPos;
	private AudioSource Audio;
    // Use this for initialization
    void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		Audio = GetComponent<AudioSource> ();
	}

	void Update () {

        // Smooth camera follow to player
        if (Frozen == false)
           TargetPos = Player.transform.TransformPoint(new Vector3(0 , 3, -20));


		transform.position = Vector3.SmoothDamp(transform.position, TargetPos, ref Velocity, .15F);
	}

    public void ClearDialog() {

        foreach (GameObject _Dialogs in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if(_Dialogs.name == "GUIText")
                Destroy(_Dialogs);
        }
    }

	public void Shake(float Duration, float Magnitude) {
		StartCoroutine(CoShake(Duration, Magnitude));
	}

	public IEnumerator CoShake(float Duration, float Magnitude) {

		float ElaspedTime = 0;

		while(ElaspedTime < Duration)
		{
			ElaspedTime += Time.deltaTime;

			// Calculations for shaking the camera less with time
			float ProComplete = ElaspedTime / Duration;
			float Damper = 1.0f - Mathf.Clamp(4.0f * ProComplete - 3.0f, 0.0f, 1.0f);

			// Get random pos in between[-1, 1] around the camera
			float x = Random.Range(-1, 1);
			float y = Random.Range(-1, 1);

			x *= Magnitude * Damper;
			y *= Magnitude * Damper;

			Camera.main.transform.position = new Vector3(transform.position.x + x, // X axis (Left & Right)
			                                             transform.position.y + y, // Y axis (Up & Down)
			                                             transform.position.z); // Z axis = orignal axis

			yield return null;
		}
	}

	public void PlayAudio(AudioClip AudioClip_) {
		// Play the audio clip one time
		Audio.PlayOneShot (AudioClip_, 1f);
	}
}
