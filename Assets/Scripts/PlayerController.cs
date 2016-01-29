using UnityEngine;
using System.Collections;
using System;


public class PlayerController : MonoBehaviour {

	public ArrayList danceMoves;
	public GameObject beatBar;
	public GameObject wooweeObject;
	WooeeController woowee;

	// Use this for initialization
	void Start () {
		danceMoves = new ArrayList ();
		woowee = wooweeObject.GetComponent<WooeeController> ();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.A)) {
			HandleButton ("jump");
		}
		if (Input.GetKeyUp(KeyCode.S)) {
			HandleButton ("kick");
		}
    }

	double getMoveAccuracy() {
		// TODO: use the beat bar script
		return 0.75;
	}

	public void HandleButton(string moveType) {
		Debug.Log ("Got button " + moveType);

		DanceMove danceMove = DanceMove.Jump;
		switch (moveType) {
		case "jump":
			danceMove = DanceMove.Jump;
			break;
		case "kick":
			danceMove = DanceMove.Kick;
			break;
		}

		double moveAcc = getMoveAccuracy ();
		doDanceMove (danceMove, moveAcc);
	}

	void animateDanceMove(DanceMove danceMove) {
		
	}

	void doDanceMove(DanceMove danceMove, double accuracy) {
		animateDanceMove (danceMove);
		danceMoves.Add (danceMove);
		woowee.reactToMove (danceMove, accuracy, this);
	}
}
