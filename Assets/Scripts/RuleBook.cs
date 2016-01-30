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
	public static int HistoryCount = 5;
	public static float Divider = 1.0f;

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

	public static float baseScore = 0.05f;

	public RuleBook() {
		rules = new IRule[] {
			new WellTimedMove(),
			new RepetativePenalty(),
			new FailSucks(),
			new Combos(),
			new GiantsRules(),
			new DwarfRules(),
			new NightRules()
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

