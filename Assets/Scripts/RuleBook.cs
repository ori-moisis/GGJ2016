using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public interface IRule
{
    // Returns the difference in affection caused by this rule due to lastMove and wooee.
	float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player);
}

public class WellTimedMove : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
    {
		if (KeyActionHelper.isFail(lastMove)) {
			return 0;
		}

		return RuleBook.baseScore * accuracy;
    }
}

public class RepetativePenalty : IRule
{
	public static int HistoryCount = 10;
	public static int MaxAllowed = 2;
	public static float Multiplier = 2.0f;



	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		if (KeyActionHelper.isFail(lastMove)) {
			return 0;
		}

		for(int i=4; i>=1; i--) {
			if (player.danceMoves.Count < i * 2) {
				continue;
			}
			ArrayList prevSequence = player.danceMoves.GetRange(player.danceMoves.Count - 2*i, i);
			ArrayList currentSequence = player.danceMoves.GetRange(player.danceMoves.Count - i, i);
			if (prevSequence.Cast<object>().SequenceEqual(currentSequence.Cast<object>())) {
				Debug.Log(i);
				Debug.Log("Rep");
				return - i * RuleBook.baseScore;
			}
		}

//		if (player.danceMoves.Count > 2) {
//			int i = Math.Max (0, player.danceMoves.Count - HistoryCount);
//			int count = 0;
//			for (; i < player.danceMoves.Count - 1; ++i) {
//				if (lastMove == (KeyAction)player.danceMoves [i]) {
//					++count;
//				}
//			}
//			return - Mathf.Abs(count - MaxAllowed) * RuleBook.baseScore * Multiplier;
//		}
//
//		return RuleBook.baseScore * accuracy;
		return 0;
	}
}

public class FailSucks : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		if (lastMove == KeyAction.Miss) {
			return -RuleBook.baseScore / 2.0f;
		}
		if (lastMove == KeyAction.Fail) {
			return -RuleBook.baseScore * 2;
		}
		return 0;
	}
}

public class Combos : IRule
{
	public static float minAffection = 0.6f;

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		if (KeyActionHelper.isCombo (lastMove)) {
			if (wooee.affection < minAffection) {
				return -2 * RuleBook.baseScore;
			} else {
				return 3 * RuleBook.baseScore;
			}
		}

		return 0;
	}
}

public class GiantsRules : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		ArrayList good = new ArrayList ();
		good.Add (KeyAction.Paddle);
		good.Add (KeyAction.Reach);

		ArrayList good2 = new ArrayList ();
		good2.Add (KeyAction.Paddle);
		good2.Add (KeyAction.Twist);

		if ( RuleBook.sequenceJustOccured(good, player)) {
			return 3*RuleBook.baseScore;
		}

		if ( RuleBook.sequenceJustOccured(good2, player)) {
			return 3*RuleBook.baseScore;
		}


		if (player.danceMoves.Count <= 1) {
			return RuleBook.baseScore * accuracy;			
		}

		return 0;
	}
}


public class DwarfRules : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		if (wooee.size == CharacterSize.Dwarf) {
			if (lastMove == KeyAction.Reach) {
				return -RuleBook.baseScore;
			}
			if (lastMove == KeyAction.Down) {
				return RuleBook.baseScore;
			}
		}
		return 0;
	}
}

public class RedRules : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		if (wooee.color == CharacterColor.Red) {
			if (lastMove == KeyAction.Paddle) {
				return -RuleBook.baseScore;
			}
		}
		return 0;
	}
}

public class NightRules : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{

		if (wooee.env == EnvironmentType.Night) {
			if (lastMove == KeyAction.Reach) {
				if ((KeyAction)player.danceMoves[-2] == KeyAction.Reach) {
					return 2*RuleBook.baseScore;
				}
			}
			if (lastMove == KeyAction.Twist) {
				if ((KeyAction)player.danceMoves[-2] == KeyAction.Twist) {
					return -2*RuleBook.baseScore;
				}
			}
		}
		return 0;
	}
}

public class RuleBook : MonoBehaviour, IRule {
    IRule[] rules;

	public static float baseScore = 0.02f;

	public RuleBook() {
		rules = new IRule[] {
			new WellTimedMove(),
			new RepetativePenalty(),
			new FailSucks(),
			new Combos(),
			new GiantsRules()
		};
	}

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
    {
        float sum = 0f;
		foreach (IRule rule in rules)
        {
			sum += rule.getAffectionDelta(lastMove, accuracy, wooee, player);
        }
        return sum;
    }

	public static bool sequenceJustOccured(ArrayList sequence, PlayerController player) {

		if (sequence.Count > player.danceMoves.Count) {
			return false;
		}

		ArrayList relevantMoves = player.danceMoves.GetRange(player.danceMoves.Count - sequence.Count, sequence.Count);
		if (relevantMoves.Cast<object>().SequenceEqual(sequence.Cast<object>())) {
			return true;
		}
		return false;
	}
}

