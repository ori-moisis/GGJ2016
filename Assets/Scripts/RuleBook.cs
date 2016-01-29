using UnityEngine;
using System.Collections;
using System;

public interface IRule
{
    // Returns the difference in affection caused by this rule due to lastMove and wooee.
	double getAffectionDelta(DanceMove lastMove, double accuracy, WooeeController wooee, PlayerController player);
}

public class OctopusRule : IRule
{
	public double getAffectionDelta(DanceMove lastMove, double accuracy, WooeeController wooee, PlayerController player)
    {
		if (player.danceMoves.Count < 2 || lastMove != (DanceMove)player.danceMoves [player.danceMoves.Count - 2]) {
			return 0.1;
		} else {
			return -0.1;
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

	public double getAffectionDelta(DanceMove lastMove, double accuracy, WooeeController wooee, PlayerController player)
    {
        double sum = 0;
		foreach (IRule rule in rules)
        {
			sum += rule.getAffectionDelta(lastMove, accuracy, wooee, player);
        }
        return sum;
    }
}

