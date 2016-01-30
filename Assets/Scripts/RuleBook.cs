using UnityEngine;
using System.Collections;
using System;

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
	public static float Divider = 10.0f;

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		if (KeyActionHelper.isFail(lastMove)) {
			return 0;
		}

		if (player.danceMoves.Count > 2) {
			int i = Math.Max (0, player.danceMoves.Count - HistoryCount);
			int count = 0;
			for (; i < player.danceMoves.Count - 1; ++i) {
				if (lastMove == (KeyAction)player.danceMoves [i]) {
					++count;
				}
			}
			return (-count * RuleBook.baseScore) / Divider;
		}

		return RuleBook.baseScore * accuracy;
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
			return -RuleBook.baseScore;
		}
		return 0;
	}
}

public class ComboTooSoon : IRule
{
	public static float minAffection = 0.6f;

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		if (KeyActionHelper.isCombo (lastMove)) {
			if (wooee.affection < minAffection) {
				return -RuleBook.baseScore;
			}
		}

		return 0;
	}
}

public class GiantsRules : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		if (wooee.size == CharacterSize.Giant) {
			if (lastMove == KeyAction.Twist) {
				return -RuleBook.baseScore;
			}
			if (lastMove == KeyAction.Reach) {
				return RuleBook.baseScore;
			}
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

public class RuleBook : MonoBehaviour, IRule {
    IRule[] rules;

	public static float baseScore = 0.05f;

	public RuleBook() {
		rules = new IRule[] {
			new WellTimedMove(),
			new RepetativePenalty(),
			new FailSucks(),
			new ComboTooSoon(),
			new GiantsRules(),
			new DwarfRules()
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
}

