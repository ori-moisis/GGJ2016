using UnityEngine;
using System.Collections;
using System.Timers;
using System.Collections.Generic;

public class BeatsBarScript : MonoBehaviour {

	public GameObject moveManagerObject;

	public int beatSkip = 2;

	public float barLengthSeconds = 3.0f;
	public float threshold = 0.5f;
	public GameObject barArea;
	public GameObject beatPrefab;
	public GameObject ringPrefab;
	public GameObject mark;
	public TextAsset beats;
    public GameObject comboBeatEffect;

	private float unitsPerSecond;

	private AudioSource aud;
	private Queue<float> beatVals;
	private Queue<Beat> curBeats;

	private bool nextIsCombo;
	private MoveManager moveManager;

	// Use this for initialization
	void Start () {
		nextIsCombo = false;

		float rightBarUnits = Mathf.Abs(barArea.transform.position.x - mark.transform.position.x);
		unitsPerSecond = rightBarUnits / barLengthSeconds;

		// Audio	
		aud = GetComponent<AudioSource> ();
		aud.Play ();

		beatVals = getBeatVals ();
		curBeats = new Queue<Beat> ();

		// Move manager
		moveManager = moveManagerObject.GetComponent<MoveManager>();
	}

	// Update is called once per frame
	void Update () {
		updateBeats ();
	}

	void OnGUI() {
		updateBeatGfx ();
	}

	void updateBeats() {
		float t = aud.time;
		while (beatVals.Count != 0 && beatVals.Peek () <= t + barLengthSeconds) {
			float beatTime = beatVals.Dequeue ();

			GameObject beatGfx;
			 
			beatGfx = Instantiate (beatPrefab) as GameObject;
			curBeats.Enqueue (new Beat(beatTime, beatGfx));	

		}

		while (curBeats.Count != 0 && curBeats.Peek ().RelativeTime(t) <= -threshold) {
			Beat beat = curBeats.Dequeue ();
            beat.gfx.GetComponent<Animator>().SetTrigger("fade");
            Destroy(beat.gfx, 1f); 

			// Consider this case as a miss, and send "miss" to Barak the shark
			moveManager.handleAction(KeyAction.Miss, 0f);
		}
	}

	void updateBeatGfx() {
		foreach (Beat beat in curBeats) {
			beat.RelativeTime (aud.time);
			beat.gfx.transform.position = new Vector3 (secondsToPosition (beat.relativeTime), mark.transform.position.y);
			beat.Show ();
		}
	}

	float secondsToPosition(float relativeTime) {
		return mark.transform.position.x + relativeTime * unitsPerSecond;
	}

	Queue<float> getBeatVals() {
		int skipped = 0;
		string line;
		Queue<float> q = new Queue<float>();
		System.IO.StringReader file = new System.IO.StringReader (beats.text);
		while((line = file.ReadLine()) != null)
		{
			skipped++;
			if (skipped % beatSkip == 0) {
				float beat = float.Parse (line);
				q.Enqueue (beat);
			}
		}
		file.Close();
		return q;
	}

	public void input(KeyAction k) {
		float acc = 0;
		if (Mathf.Abs (curBeats.Peek ().RelativeTime(aud.time)) <= threshold) {
			Beat b = curBeats.Dequeue ();
            acc = (threshold - Mathf.Abs (b.relativeTime)) / threshold;
			GameObject ringGfx = Instantiate (ringPrefab) as GameObject;
			ringGfx.transform.position = b.gfx.transform.position;
			Animator animator = ringGfx.GetComponent<Animator> ();
			AnimationClip beatHitClip = animator.runtimeAnimatorController.animationClips [0];
			animator.Play (beatHitClip.name);
			Destroy (ringGfx, beatHitClip.length);
            b.gfx.GetComponent<Animator>().SetTrigger("fade");
            Destroy(b.gfx, beatHitClip.length + 1f); // delay added for fading aniamtion to finish
		} else {
            comboUnhighlightNextBear();
            k = KeyAction.Fail;
			aud.time = aud.time - 0.1f;
		}			
		Debug.Log (k);
		Debug.Log (acc);
		moveManager.handleAction (k, acc);

        // accelerate song
        if (moveManager.playerObject.GetComponent<PlayerController>().woowee.affection > 0.75f && beatSkip == 2) {
            beatSkip = 1;
            beatVals = filterPassedBeats(getBeatVals());

            //updateBeatGfx();
        }
	}

    Queue<float> filterPassedBeats(Queue<float> queue)
    {
        Queue<float> returnQueue = new Queue<float>();

        foreach(float beat in queue)
        {
            if (beat > aud.time + 3) {
                returnQueue.Enqueue(beat);
            }
        }
        return returnQueue;
    }

    public void comboHighlightNextBeat() {
        Beat comboBeat = curBeats.Peek();
        comboBeat.gfx.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        GameObject beatEffect = Instantiate(comboBeatEffect, comboBeat.gfx.transform.position, Quaternion.identity) as GameObject;
        beatEffect.transform.parent = comboBeat.gfx.transform;
    }

    void comboUnhighlightNextBear() {
        Beat comboBeat = curBeats.Peek();
        comboBeat.gfx.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if (comboBeat.gfx.transform.childCount > 0)  {
            Destroy(comboBeat.gfx.transform.GetChild(0).gameObject);
        }
    }
}

public class Beat {
	// Times are in seconds
	public float beatTime;
	public float relativeTime;
	public bool isDying;
	public GameObject gfx;

	public Beat(float beatTime, GameObject gfx) {
		this.isDying = false;
		this.beatTime = beatTime;
		this.relativeTime = beatTime;
		this.gfx = gfx;
		this.gfx.SetActive (false);
		this.gfx.GetComponent<Renderer> ().enabled = false;
	}

	public void Show() {
		this.gfx.SetActive (true);
		this.gfx.GetComponent<Renderer> ().enabled = true;
	}

	public float RelativeTime(float currentTime) {
		return (this.relativeTime = this.beatTime - currentTime);
	}
		
}

public enum KeyAction {Paddle, Reach, Twist, Down, Miss, Fail, 
					   BitchCombo, RiseCombo, DoNotStopCombo}

public class KeyActionHelper {
	public static bool isCombo(KeyAction action) {
		return action == KeyAction.BitchCombo || action == KeyAction.RiseCombo || action == KeyAction.DoNotStopCombo;
	}

	public static bool isFail(KeyAction action) {
		return action == KeyAction.Fail || action == KeyAction.Miss;
	}
}
