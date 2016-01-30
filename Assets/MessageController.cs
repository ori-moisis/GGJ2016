using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum TextOverlayType { Hit, Miss, Fail, Combo }

public class MessageController : MonoBehaviour {
    public Sprite[] hitSprites;
    public Sprite[] missSprites;
    public Sprite[] failSprites;
    public Sprite[] comboSprites;


    // Use this for initialization
    void Start () {
	
	}

    public void showRandomOverlayTextOfType(TextOverlayType type)
    {
        Sprite[] sprites = null;

        switch (type)
        {
            case TextOverlayType.Hit:
                sprites = hitSprites;
                break;
            case TextOverlayType.Miss:
                sprites = missSprites;
                break;
            case TextOverlayType.Fail:
                sprites = failSprites;
                break;
            case TextOverlayType.Combo:
                sprites = comboSprites;
                break;
        }

        if (sprites != null && sprites.Length > 0)
        {
            Sprite sprite = sprites[Random.Range(0, sprites.Length)];
            if (sprite)
            {
                Image image = this.GetComponent<Image>();
                image.sprite = sprite;
                this.GetComponent<Animator>().SetTrigger("showMessage");
            }
        }
    }
}
