using UnityEngine;
using System.Collections;
using System.Linq;

public enum DanceMove { Jump, Kick }

// maps actions to dance moves, including replacing relevant moves with combos if applicable.
public class MoveManager : MonoBehaviour {
    public class Combo : object {
        public ArrayList moveSequence;
        public DanceMove comboMove;

        public Combo(DanceMove comboMove, DanceMove[] moveSequence) {
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
            new Combo(DanceMove.Jump, new DanceMove[] { DanceMove.Jump })
        };
    }

	public void handleAction(KeyAction action, float accuracy) {
        player.doDanceMove(moveForNextDanceMove(moveForAction(action)), accuracy);
        if (isOneMoveFromCombo()) {
            beatsBar.comboHighlightNextBeat();
        }
    }

	DanceMove moveForAction(KeyAction action) {
        return DanceMove.Jump;
    }

    // returns the combo dance move that will be completed with `move` or `move` if it does not complete a combo.
    public DanceMove moveForNextDanceMove(DanceMove move) {
        ArrayList previousMoves = player.danceMoves;

        if (previousMoves.Count < 1) {
            return move;
        }

        if (!isOneMoveFromCombo()) {
            return move;
        }

        foreach (Combo combo in combos) {
            int comboLength = combo.moveSequence.Count;
            ArrayList relevantMoves = previousMoves.GetRange(previousMoves.Count - (comboLength), comboLength - 1);
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
            ArrayList relevantPreviousMoves = previousMoves.GetRange(previousMoves.Count - (comboLength - 1), comboLength - 1);
            if (relevantPreviousMoves.Cast<object>().SequenceEqual(combo.moveSequence.GetRange(0, comboLength - 1).Cast<object>())) {
                return true;
            }
        }
        return false;
    }
}
