using UnityEngine;
using System.Collections;
using System;


public class PlayerController : MonoBehaviour {

	public ArrayList danceMoves;
	public GameObject wooweeObject;
	WooeeController woowee;

	// Use this for initialization
	void Start () {
		danceMoves = new ArrayList ();
		woowee = wooweeObject.GetComponent<WooeeController> ();
    }
	
	// Update is called once per frame
	void Update () {
    }
		
	void animateDanceMove(DanceMove danceMove) {
		
	}

	public void doDanceMove(DanceMove danceMove, float accuracy) {
		animateDanceMove (danceMove);
		danceMoves.Add (danceMove);
		woowee.reactToMove (danceMove, accuracy, this);
	}
}
