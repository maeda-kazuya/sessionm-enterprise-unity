using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tier 
{
	private string tier;
	private string name;
	private string instructions;
	private double multiplier;
	private int startValue;
	private int endValue;
	private double securePercent;
	private int requiredPoints;

	/*! Creates a new tier with the specified parameters. */
	public Tier(string tier, string name, string instructions, double multiplier, int startValue, int endValue, double securePercent, int requiredPoints)
	{
		this.tier = tier;
		this.name = name;
		this.instructions = instructions;
		this.multiplier = multiplier;
		this.startValue = startValue;
		this.endValue = endValue;
		this.securePercent = securePercent;
		this.requiredPoints = requiredPoints;
	}

	/*! Returns short tier name used for string matching. */
	public string GetTier() { return tier; }
	/*! Returns full tier name used for displaying UI. */
	public string GetName() { return name; }
	/*! Returns instructions needed for a user to reach the tier. */
	public string GetInstructions() { return instructions; }
	/*! Returns multiplier bonus applied to points received by a user who has reached the tier. */
	public double GetMultiplier() { return multiplier; }
	/*! Returns amount of points needed for a user to reach the tier. */
	public int GetStartValue() { return startValue; }
	/*! Returns amount of points needed for a user to reach the next tier. */
	public int GetEndValue() { return endValue; }
	/*! Returns fraction of points towards the next tier a user must earn to remain in the current tier. */
	public double GetSecurePercent() { return securePercent; }
	/*! Returns amount of points a user must earn to remain in the tier. */
	public int GetRequiredPoints() { return requiredPoints; }
}
