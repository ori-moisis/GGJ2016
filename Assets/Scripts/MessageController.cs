using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum TextOverlayType { Hit, Miss, Fail, Combo, Lose, Win };

public class MessageController : MonoBehaviour {
    public Sprite[] hitSprites;
	public AudioClip[] hitSounds;
    public Sprite[] missSprites;
	public AudioClip[] missSounds;
    public Sprite[] failSprites;
	public AudioClip[] failSounds;
    public Sprite[] comboSprites;
	public AudioClip[] comboSounds;
	public Sprite[] loseSprites;
	public AudioClip[] loseSounds;
	public Sprite[] winSprites;
	public AudioClip[] winSounds;
	public Image image;
	public AudioSource audioSrc;

	public Animator endSceneAnim;

	bool done = false;

	private Animator animator;


    // Use this for initialization
    void Start () {
		animator = this.GetComponent<Animator> ();
	}

    public void showRandomOverlayTextOfType(TextOverlayType type)
    {
        Sprite[] sprites = null;
		AudioClip[] sounds = null;

        switch (type)
        {
		case TextOverlayType.Hit:
			sprites = hitSprites;
			sounds = hitSounds;
            break;
		case TextOverlayType.Miss:
			sprites = missSprites;
			sounds = missSounds;
            break;
		case TextOverlayType.Fail:
			sprites = failSprites;
			sounds = failSounds;
            break;
		case TextOverlayType.Combo:
			sprites = comboSprites;
			sounds = comboSounds;
            break;
		case TextOverlayType.Win:
			sprites = winSprites;
			sounds = winSounds;
			break;
		case TextOverlayType.Lose:
			image.sprite = null;
			image.overrideSprite = null;
			animator.SetTrigger ("nopeMessage");
			done = true;
			sounds = loseSounds;
			break;
        }

		if (sounds == null || sounds.Length == 0) {
			// Should not happend
			return;
		}

		int index = Random.Range (0, sounds.Length - 1);
		audioSrc.PlayOneShot (sounds [index]);

        if (sprites != null && sprites.Length > 0)
        {
            Sprite sprite = sprites[index];
            if (sprite)
            {
				image.sprite = sprite;
				image.overrideSprite = sprite;
				this.animator.SetTrigger("showMessage");
            }
        }
    }

	public bool isPlaying() {
		return done;
		AnimatorStateInfo clip = this.animator.GetCurrentAnimatorStateInfo (0);
		return clip.IsName ("NopeMessage");
	}
}
