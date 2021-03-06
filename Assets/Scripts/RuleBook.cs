﻿using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public interface IRule
{
    // Returns the difference in affection caused by this rule due to lastMove and wooee.
	float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player);
}

public class BaseRule : IRule {
	public ArrayList good = new ArrayList ();
	public ArrayList bad = new ArrayList ();
	public float goodFactor;
	public float badFactor;

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player) {
		return getDelta (player);
	}

	public float getDelta(PlayerController player) {
		if ( RuleBook.sequenceJustOccured(good, player)) {
			return goodFactor * RuleBook.baseScore * good.Count;
		}
		if ( RuleBook.sequenceJustOccured(bad, player)) {
			return -badFactor * RuleBook.baseScore * bad.Count;
		}
		return 0;		
	}
}

public class WellTimedMove : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
    {
		if (KeyActionHelper.isFail(lastMove)) {
			return 0;
		}
		if (wooee.affection < 0.3) {
			return 6 * RuleBook.baseScore * accuracy;
		} else if (wooee.affection < 0.6) {
			return RuleBook.baseScore * accuracy;
		}
		return 0;


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
				return - i * RuleBook.baseScore * 8;
			}
		}
		return 0;
	}
}

public class FailSucks : IRule
{
	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player)
	{
		if (lastMove == KeyAction.Miss) {
			return -RuleBook.baseScore;
		}
		if (lastMove == KeyAction.Fail) {
			return -RuleBook.baseScore * 4;
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
				return 1 * RuleBook.baseScore;
			} else {
				return 3 * RuleBook.baseScore;
			}
		}

		return 0;
	}
}

public class GiantsRules : BaseRule
{
	public GiantsRules() {
		goodFactor = 3.0f;
		badFactor = 1.0f;
		good.Add (KeyAction.Paddle);
		good.Add (KeyAction.Reach);
		bad.Add (KeyAction.Twist);
		bad.Add (KeyAction.Down);
	}

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player) {
		if (wooee.size == CharacterSize.Giant) {
			return getDelta (player);
		}
		return 0;
	}
}


public class DwarfRules : BaseRule
{
	public DwarfRules() {
		goodFactor = 3.0f;
		badFactor = 1.0f;
		good.Add (KeyAction.Reach);
		good.Add (KeyAction.Twist);
		bad.Add (KeyAction.Down);
		bad.Add (KeyAction.Paddle);
	}

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player) {
		if (wooee.size == CharacterSize.Dwarf) {
			return getDelta (player);
		}
		return 0;
	}
}

public class RedRules : BaseRule
{
	public RedRules() {
		goodFactor = 3.0f;
		badFactor = 1.0f;
		good.Add (KeyAction.Paddle);
		good.Add (KeyAction.Twist);
		bad.Add (KeyAction.Reach);
		bad.Add (KeyAction.Down);
	}

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player) {
		if (wooee.color == CharacterColor.Red) {
			return getDelta (player);
		}
		return 0;
	}
}

public class BlueRules : BaseRule
{
	public BlueRules() {
		goodFactor = 3.0f;
		badFactor = 1.0f;
		good.Add (KeyAction.Twist);
		good.Add (KeyAction.Paddle);
		bad.Add (KeyAction.Down);
		bad.Add (KeyAction.Reach);
	}

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player) {
		if (wooee.color == CharacterColor.Blue) {
			return getDelta (player);
		}
		return 0;
	}
}

public class NightRules : BaseRule
{
	public NightRules() {
		goodFactor = 3.0f;
		badFactor = 1.0f;
		good.Add (KeyAction.Paddle);
		good.Add (KeyAction.Paddle);
		bad.Add (KeyAction.Reach);
		bad.Add (KeyAction.Reach);
	}

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player) {
		if (wooee.env == EnvironmentType.Night) {
			return getDelta (player);
		}
		return 0;
	}
}

public class DayRules : BaseRule
{
	public DayRules() {
		goodFactor = 3.0f;
		badFactor = 1.0f;
		good.Add (KeyAction.Twist);
		good.Add (KeyAction.Twist);
		bad.Add (KeyAction.Down);
		bad.Add (KeyAction.Down);
	}

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player) {
		if (wooee.env == EnvironmentType.Night) {
			return getDelta (player);
		}
		return 0;
	}
}

public class FezRules : BaseRule
{
	public FezRules() {
		goodFactor = 3.0f;
		badFactor = 1.0f;
		good.Add (KeyAction.Paddle);
		good.Add (KeyAction.Reach);
		good.Add (KeyAction.Twist);
		bad.Add (KeyAction.Down);
	}

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player) {
		if (wooee.trait == CharacterTrait.Fez) {
			return getDelta (player);
		}
		return 0;
	}
}

public class BowtieRules : BaseRule
{
	public BowtieRules() {
		goodFactor = 3.0f;
		badFactor = 1.0f;
		good.Add (KeyAction.Reach);
		good.Add (KeyAction.Down);
		good.Add (KeyAction.Paddle);
		bad.Add (KeyAction.Twist);
	}

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player) {
		if (wooee.trait == CharacterTrait.Bowtie) {
			return getDelta (player);
		}
		return 0;
	}
}

public class StacheRules : BaseRule
{
	public StacheRules() {
		goodFactor = 3.0f;
		badFactor = 1.0f;
		good.Add (KeyAction.Twist);
		good.Add (KeyAction.Paddle);
		good.Add (KeyAction.Down);
		bad.Add (KeyAction.Reach);
	}

	public float getAffectionDelta(KeyAction lastMove, float accuracy, WooeeController wooee, PlayerController player) {
		if (wooee.trait == CharacterTrait.Stache) {
			return getDelta (player);
		}
		return 0;
	}
}

public class RuleBook : MonoBehaviour, IRule {
    IRule[] rules;

	public static float baseScore = 0.005f;

	public RuleBook() {
		rules = new IRule[] {
			new WellTimedMove(),
			new RepetativePenalty(),
			new FailSucks(),
			new Combos(),
			new GiantsRules(),
			new DwarfRules(),
			new RedRules(),
			new BlueRules(),
			new NightRules(),
			new DayRules(),
			new FezRules(),
			new BowtieRules(),
			new StacheRules()
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

