using UnityEngine;
using System.Collections;

public class ComboEffect : MonoBehaviour {
    public GameObject effectPrefab;
    public float duration = 2;
	
	public void instantiateEffect() {
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(effect, duration);
    }
}
