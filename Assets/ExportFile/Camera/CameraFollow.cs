using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Player Player;

	// Use this for initialization
	void Start () {
        Player = FindObjectOfType<Player>();



    }
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = Player.transform.position;

    }
}
