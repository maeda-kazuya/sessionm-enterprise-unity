using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*! UI Activity type */
public enum ActivityType
{
	// User achievement.  
	Achievement = 1,
	// User portal.
	Portal = 2
}

/*! Debug log levels (iOS only) */
public enum LogLevel
{
	Off = 0,
	Info = 1, 
	Debug = 2
}

/*! Session state */
public enum SessionState
{
	Stopped = 0,
	StartedOnline = 1, 
	StartedOffline = 2
}

/*! Service region */
public enum ServiceRegion
{
	Unknown = 0,
	Japan = 1,
	USA = 2,
	Custom = 3
}

/*!
 * User actions.
 * User action within SessionM UI activity. Identifies events such as user engaging with achievement prompt, etc.
 */ 
public enum UserAction
{
	AchievementViewAction = 100,
	AchievementEngagedAction = 101,
	AchievementDismissedAction = 102,
	SponsorContentViewedAction = 103,
	SponsorContentEngagedAction = 104,
	SponsorContentDismissedAction = 105,
	PortalViewedAction = 106,
	SignInAction = 107,
	SignOutAction = 108,
	RegisteredAction = 109,
	PortalDismissedAction = 110,
	RedeemedRewardAction = 111,
	CheckinCompletedAction = 112,
	VirtualItemRewardAction = 113
}

/*!
 * SessionM service interface. 
 *
 * In order to enable session M service in your Unity application make sure to do the following:
 * 1. Register with Session M developer portal and obtain valid application ID for your application. 
 * 2. Copy SessionM Unity plugin into your application project. More information about Unity plugin integration can be found at http://unity3d.com/support/documentation/Manual/Plugins.html.
 * 3. In the Unity editor associate SessionM.cs script with a game object in your application. Any game object can be used for this purpose but should preferrably be a long-lived one, such as main camera. 
 *    This enables service callbacks into ISessionMCallback instance from the native application layer. 
 * 3. Optionally, use SessionMManager.cs to jump-start SessionM service integration. Associate this script with one of the game objects in your application and inject your application logic in 
 *    appropriate places in the script to get your SessionM enabled application up and running.   
 * 
 * Following is the basic pattern of integrating Session M service within your appliction code:
 * 1. Configure SessionM script associated with the game object with a valid application ID. 
 * 2. Set callback object to get notified about service events and, in particular, earned achievements.
 *    ISessionMCallback callback = ... <your callback object>
 *    service.SetCallback(callback);
 * 3. Log user actions in your application code earning user achievements, e.g. reaching new game level, daily visit, killing the scariest monster, etc.        
 *    service.LogAction("Level X");
 * 4. Present an UI activity, such as achievement when it becomes available, user portal or Session M program introduction at the appropriate time in your application flow. 
 *    if(service.IsActivityAvailable(ActivityType.Achievement)) {
 *        service.PresentActivity(ActivityType.Achievement);
 *    }
 * 
 * Achievement UI Customization.
 * 
 * Application can customize achievement presentation to suit its style and UI flow by implementing the following steps:
 * 1. In developer portal configure an achievement as custom.
 * 2. Use object IAchievementData to access information about achievement and method NotifyUnclaimedAchievementDataUpdated(ISessionM, IAchievementData) in ISessionMCallback interface to get notified when new achievement is earned. 
 * 3. When displaying an achievement UI make sure to call the following methods on IAchievementData object:
 *    a. NotifyPresented() when achievement alert UI has been displayed. 
 *    b. NotifyDismissed() when user or application has dismissed the achievement alert without engaging. 
 *    c. NotifyClaimed() when user claimed the achievement. 
 */ 
public interface ISessionM
{
	/*! Sets the object to use for executing Unity callback implementations. */
	void SetCallback(ISessionMCallback callback); 
	
	/*! Returns callback object. */
	ISessionMCallback GetCallback();
	
	/*! Starts session with an application identifier. */
	void StartSession(string appId);
	
	/*! Returns session state. */
	SessionState GetSessionState();

	/*! Gets Current User JSON Object (Deserializes to UserData). */
	string GetUser();

	/*! Sends a request to the server to log in the user with the specified email and password. Returns whether the request can be sent. */
	bool LogInUserWithEmail(string email, string password);

	/*! Logs out the current user. */
	void LogOutUser();

	/*! Sends a request to the server to sign up the user with the specified email and parameters. Returns whether the request can be sent. */
	bool SignUpUser(string email, string password, string birthYear, string gender, string zipCode);

	/*! Sets current user opt-out status locally. */
	void SetUserOptOutStatus(bool status);

	/*! Sets value of shouldAutoUpdateAchievementsList (default is false). */
	void SetShouldAutoUpdateAchievementsList(bool shouldAutoUpdate);

	/*! Manually updates the user's achievements list. */
	void UpdateAchievementsList();

	/*! Returns user's number of unclaimed achievements. */
	int GetUnclaimedAchievementCount();
	
	/*! Returns current unclaimed achievement data. */
	string GetUnclaimedAchievementData();
	
	/*! Logs action. Calling this method may trigger an achievement which application will be notified about via ISessionMCallback.NotifyUnclaimedAchievementDataUpdated(ISessionM,IAchievementData) callback. */
	void LogAction(string action); 
	
	/*! Logs a number of actions - equivalent of calling LogAction(string) a number of times specified in argument "count". */
	void LogAction(string action, int count);

	/*! Logs a number of actions with additional developer-defined data associated with the action. */
	void LogAction(string action, int count, Dictionary<string, object> payloads);
	
	/*! Presents UI activity of specified type if available. */
	bool PresentActivity(ActivityType type);

	/*! Dismisses currently presented UI activity. */
	void DismissActivity();
	
	/*!
	 * Returns true if specified UI activity is available for presentation, false - otherwise. User portal (ActivityType.Portal) is always available.
	 * Achievement activity (ActivityType.Achievement) become available when new achievement is earned as a result of user action.
	*/
	bool IsActivityAvailable(ActivityType type);

	/*! Returns true if UI activity is currently being presented, false - otherwise. */
	bool IsActivityPresented();
	
	/*! Returns debug log level (iOS only). */
	LogLevel GetLogLevel();

	/*! Ses debug log level (iOS only). */
	void SetLogLevel(LogLevel level);

	/*! Sets service region. */
	void SetServiceRegion(ServiceRegion region);

	/*! Sets server url. */
	void SetServerType(string url);

	/*! Sets app key. */
	void SetAppKey(string appKey);

	/*! Returns SDK version. */
	string GetSDKVersion();
	
	/*! This method is deprecated. Please use GetOffers instead. */
	string GetRewards();

	/*! Sets messages feature enabled. */
	void SetMessagesEnabled(bool enabled);

	/*! Returns a list of all messages. */
	string GetMessagesList();

	/*! Sets session metadata to be sent on session start. */
	void SetMetaData(string data, string key);
	
	/*! Attempts to authenticate user with the provided token. Returns false if either provider or token is missing. */
	bool AuthenticateWithToken(string provider, string token);

	/*! Notifies the SessionM SDK that a custom achievement has been presented. */
	void NotifyPresented();
	
	/*! Notifies the SessionM SDK that a custom achievement has been dismissed. */
	void NotifyDismissed();
	
	/*! Notifies the SessionM SDK that a custom achievement has been claimed. */
	void NotifyClaimed();

	/*! Presents list of tiers the user can reach. Note: this method is deprecated. */
	void PresentTierList();

	/*! Returns list of tiers user can reach. */
	string GetTiers();

	/*! Updates list of cached offers that the user can redeem. */
	void UpdateOffers();

	/*! Returns list of cached offers that the user can redeem. */
	string GetOffers();

	/*! Fetches content data with specified ID. */
	void FetchContent(string contentID, bool isExternalID);
}
