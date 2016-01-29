using UnityEngine;
using System.Collections;

// Characteristics types
public enum CharacterType { Cat };
public enum CharacterTrait { A, B ,C };
public enum CharacterSize { Giant, Normal, Dwarf };
public enum CharacterColor { Blue, Red, Normal };

public class WooeeController : MonoBehaviour {
    // [0,1]
    public float affection;

    // characteristics
    public bool randomizeCharacteristics;
    public CharacterType type;
    public CharacterTrait trait;
	public CharacterSize size;
	public CharacterColor color;

    //rules
	public GameObject ruleBookObject;
    RuleBook ruleBook;


    // Use this for initialization
    void Start () {
		ruleBook = ruleBookObject.GetComponent<RuleBook> ();
        if (randomizeCharacteristics) {
            generateRandomCharacteristics();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void generateRandomCharacteristics() {
        // random placeholder
		type = CharacterType.Cat;
		trait = CharacterTrait.A;
		size = CharacterSize.Normal;
		color = CharacterColor.Normal;
    }

	public void reactToMove(KeyAction danceMove, float accuracy, PlayerController player) {
		affection += ruleBook.getAffectionDelta(danceMove, accuracy, this, player);
    }
}
