﻿using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public GameObject beatBarObject;
	BeatsBarScript beatBar;

	// Use this for initialization
	void Start () {
		beatBar = beatBarObject.GetComponent<BeatsBarScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		string button = null;
		if (Input.GetKeyUp (KeyCode.A)) {
			button = "A";
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			button = "B";
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			button = "C";
		}
		if (Input.GetKeyUp (KeyCode.F)) {
			button = "D";
		}
		if (button != null) {
			HandleButtonPress (button);
		}
	}

	public void HandleButtonPress(string type) {
		Debug.Log ("InputManager got button " + type);
		// TODO: send input to the beat bar

		beatBar.input(OurCoolKey.A);
	}
}