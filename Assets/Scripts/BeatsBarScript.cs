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
	public GameObject comboPrefab;
	public GameObject ringPrefab;
	public GameObject mark;

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

		beatVals = getBeatVals ("Assets/Audio/beats.txt");
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
			Destroy (beat.gfx);

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
		if (nextIsCombo) {
			Beat combo = curBeats.Peek();
		}
	}

	float secondsToPosition(float relativeTime) {
		return mark.transform.position.x + relativeTime * unitsPerSecond;
	}

	Queue<float> getBeatVals(string filename) {
		int skipped = 0;
		string line;
		Queue<float> q = new Queue<float>();
		// Read the file and display it line by line.
		System.IO.StreamReader file = new System.IO.StreamReader(filename);
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
			// TODO: animate removal
			acc = (threshold - Mathf.Abs (b.relativeTime)) / threshold;
			GameObject ringGfx = Instantiate (ringPrefab) as GameObject;
			ringGfx.transform.position = b.gfx.transform.position;
			Animator animator = ringGfx.GetComponent<Animator> ();
			AnimationClip beatHitClip = animator.runtimeAnimatorController.animationClips [0];
			animator.Play (beatHitClip.name);
			Destroy (ringGfx, beatHitClip.length);
			Destroy (b.gfx, beatHitClip.length);
		} else {
			k = KeyAction.Fail;
			aud.time = aud.time - 0.1f;
		}			
		Debug.Log (k);
		Debug.Log (acc);
		moveManager.handleAction (k, acc);
	}

	public void comboHighlightNextBeat() {
		nextIsCombo = true;
		if (nextIsCombo) {
			Queue<Beat> tempQueue = new Queue<Beat> ();
			bool firstInQueue = true;
			while (curBeats.Count > 0) {
				Beat current = curBeats.Dequeue();
				if (firstInQueue) {
					firstInQueue = false;
					GameObject comboBeat = Instantiate (comboPrefab) as GameObject;
					tempQueue.Enqueue(new Beat(current.beatTime, comboBeat));
					Destroy (current.gfx);
				}
				else {
					tempQueue.Enqueue(current);
				}
			}
			curBeats = tempQueue;
			nextIsCombo = false;
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
