using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AffectionBar : MonoBehaviour {

	public GameObject wooweeObject;
	WooeeController woowee;
	Slider slider;

	// Use this for initialization
	void Start () {
		woowee = wooweeObject.GetComponent<WooeeController> ();
		slider = this.GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		slider.value = woowee.affection;
	}
}
