using UnityEngine;
using System.Collections;
using System.Linq;

// maps actions to dance moves, including replacing relevant moves with combos if applicable.
public class MoveManager : MonoBehaviour {
    public class Combo : object {
        public ArrayList moveSequence;
        public KeyAction comboMove;

        public Combo(KeyAction comboMove, KeyAction[] moveSequence) {
            this.comboMove = comboMove;
            this.moveSequence = new ArrayList(moveSequence);
        }
    }

    public GameObject playerObject;
    public GameObject beatsBarObject;
    PlayerController player;
    BeatsBarScript beatsBar;
    Combo[] combos;


    void Start() {
        player = playerObject.GetComponent<PlayerController>();
        beatsBar = beatsBarObject.GetComponent<BeatsBarScript>();
        combos = new Combo[] {
			new Combo(KeyAction.BitchCombo, new KeyAction[] { KeyAction.Down, KeyAction.Down, KeyAction.Down, KeyAction.Twist }),
			new Combo(KeyAction.DoNotStopCombo, new KeyAction[] { KeyAction.Paddle, KeyAction.Paddle, KeyAction.Paddle, KeyAction.Paddle }),
			new Combo(KeyAction.RiseCombo, new KeyAction[] { KeyAction.Down, KeyAction.Reach, KeyAction.Down, KeyAction.Reach })
        };
    }

	public void handleAction( KeyAction action, float accuracy) {
        player.doDanceMove(moveForNextDanceMove(action), accuracy);
        if (isOneMoveFromCombo()) {
            beatsBar.comboHighlightNextBeat();
        }
    }

    // returns the combo dance move that will be completed with `move` or `move` if it does not complete a combo.
    public KeyAction moveForNextDanceMove(KeyAction move) {
        ArrayList previousMoves = player.danceMoves;

        if (previousMoves.Count < 1) {
            return move;
        }

        if (!isOneMoveFromCombo()) {
            return move;
        }

        foreach (Combo combo in combos) {
            int comboLength = combo.moveSequence.Count;
			if (previousMoves.Count < comboLength) {
				continue;
			}
            ArrayList relevantMoves = previousMoves.GetRange(previousMoves.Count - comboLength, comboLength - 1);
            relevantMoves.Add(move);
            if (relevantMoves.Cast<object>().SequenceEqual(combo.moveSequence.Cast<object>())) {
                return combo.comboMove;
            }
        }
        return move;
    }

    bool isOneMoveFromCombo() {
        ArrayList previousMoves = player.danceMoves;

        if (previousMoves.Count < 1) {
            return false;
        }

        foreach (Combo combo in combos) {
            int comboLength = combo.moveSequence.Count;
			if (previousMoves.Count < (comboLength - 1)) {
				continue;
			}
            ArrayList relevantPreviousMoves = previousMoves.GetRange(previousMoves.Count - (comboLength - 1), comboLength - 1);
            if (relevantPreviousMoves.Cast<object>().SequenceEqual(combo.moveSequence.GetRange(0, comboLength - 1).Cast<object>())) {
                return true;
            }
        }
        return false;
    }
}
