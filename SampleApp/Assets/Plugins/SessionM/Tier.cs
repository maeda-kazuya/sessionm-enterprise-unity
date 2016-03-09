using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tier 
{
	public string tier;
	public string name;
	public string instructions;
	public double multiplier;

	public Tier(string tier, string name, string instructions, double multiplier)
	{
		this.tier = tier;
		this.name = name;
		this.instructions = instructions;
		this.multiplier = multiplier;
	}
}
