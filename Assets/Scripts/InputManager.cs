using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour {

	public GameObject beatBarObject;
	public GameObject[] buttons;
	BeatsBarScript beatBar;

	// Use this for initialization
	void Start () {
		beatBar = beatBarObject.GetComponent<BeatsBarScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		GameObject button = null;
		if (Input.GetKeyDown (KeyCode.A)) {
			button = buttons [0];
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			button = buttons [1];
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			button = buttons [2];
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			button = buttons [3];
		}
		if (button != null) {
			button.GetComponent<Button> ().onClick.Invoke ();
		}
	}

	public void HandleButtonPress(string type) {
		Debug.Log ("InputManager got button " + type);
		switch (type) {
		case "A":
			beatBar.input (KeyAction.Paddle);
			break;
		case "B":
			beatBar.input (KeyAction.Reach);
			break;
		case "C":
			beatBar.input (KeyAction.Twist);
			break;
		case "D":
			beatBar.input (KeyAction.Down);
			break;				
		}
	}
}
