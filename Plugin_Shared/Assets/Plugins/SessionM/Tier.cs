using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tier 
{
	/*! Returns short tier name used for string matching. */
	public string tier;
	/*! Returns full tier name used for displaying UI. */
	public string name;
	/*! Returns instructions needed for a user to reach the tier. */
	public string instructions;
	/*! Returns multiplier bonus applied to points received by a user who has reached the tier. */
	public double multiplier;

	/*! Creates a new tier with the specified parameters. */
	public Tier(string tier, string name, string instructions, double multiplier)
	{
		this.tier = tier;
		this.name = name;
		this.instructions = instructions;
		this.multiplier = multiplier;
	}
}
