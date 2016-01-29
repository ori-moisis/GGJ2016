using UnityEngine;
using System.Collections;
using System.Timers;
using System.Collections.Generic;

public class BeatsBarScript : MonoBehaviour {

	public GameObject moveManagerObject;
	MoveManager moveManager;
	public AudioSource aud;
	public GameObject mark;
	public GameObject rectangle;
	public GameObject beatPrefab;
	public int barLengthSeconds = 3;
	public float threshold = 0.5f;
	public int beatSkip = 2;
	public Queue<float> beatVals;
	public Queue<Beat> curBeats;

	private Vector3 barLeft;


	// Use this for initialization
	void Start () {		
		// Audio	
		aud = GetComponent<AudioSource> ();
		aud.Play ();
		beatVals = getBeatVals ("Assets/Audio/beats.txt");
		curBeats = new Queue<Beat> ();

		// Move manager
		moveManager = moveManagerObject.GetComponent<MoveManager>();

		Bounds barBounds = rectangle.GetComponent<Renderer> ().bounds;
		barLeft = new Vector3 (barBounds.min.x, barBounds.center.y);

		mark.transform.position = barLeft + new Vector3 (secondsToOffset(0.0f), 0.0f);
	}

	// Update is called once per frame
	void Update () {
		updateBeats ();
	}

	void OnGUI() {
		updateSquares ();
	}

	void updateBeats() {
		float t = aud.time;
		while (beatVals.Count != 0 && beatVals.Peek () <= t + barLengthSeconds) {
			float beatTime = beatVals.Dequeue ();
			GameObject obj = Instantiate (beatPrefab) as GameObject;
			curBeats.Enqueue (new Beat(beatTime, obj));
		}

		while (curBeats.Count != 0 && curBeats.Peek ().RelativeTime(t) <= -threshold) {
			Beat beat = curBeats.Dequeue ();
			Destroy (beat.obj);

			// Consider this case as a miss, and send "miss" to Barak the shark
			moveManager.handleAction(KeyAction.Miss, 0f);
		}
	}

	void updateSquares() {
		foreach (Beat beat in curBeats) {
			beat.RelativeTime (aud.time);
			float offsetX = secondsToOffset (beat.relativeTime);
			beat.obj.transform.position = barLeft + new Vector3(offsetX, 0.0f);
			beat.Show ();
		}
	}

	float secondsToOffset(float relativeTime) {
		return (relativeTime + threshold) / (barLengthSeconds + threshold) * rectangle.transform.localScale.x;
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
			// TODO: animate removal
			Beat b = curBeats.Dequeue ();
			acc = Mathf.Abs (b.relativeTime) / threshold;
			Destroy (b.obj);
		} else {
			k = KeyAction.Fail;
		}			
		Debug.Log (k);
		Debug.Log (acc);
		moveManager.handleAction (k, acc);
	}

	public void comboHighlightNextBeat() {
	}
}

public class Beat {
	// Times are in seconds
	public float beatTime;
	public float relativeTime;
	public bool isCombo;
	public GameObject obj;

	public Beat(float beatTime, GameObject obj) {
		this.beatTime = beatTime;
		this.relativeTime = beatTime;
		this.isCombo = false;
		this.obj = obj;
		this.obj.SetActive (false);
		this.obj.GetComponent<Renderer> ().enabled = false;
	}

	public void Show() {
		this.obj.SetActive (true);
		this.obj.GetComponent<Renderer> ().enabled = true;
	}

	public float RelativeTime(float currentTime) {
		return (this.relativeTime = this.beatTime - currentTime);
	}
}

public enum KeyAction {A, B, C, D, Miss, Fail}
