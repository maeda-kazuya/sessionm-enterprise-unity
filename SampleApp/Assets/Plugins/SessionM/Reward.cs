using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[System.Serializable]
public class Reward
{
	public int id;
	public string name;
	public int points;
	public string imageURL;
	public string url;
	public string expiresAt;
	public string type;
	public string tier;

	public Reward(int id, string name, int points, string imageURL, string url, string tier, string type, string expiresAt)
	{
		this.id = id;
		this.name = name;
		this.points = points;
		this.imageURL = imageURL;
		this.url = url;
		this.tier = tier;
		this.expiresAt = expiresAt;
		this.type = type;
	}
}
