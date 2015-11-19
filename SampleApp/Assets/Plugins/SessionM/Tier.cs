using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tier 
{
	public string tier;
	public string name;
	public string instructions;

	public Tier(string tier, string name, string instructions)
	{
		this.tier = tier;
		this.name = name;
		this.instructions = instructions;
	}
}
