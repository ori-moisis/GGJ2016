using UnityEngine;
using System.Collections;

// Characteristics types
using System;
using UnityEngine.UI;


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

	public GameObject wooeeTextObject;
	Text wooeeText;


    // Use this for initialization
    void Start () {
		ruleBook = ruleBookObject.GetComponent<RuleBook> ();
		wooeeText = wooeeTextObject.GetComponent<Text> ();
        if (randomizeCharacteristics) {
            generateRandomCharacteristics();
        }

		wooeeText.text = type.ToString() + "\n" 
			+ trait.ToString() + "\n" 
			+ size.ToString() + "\n" 
			+ color.ToString();

		SpriteRenderer renderer = GetComponentInParent<SpriteRenderer> ();
		switch (color) {
		case CharacterColor.Blue:
			renderer.color = Color.HSVToRGB (160 / 255.0f, 200 / 255.0f, 255 / 255.0f);
			break;
		case CharacterColor.Red:
			renderer.color = Color.HSVToRGB (0 / 255.0f, 200 / 255.0f, 255 / 255.0f);
			break;
		}
		switch (size) {
		case CharacterSize.Dwarf:
			renderer.transform.localScale -= new Vector3 (0.5f, 0.5f, 0.0f);
			renderer.transform.transform.position += new Vector3 (0.25f, -0.5f);
			break;
		case CharacterSize.Giant:
			renderer.transform.localScale += new Vector3 (0.5f, 0.5f, 0.0f);
			renderer.transform.transform.position += new Vector3 (-0.25f, 0.5f);
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// http://stackoverflow.com/a/3132139
	static T RandomEnumValue<T> ()
	{
		var v = Enum.GetValues (typeof (T));
		return (T) v.GetValue (new System.Random ().Next(v.Length));
	}

    void generateRandomCharacteristics() {
		type = RandomEnumValue<CharacterType> ();
		trait = RandomEnumValue<CharacterTrait> ();
		size = RandomEnumValue<CharacterSize> ();
		color = RandomEnumValue<CharacterColor> ();
    }

	public void reactToMove(KeyAction danceMove, float accuracy, PlayerController player) {
		affection += ruleBook.getAffectionDelta(danceMove, accuracy, this, player);
    }
}
