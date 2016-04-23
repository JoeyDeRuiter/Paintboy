using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

    public int CurProgress;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(this.gameObject);
        
	}

    void OnLevelWasLoaded() {
        Debug.Log("Load new data");
    }

}
