using UnityEngine;
using System.Collections;
using System;


public class PlayerController : MonoBehaviour {

	public ArrayList danceMoves;
	public GameObject wooweeObject;
	public WooeeController woowee;
    public GameObject messageObject;
	public MessageController messageController;
    public ComboEffect comboEffect;

	public AudioClip failSound;
	public AudioClip missSound;
	public AudioClip hitNoteSound;
	public AudioClip comboSound;

	public float approachingFactor = 10;
	public Vector3 startingPosition;

	private float affectionTrend;


	// Use this for initialization
	void Start () {
		danceMoves = new ArrayList ();
		woowee = wooweeObject.GetComponent<WooeeController> ();
        messageController = messageObject.GetComponent<MessageController> ();
        comboEffect = GetComponentInChildren<ComboEffect>();
		startingPosition = this.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		this.transform.position = startingPosition - new Vector3 (woowee.affection-0.5f, 0.0f, 0.0f) * approachingFactor;
    }
		
	void animateDanceMove(KeyAction danceMove) {
		GetComponent<Animator> ().SetTrigger (danceMove.ToString ());
	}

	public void doDanceMove(KeyAction danceMove, float accuracy) {
        Debug.Log("Player got dance move:" + danceMove + " accuracy:" + accuracy);
		playSound (danceMove);
		animateDanceMove (danceMove);
		danceMoves.Add (danceMove);
		float affectionDelta = woowee.reactToMove (danceMove, accuracy, this);
        Debug.Log("isCombo:" + KeyActionHelper.isCombo(danceMove));
        if (KeyActionHelper.isCombo(danceMove)) {
            comboEffect.instantiateEffect();
        }
		showMessage (danceMove, affectionDelta);
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

	private void showMessage(KeyAction danceMove, float affectionDelta) {
		if (messageController.isPlaying ()) {
			return;
		}
		if (woowee.affection <= 0.0f) {
			messageController.showRandomOverlayTextOfType (TextOverlayType.Lose);
			return;
		} else if (woowee.affection >= 1.0f) {
			messageController.showRandomOverlayTextOfType (TextOverlayType.Win);
			return;
		}
		affectionTrend += Mathf.Sign (affectionDelta);
		switch (danceMove) {
		case KeyAction.Fail:
			if (affectionTrend <= -3.0f) {
				messageController.showRandomOverlayTextOfType (TextOverlayType.Fail);
				affectionTrend = 0.0f;
			}
			break;
		case KeyAction.Miss:
			messageController.showRandomOverlayTextOfType (TextOverlayType.Miss);
			break;
		case KeyAction.BitchCombo:
		case KeyAction.DoNotStopCombo:
		case KeyAction.RiseCombo:
			messageController.showRandomOverlayTextOfType (TextOverlayType.Combo);
			break;
		default:
			if (affectionTrend >= 3.0f) {
				messageController.showRandomOverlayTextOfType (TextOverlayType.Hit);
				affectionTrend = 0.0f;
			}
			break;
		}
	}
}
