using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class UserData
{	
	private bool isOptedOut;
	private bool isRegistered;
	private bool isLoggedIn;
	private int pointBalance;
	private long tierPointBalance;
	private int unclaimedAchievementCount;
	private int unclaimedAchievementValue;
	private List<AchievementData> achievements;
	private List<AchievementData> achievementsList;
	private string tierIdentifier;
	private string tierName;
	private string tierPercentage;
	private string tierAnniversaryDate;
	private string startTier;

	/*! Creates a new user with the specified parameters. */
	public UserData(bool isOptedOut, bool isRegistered, bool isLoggedIn, int pointBalance, long tierPointBalance, int unclaimedAchievementCount, int unclaimedAcheivementValue, List<AchievementData> achievements, List<AchievementData> achievementsList, string tierIdentifier, string tierName, string tierPercentage, string tierAnniversaryDate, string startTier)
	{
		this.isOptedOut = isOptedOut;
		this.isRegistered = isRegistered;
		this.isLoggedIn = isLoggedIn;
		this.pointBalance = pointBalance;
		this.tierPointBalance = tierPointBalance;
		this.unclaimedAchievementCount = unclaimedAchievementCount;
		this.unclaimedAchievementValue = unclaimedAcheivementValue;
		this.achievements = achievements;
		this.achievementsList = achievementsList;
		this.tierIdentifier = tierIdentifier;
		this.tierName = tierName;
		this.tierPercentage = tierPercentage;
		this.tierAnniversaryDate = tierAnniversaryDate;
		this.startTier = startTier;
	}

	/*! Returns whether user is opted out of the rewards system. */
	public bool IsOptedOut() { return this.isOptedOut; }
	/*! Returns whether user is registered or anonymous. */
	public bool IsRegistered() { return this.isRegistered; }
	/*! Returns whether user is currently logged in. */
	public bool IsLoggedIn() { return this.isLoggedIn; }
	/*! Returns user's current point balance. */
	public int GetUserPointBalance() { return this.pointBalance; }
	/*! Returns user's current tier point balance. */
	public long GetUserTierPointBalance() { return this.tierPointBalance; }
	/*! Returns how many achievements that the user has earned, but not claimed. */
	public int GetUnclaimedAchievementCount() { return this.unclaimedAchievementCount; }
	/*! Returns total point value of achievements that the user has earned, but not claimed. */
	public int GetUnclaimedAchievementValue() { return this.unclaimedAchievementValue; }
	/*! Returns list of achievements that the user can earn. */
	public List<AchievementData> GetAchievements() { return this.achievements; }
	/*! Returns history of user's earned achievements. */
	public List<AchievementData> GetAchievementsList() { return this.achievementsList; }
	/*! Returns identifier for user's current rewards tier. */
	public string GetTierIdentifier() { return this.tierIdentifier; }
	/*! Returns user's current rewards tier. */
	public string GetTierName() { return this.tierName; }
	/*! Returns user's progress towards next rewards tier. */
	public string GetTierPercentage() { return this.tierPercentage; }
	/*! Returns when user reached their current tier. */
	public string GetTierAnniversaryDate() { return this.tierAnniversaryDate; }
	/*! Returns user's tier at the start of the year. */
	public string GetStartTier() { return this.startTier; }
}
