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
		if (player.danceMoves.Count < 2 || lastMove != (KeyAction)player.danceMoves [player.danceMoves.Count - 2]) {
			return 1 * accuracy;
		} else {
			return -1 * accuracy;
		}
    }
}

public class RuleBook : MonoBehaviour, IRule {
    IRule[] rules;

	public static float baseScore = 0.1f;

	public RuleBook() {
		rules = new IRule[] {
			new BaseRule()
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

