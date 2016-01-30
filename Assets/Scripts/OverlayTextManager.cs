using UnityEngine;
using System.Collections;


public class OverlayTextManager : MonoBehaviour {
    public GameObject[] hitMessages;
    public GameObject[] missMessages;
    public GameObject[] failMessages;
    public GameObject[] comboMessages;

    Queue animationQueue;

	// Use this for initialization
	void Start () {
        animationQueue = new Queue();
	}

    public void enqueueRandomOverlayTextOfType(TextOverlayType type) {
        GameObject[] messages = null;

        switch (type) {
            case TextOverlayType.Hit:
                messages = hitMessages;
                break;
            case TextOverlayType.Miss:
                messages = missMessages;
                break;
            case TextOverlayType.Fail:
                messages = failMessages;
                break;
            case TextOverlayType.Combo:
                messages = comboMessages;
                break;
        }

        if (messages != null && messages.Length > 0) {
            GameObject message = messages[Random.Range(0, messages.Length)];
            if (message) {
                animationQueue.Enqueue(message);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

	}
}
