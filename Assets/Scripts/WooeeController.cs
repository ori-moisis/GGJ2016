using UnityEngine;
using System.Collections;

// Characteristics types
public enum Animal { Cat, Octopus };
public enum Hat { Pimp, Party };

public class WooeeController : MonoBehaviour {
    // [0,1]
    public double affection;

    // characteristics
    public bool randomizeCharacteristics;
    public Animal animal;
    public Hat hat;

    //rules
    RuleBook ruleBook;


    // Use this for initialization
    void Start () {
        ruleBook = new RuleBook();
        if (randomizeCharacteristics) {
            generateRandomCharacteristics();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void generateRandomCharacteristics() {
        // random placeholder
        animal = Animal.Octopus;
        hat = Hat.Pimp;
    }

    public void reactToMove(DanceMove danceMove) {
        affection += ruleBook.getAffectionDelta(danceMove, this);
    }
}
