using UnityEngine;
using System.Collections;

// Characteristics types
using System;
using UnityEngine.UI;


public enum CharacterType { Cat };
public enum CharacterTrait { A, B ,C };
public enum CharacterSize { Giant, Normal, Dwarf };
public enum CharacterColor { Blue, Red, Normal };
public enum EnvironmentType { Day, Night };

public class WooeeController : MonoBehaviour {
    // [0,1]
    public float affection;

	public float affectionDiffThresh;
	public int affectionDiffMoves;

	float[] affectionHist;
	int affectionHistIndex = 0;

	public Sprite[] sprites;

    // characteristics
    public bool randomizeCharacteristics;
    public CharacterType type;
    public CharacterTrait trait;
	public CharacterSize size;
	public CharacterColor color;
	public EnvironmentType env;

    //rules
	public GameObject ruleBookObject;
    RuleBook ruleBook;

	public GameObject wooeeTextObject;
	Text wooeeText;

	public GameObject backgroundObject;
	public Sprite[] backgrounds;

	Animator animator;


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
		Sprite sprite = sprites[0];
		switch (color) {
		case CharacterColor.Blue:
			sprite = sprites [1];
			break;
		case CharacterColor.Red:
			sprite = sprites [2];
			break;
		}
		renderer.sprite = sprite;

		if (env == EnvironmentType.Day) {
			backgroundObject.GetComponent<SpriteRenderer> ().sprite = backgrounds [0];
		} else {
			backgroundObject.GetComponent<SpriteRenderer> ().sprite = backgrounds [1];
		}

		animator = GetComponent<Animator> ();
		affectionHist = new float[affectionDiffMoves];
	}

	void resetAffectionHist() {
		for (int i = 0; i < affectionHist.Length; ++i) {
			affectionHist [i] = affection;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (affection < 0) {
			affection = 0;
		} 
		if (affection > 1) {
			affection = 1;
		}
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
		env = RandomEnumValue<EnvironmentType> ();
    }

	public float reactToMove(KeyAction danceMove, float accuracy, PlayerController player) {
		float delta = ruleBook.getAffectionDelta(danceMove, accuracy, this, player);
		affection += delta;

		affectionHist [affectionHistIndex] = affection;
		affectionHistIndex = (affectionHistIndex + 1) % affectionHist.Length;
		float diff = 0;
		float lastAffect = affectionHist[affectionHistIndex];
		for (int i = 0; i < affectionHist.Length; ++i) {
			float currAffect = affectionHist [(affectionHistIndex + i) % affectionHist.Length];
			diff += (currAffect - lastAffect);
			lastAffect = currAffect;
			if (Math.Abs (diff) > this.affectionDiffThresh) {
				if (diff > this.affectionDiffThresh) {
					this.animator.SetTrigger ("Blush");
				} else {
				}
				this.resetAffectionHist ();
			}
		}

		return delta;
		
    }
}
