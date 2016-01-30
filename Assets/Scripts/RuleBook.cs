using UnityEngine;
using System.Collections;
using System;

public interface IRule
{
    // Returns the difference in affection caused by this rule due to lastMove and wooee.
	float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player);
}

public class BaseRule : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
    {
		if (lastMove == KeyAction.Fail || lastMove == KeyAction.Miss) {
			return 0;
		}

		if (player.danceMoves.Count < 2 || lastMove != (KeyAction)player.danceMoves [player.danceMoves.Count - 2]) {
			return RuleBook.baseScore * accuracy;
		} else {
			return -RuleBook.baseScore * accuracy;
		}
    }
}

public class FailSucks : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		if (lastMove == KeyAction.Fail || lastMove == KeyAction.Miss) {
			return -RuleBook.baseScore;
		}
		return 0;
	}
}

public class RuleBook : MonoBehaviour, IRule {
    IRule[] rules;

	public static float baseScore = 0.05f;

	public RuleBook() {
		rules = new IRule[] {
			new BaseRule(),
			new FailSucks()
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

