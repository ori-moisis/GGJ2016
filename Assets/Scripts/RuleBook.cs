using UnityEngine;
using System.Collections;
using System;

public interface IRule
{
    // Returns the difference in affection caused by this rule due to lastMove and wooee.
    double getAffectionDelta(DanceMove lastMove, WooeeController wooee);
}

public class OctopusRule : IRule
{
    public double getAffectionDelta(DanceMove lastMove, WooeeController wooee)
    {
        return 0;
    }
}

public class RuleBook : IRule {
    IRule[] rules;

    public double getAffectionDelta(DanceMove lastMove, WooeeController wooee)
    {
        double sum = 0;
        foreach (IRule rule in rules)
        {
            sum += rule.getAffectionDelta(lastMove, wooee);
        }
        return sum;
    }

    // Use this for initialization
    void Start () {
        rules = new IRule[] {
            new OctopusRule()
        };
	}
}

