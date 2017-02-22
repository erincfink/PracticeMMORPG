using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject[] players;

	// Use this for initialization
	void Start () {
        if (players.Length > 0) {
            Instantiate(players[0], GameObject.Find("Characters").transform, true);
        }
        else {
            Debug.LogError("There are no player prefabs assigned in GameController");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
