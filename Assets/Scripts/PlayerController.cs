using UnityEngine;
using System.Collections;
using System;


public class PlayerController : MonoBehaviour {

	public ArrayList danceMoves;
	public GameObject wooweeObject;
	WooeeController woowee;
    public GameObject messageObject;
    MessageController messageController;
    ComboEffect comboEffect;

	public AudioClip failSound;
	public AudioClip missSound;
	public AudioClip hitNoteSound;
	public AudioClip comboSound;


	// Use this for initialization
	void Start () {
		danceMoves = new ArrayList ();
		woowee = wooweeObject.GetComponent<WooeeController> ();
        messageController = messageObject.GetComponent<MessageController> ();
        comboEffect = GetComponentInChildren<ComboEffect>();
    }
	
	// Update is called once per frame
	void Update () {
    }
		
	void animateDanceMove(KeyAction danceMove) {
		GetComponent<Animator> ().SetTrigger (danceMove.ToString ());
	}

	public void doDanceMove(KeyAction danceMove, float accuracy) {
        Debug.Log("Player got dance move:" + danceMove + " accuracy:" + accuracy);
		playSound (danceMove);
		animateDanceMove (danceMove);
		danceMoves.Add (danceMove);
		woowee.reactToMove (danceMove, accuracy, this);
        Debug.Log("isCombo:" + KeyActionHelper.isCombo(danceMove));
        if (KeyActionHelper.isCombo(danceMove)) {
            comboEffect.instantiateEffect();
        }
	}

	public void playSound(KeyAction danceMove) {
		AudioSource audio = GetComponent<AudioSource> ();
		switch (danceMove) {
		case KeyAction.Fail:
			audio.PlayOneShot (failSound);
			break;
		case KeyAction.Miss:
			audio.PlayOneShot (missSound);
			break;
		case KeyAction.BitchCombo:
			audio.PlayOneShot (comboSound);
			break;
		case KeyAction.DoNotStopCombo:
			audio.PlayOneShot (comboSound);
			break;
		case KeyAction.RiseCombo:
			audio.PlayOneShot (comboSound);
			break;
		default:
			audio.PlayOneShot (hitNoteSound);
			break;
		}
	}
}
