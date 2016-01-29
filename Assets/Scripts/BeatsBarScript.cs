using UnityEngine;
using System.Collections;
using System.Timers;
using System.Collections.Generic;

public class BeatsBarScript : MonoBehaviour {

	public GameObject beat;
	public AudioSource aud;
	public int barLengthSeconds = 3;
	public float threshold = 0.5f;
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
//		for (int i = 0; i <= 10; i++) {
//			GameObject square = Instantiate (beat) as GameObject;
//			square.transform.position = new Vector3 (i, 0, 0);
//			squares.Add (square);
//		}
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
}

public class Beat {
	public float val;
	public bool isCombo;

	public Beat(float val, bool isCombo) {
		this.val = val;
		this.isCombo = isCombo;
	}
}