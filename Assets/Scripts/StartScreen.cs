using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame() {
		Debug.Log ("Start game");
		SceneManager.LoadScene ("Project");

//		SceneManager.UnloadScene ("Start");
	}
}
