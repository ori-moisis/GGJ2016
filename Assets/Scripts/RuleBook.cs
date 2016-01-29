using UnityEngine;
using System.Collections;
using System;

public interface IRule
{
    // Returns the difference in affection caused by this rule due to lastMove and wooee.
	float getAffectionDelta(DanceMove lastMove, float accuracy, WooeeController wooee, PlayerController player);
}

public class OctopusRule : IRule
{
	public float getAffectionDelta(DanceMove lastMove, float accuracy, WooeeController wooee, PlayerController player)
    {
		if (player.danceMoves.Count < 2 || lastMove != (DanceMove)player.danceMoves [player.danceMoves.Count - 2]) {
			return 0.1f;
		} else {
			return -0.1f;
		}
    }
}

public class RuleBook : IRule {
    IRule[] rules;

	public RuleBook() {
		rules = new IRule[] {
			new OctopusRule()
		};
	}

	public float getAffectionDelta(DanceMove lastMove, float accuracy, WooeeController wooee, PlayerController player)
    {
        float sum = 0f;
		foreach (IRule rule in rules)
        {
			sum += rule.getAffectionDelta(lastMove, accuracy, wooee, player);
        }
        return sum;
    }
}

