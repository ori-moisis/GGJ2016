using UnityEngine;
using System.Collections;
using System.Timers;
using System.Collections.Generic;

public class BeatsBarScript : MonoBehaviour {

	public AudioSource aud;
	public int barLengthSeconds = 3;
	public Queue<float> allBeats;
	public Queue<float> curBeats;
	public Queue<float> curBeatsRelative;


	// Use this for initialization
	void Start () {				
		aud = gameObject.GetComponent<AudioSource> ();
		aud.Play ();
		allBeats = getBeats ("Assets/Audio/beats.txt");
		curBeats = new Queue<float> ();
	}

	// Update is called once per frame
	void Update () {
		updateBeats ();
	}

	void updateBeats() {
		float t = aud.time;
		while (allBeats.Count != 0 && allBeats.Peek () <= t + barLengthSeconds) {
			curBeats.Enqueue (allBeats.Dequeue ());
		}
		while (curBeats.Count != 0 && curBeats.Peek () < t) {
			curBeats.Dequeue ();
		}
		curBeatsRelative = new Queue<float> ();
		foreach (float f in curBeats) {
			curBeatsRelative.Enqueue (f-t);
		}

		Debug.Log (t);
		string s = "";
		foreach (float f in curBeatsRelative) {
			s = s + " " + f.ToString ();
		}
		Debug.Log (s);
	}

	Queue<float> getBeats(string filename) {
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
