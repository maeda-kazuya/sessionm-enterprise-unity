using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class AchievementData : IAchievementData
{	
	private string identifier = null;
	private string importID = null;
	private string instructions = null;
	private string achievementIconURL = null;
	private string action = null;
	private string name = null;
	private string message = null;
	private string limitText = null;
	private int mpointValue;
	private bool isCustom;
	private DateTime lastEarnedDate;
	private int timesEarned;
	private int unclaimedCount;
	private int distance;

	/*! Creates a new achievement with the specified parameters. */	
	public AchievementData(string identifier, string importID, string instructions, string achievementIconURL, string action, string name, string message, string limitText, int mpointValue, bool isCustom, DateTime lastEarnedDate, int timesEarned, int unclaimedCount, int distance)
	{
		this.identifier = identifier;
		this.importID = importID;
		this.instructions = instructions;
		this.achievementIconURL = achievementIconURL;
		this.action = action;
		this.name = name;
		this.message = message;
		this.limitText = limitText;
		this.mpointValue = mpointValue;
		this.isCustom = isCustom;
		this.lastEarnedDate = lastEarnedDate;
		this.timesEarned = timesEarned;
		this.unclaimedCount = unclaimedCount;
		this.distance = distance;
	}

	/*! Returns achievement ID. */
	public string GetIdentifier() { return identifier; }
	/*! Returns the ID assigned to the achievement in the csv file exported from the SessionM Developer Portal. */
	public string GetImportID() { return importID; }
	/*! Returns instructions explaining how to earn the achievement. */
	public string GetInstructions() { return instructions; }
	/*! Returns achievement's icon URL. */
	public string GetAchievementIconURL() { return achievementIconURL; }
	/*! Returns the name of the action performed by the user to earn the achievement. */
	public string GetAction() { return action; }
	/*! Returns title used on achievement display. */
	public string GetName() { return name; }
	/*! Returns message used on achievement display. */
	public string GetMessage() { return message; }
	/*! Returns a description of how many times the achievement can be earned (e.g. "Once" or "1 time every day"). */
	public string GetLimitText() { return limitText; }
	/*! Returns the amount of points the user earns when the achievement is claimed. */
	public int GetMpointValue() { return mpointValue; }
	/*! Returns whether achievement uses a custom, developer-defined display. */
	public bool GetIsCustom() { return isCustom; }
	/*! Returns the date of when the user last earned the achievement. */
	public DateTime GetLastEarnedDate() { return lastEarnedDate; }
	/*! Returns the number of times the user has earned the achievement. */
	public int GetTimesEarned() { return timesEarned; }
	/*! Returns the current amount of this achievement that the user has earned, but not yet claimed. Returns -1 if achievement is not accessed from UserData.GetAchievementsList(). */
	public int GetUnclaimedCount() { return unclaimedCount; }
	/*! Returns the number of actions required by the user to earn a new achievement. Returns -1 if achievement can't be earned in the current session. */
	public int GetDistance() { return distance; }
}
