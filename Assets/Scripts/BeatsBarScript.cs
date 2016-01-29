using UnityEngine;
using System.Collections;
using System.Timers;
using System.Collections.Generic;

public class BeatsBarScript : MonoBehaviour {

	public GameObject beat;
	public GameObject moveManagerObject;
	MoveManager moveManager;
	public AudioSource aud;
	public int barLengthSeconds = 3;
	public float threshold = 0.5f;
	public float accuracy = 0.2f;
	public Queue<float> beatVals;
	public Queue<float> curBeatVals;
	public Queue<Beat> curBeats;


	// Use this for initialization
	void Start () {		
		// Audio	
		aud = gameObject.GetComponent<AudioSource> ();
		aud.Play ();
		beatVals = getBeatVals ("Assets/Audio/beats.txt");
		curBeatVals = new Queue<float> ();
		List<GameObject> squares = new List<GameObject> ();
		// Move manager
		moveManager = moveManagerObject.GetComponent<MoveManager>();
	}

	// Update is called once per frame
	void Update () {
		updateBeats ();
		updateSquares ();
	}

	void updateBeats() {
		float t = aud.time;
		while (beatVals.Count != 0 && beatVals.Peek () <= t + barLengthSeconds) {
			curBeatVals.Enqueue (beatVals.Dequeue ());
		}
		while (curBeatVals.Count != 0 && curBeatVals.Peek () < t - threshold) {
			curBeatVals.Dequeue ();
			// Consider this case as a miss, and send "miss" to Barak the shark
			moveManager.handleAction(OurCoolKey.Miss, 0f);
		}
		curBeats = new Queue<Beat> ();
		foreach (float f in curBeatVals) {
			curBeats.Enqueue (new Beat(f-t, false));
		}
			
		string s = "";
		foreach (Beat b in curBeats) {
			s = s + " " + b.val.ToString();
		}
	}

	void updateSquares() {
		
	}

	Queue<float> getBeatVals(string filename) {
		string line;
		Queue<float> q = new Queue<float>();
		// Read the file and display it line by line.
		System.IO.StreamReader file = new System.IO.StreamReader(filename);
		while((line = file.ReadLine()) != null)
		{
			float beat = float.Parse (line);
			//Debug.Log (beat);
			q.Enqueue(beat);		
		}
		file.Close();
		return q;
	}

	public void input(OurCoolKey k) {
		float acc = 0;
		if (Mathf.Abs (curBeats.Peek ().val) <= accuracy) {
			// TODO: animate removal
			Beat b = curBeats.Dequeue ();
			acc = Mathf.Abs (b.val);
		} else {
			k = OurCoolKey.Fail;
		}			
		Debug.Log (k);
		Debug.Log (acc);
		moveManager.handleAction (k, acc);
	}

	public void comboHighlightNextBeat() {
	}
}

public class Beat {
	public float val;
	public bool isCombo;

	public Beat(float val, bool isCombo) {
		this.val = val;
		this.isCombo = isCombo;
	}
}

public enum OurCoolKey {A, B, C, D, Miss, Fail}
