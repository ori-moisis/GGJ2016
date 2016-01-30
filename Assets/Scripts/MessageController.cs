using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum TextOverlayType { Hit, Miss, Fail, Combo, Lose, Win };

public class MessageController : MonoBehaviour {
    public Sprite[] hitSprites;
    public Sprite[] missSprites;
    public Sprite[] failSprites;
    public Sprite[] comboSprites;
	public Sprite[] loseSprites;
	public Sprite[] winSprites;
	public Image image;

	private Animator animator;


    // Use this for initialization
    void Start () {
		animator = this.GetComponent<Animator> ();
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
			case TextOverlayType.Win:
				sprites = winSprites;
				break;
			case TextOverlayType.Lose:
				image.sprite = null;
				image.overrideSprite = null;
				animator.SetTrigger("nopeMessage");
				return;
        }

        if (sprites != null && sprites.Length > 0)
        {
            Sprite sprite = sprites[Random.Range(0, sprites.Length - 1)];
            if (sprite)
            {
				image.sprite = sprite;
				image.overrideSprite = sprite;
				this.animator.SetTrigger("showMessage");
            }
        }
    }

	public bool isPlaying() {
		AnimatorStateInfo clip = this.animator.GetCurrentAnimatorStateInfo (0);
		return clip.IsName ("NopeMessage");
	}
}
