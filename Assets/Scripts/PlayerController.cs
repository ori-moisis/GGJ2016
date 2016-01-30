﻿using UnityEngine;
using System.Collections;
using System;


public class PlayerController : MonoBehaviour {

	public ArrayList danceMoves;
	public GameObject wooweeObject;
	WooeeController woowee;
    public GameObject messageObject;
    MessageController messageController;

	// Use this for initialization
	void Start () {
		danceMoves = new ArrayList ();
		woowee = wooweeObject.GetComponent<WooeeController> ();
        messageController = messageObject.GetComponent<MessageController> ();
    }
	
	// Update is called once per frame
	void Update () {
    }
		
	void animateDanceMove(KeyAction danceMove) {
		
	}

	public void doDanceMove(KeyAction danceMove, float accuracy) {
        Debug.Log("Player got dance move:" + danceMove + " accuracy:" + accuracy);
		animateDanceMove (danceMove);
		danceMoves.Add (danceMove);
		woowee.reactToMove (danceMove, accuracy, this);
       // messageController.showRandomOverlayTextOfType(TextOverlayType.Hit);
    }
}
