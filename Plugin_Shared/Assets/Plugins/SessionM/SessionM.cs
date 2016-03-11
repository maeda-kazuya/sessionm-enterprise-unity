using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MiniJSON;

/*!
 * SessionM service implementation. Implements and provides access (via SessionM.GetInstance()) to the SessionM service.
 * The SessionM Class is a MonoBehaviour singleton. Drop the game object in your scene and set it up instead of trying to instantiate it via code.
 * Put the SessionM object in your project as early as possible. The object will survive loads, so there's never a reason to put it in more than one place in your scenes.
 */ 
public class SessionM : MonoBehaviour
{
	private ISessionMCallback callback;
	private static SessionM instance;
	
	/*! iOS Application ID linked to SampleApp game object. */
	public string iosAppId;
	/*! Android Application ID linked to SampleApp game object. */
	public string androidAppId;
	/*! iOS debug log level linked to SampleApp game object. */
	public LogLevel logLevel;	

	/*! Returns the SessionM singleton instance. */	
	public static SessionM GetInstance() 
	{
		if(instance == null) {
			SessionM existingSessionM = GameObject.FindObjectOfType<SessionM>();
			if(existingSessionM == null) {
				Debug.LogError("There is no SessionM GameObject set up in the scene.  Please add one and set it up as per the SessionM Plug-In Documentation.");
				return null;
			}
			existingSessionM.SetSessionMNative();
			instance = existingSessionM;
		}
		
		return instance;
	}

	/*!
	 * The SessionM service region used to determine request routes - must be set before starting session. Default value is ServiceRegion.USA.
	 *
	 * Note: using SetServerType will cause serviceRegion to be set to ServiceRegion.Custom
	 */
	public static ServiceRegion serviceRegion = ServiceRegion.USA;
	/*! The server that the SessionM plug-in routes requests to - must be set before starting session. Default value is 'https://api.sessionm.com'. */
	public static string serverURL = "https://api.sessionm.com";
	/*! Determines whether the user's achievements list will be updated automatically. Default value is false. */
	public static bool shouldAutoUpdateAchievementsList = false;
	/*! Determines whether access to the messages API is enabled. Default value is false. */
	public static bool shouldEnableMessages = false;
	/*! Determines whether the session is started automatically when the SessionM game object is activated. Default value is true. */
	public static bool shouldAutoStartSession = true;

	/*! Call this method before starting the session to set the service region. Do not use with SetServerType. */
	public static void SetServiceRegion(ServiceRegion region)
	{
		serviceRegion = region;
	}

	/*! Call this method before starting the session to set the server URL. Do not use with SetServiceRegion. */
	public static void SetServerType(string url)
	{
		serviceRegion = ServiceRegion.Custom;
		serverURL = url;
	}

	/*! Sets whether the user's achievements list will be updated automatically. Default value is false. */
	public static void SetShouldAutoUpdateAchievementsList(bool shouldAutoUpdate)
	{
		shouldAutoUpdateAchievementsList = shouldAutoUpdate;
	}

	/*! Sets whether access to the messages API is enabled. Default value is false. */
	public static void SetMessagesEnabled(bool shouldEnable)
	{
		shouldEnableMessages = shouldEnable;
	}

	/*! Determines whether the session is started automatically when the SessionM game object is activated. Default value is true. */
	public static void SetSessionAutoStartEnabled(bool autoStartEnabled)
	{
		shouldAutoStartSession = autoStartEnabled;
	}

	/*! Returns shouldAutoStartSession. */
	public static bool IsSessionAutoStartEnabled()
	{
		return shouldAutoStartSession;
	}

	private ISessionM sessionMNative;
	/*!
	 * Instantiates the appropiate native interface to be used for the current platform.
	 *
	 * iOS: ISessionM_IOS
	 * Android: ISessionM_Android
	 * All others: ISessionM_Dummy (the dummy simply catches all calls coming into SessionM from unsupported platforms)
	 *
	 * If you need to modify how SessionM is interacting with either iOS or Android natively, please look in the respective interface class.
	 */
	public ISessionM SessionMNative 
	{
		get { return sessionMNative; }
	}
	
	/*!
	 * Returns SessionM's current session state.
	 *
	 * Can be: Stopped, Started Online, Started Offline
	 */
	public SessionState GetSessionState()
	{
		return sessionMNative.GetSessionState();
	}

	/*! Manually starts a session with the specified application API key. */
	public void StartSession(string appKey)
	{
		sessionMNative.StartSession(appKey);
	}

	/*! Returns user's number of unclaimed achievements. */
	public int GetUnclaimedAchievementCount()
	{
		return sessionMNative.GetUnclaimedAchievementCount();
	}

	/*! Returns current user data. */
	public UserData GetUserData()
	{
		UserData userData = null;
		string userDataJSON = null;

		userDataJSON = sessionMNative.GetUser();

		if(userDataJSON == null) {
			return null;
		}

		userData = GetUserData(userDataJSON);

		return userData;
	}

        /*! Sends a request to the server to log in the user with the specified email and password. Returns whether the request can be sent. */
	public bool LogInUserWithEmail(string email, string password) {
		return sessionMNative.LogInUserWithEmail(email, password);
	}

	/*! Logs out the current user. */
	public void LogOutUser() {
		sessionMNative.LogOutUser();
	}

        /*! Sends a request to the server to sign up the user with the specified parameters. Returns whether the request can be sent. */
	public bool SignUpUser(string email, string password, string birthYear, string gender, string zipCode) {
		return sessionMNative.SignUpUser(email, password, birthYear, gender, zipCode);
	}

	/*! Sets current user opt-out status locally. */
	public void SetUserOptOutStatus(bool status){
		sessionMNative.SetUserOptOutStatus(status);
	}

	/*! Manually updates the user's achievements list. */
	public void UpdateAchievementsList()
	{
		sessionMNative.UpdateAchievementsList();
	}

	/*! Returns current unclaimed achievement data. */
	public AchievementData GetUnclaimedAchievementData() 
	{
		IAchievementData achievementData = null;
		string achievementJSON = null;
		
		achievementJSON = sessionMNative.GetUnclaimedAchievementData();
		
		if(achievementJSON == null) {
			return null;
		}
		
		achievementData = GetAchievementData(achievementJSON);
		return achievementData as AchievementData;
	}
	
	/*! Logs an event for the specified achievement action. */
	public void LogAction(string action) 
	{
		sessionMNative.LogAction(action);
	}
	
	/*! Logs multiple events for the specified achievement action. */
	public void LogAction(string action, int count) 
	{
		sessionMNative.LogAction(action, count);
	}
	
	/*! Logs multiple events for the specified achievement action while supplying additional developer-defined data that is associated with the action. */
	public void LogAction(string action, int count, Dictionary<string, object> payloads)
	{
		sessionMNative.LogAction(action, count, payloads);
	}

	/*! Presents UI activity of specified type. */
	public bool PresentActivity(ActivityType type)
	{
		return sessionMNative.PresentActivity(type);
	}
	
	/*! Returns whether an activity of the specified type is available. */
	public bool IsActivityAvailable(ActivityType type)
	{
		return sessionMNative.IsActivityAvailable(type);
	}
	
	/*! Displays the rewards portal. */
	public bool ShowPortal()
	{
		return PresentActivity(ActivityType.Portal);
	}
	
	/*! Returns the version number of the SessionM Unity Plug-in. */
	public string GetSDKVersion()
	{
		return sessionMNative.GetSDKVersion();
	}

	/*! This method is deprecated. Please use GetOffers instead. */
	public Reward[] GetRewards()
	{
		return GetRewardData(sessionMNative.GetRewards());
	}

	/*! Returns a list of campaign messages. */
	public string GetMessagesList()
	{
		return sessionMNative.GetMessagesList();
	}

	/*! Returns the current iOS debug log level. */
	public LogLevel GetLogLevel()
	{
		return sessionMNative.GetLogLevel();
	}

	/*! Sets the iOS debug log level. For Android, use logcat instead. */
	public void SetLogLevel(LogLevel level)
	{
		sessionMNative.SetLogLevel(level);
	}

	/*! Returns whether a UI activity is currently presented. */	
	public bool IsActivityPresented()
	{
		return sessionMNative.IsActivityPresented();
	}

	/*! Sets session metadata to be sent on session start. */
	public void SetMetaData(string data, string key)
	{
		sessionMNative.SetMetaData(data, key);
	}

	/*! Sends a request to the server to authenticate a user with the specified OAuth token string from the specified provider. Returns whether request can be sent. */
	public bool AuthenticateWithToken(string provider, string token)
	{
		return sessionMNative.AuthenticateWithToken(provider, token);
	}

	/*! Call this method before starting the session to set the app key. */
	public void SetAppKey(string appKey)
	{
		sessionMNative.SetAppKey(appKey);
	}
	
	/*! Notifies the SessionM SDK that a custom achievement has been presented. */
	public void NotifyPresented()
	{
		sessionMNative.NotifyPresented();
	}
	
	/*! Notifies the SessionM SDK that a custom achievement has been dismissed. */
	public void NotifyDismissed()
	{
		sessionMNative.NotifyDismissed();
	}
	
	/*! Notifies the SessionM SDK that a custom achievement has been claimed. */
	public void NotifyClaimed()
	{
		sessionMNative.NotifyClaimed();
	}

	/*! Dismisses currently presented UI activity. */	
	public void DismissActivity()
	{
		sessionMNative.DismissActivity();
	}

	/*! Presents list of tiers the user can reach. Note: this method is deprecated. */
	public void PresentTierList()
	{
		sessionMNative.PresentTierList();
	}

	/*! Returns the list of tiers the user can reach. */
	public Tier[] GetTiers()
	{
		return GetTierData(sessionMNative.GetTiers());
	}
	
	/*! Sends a request to the server to update the cached list of offers that the user can redeem. Offers are returned in NotifyOffersUpdated callback. */
	public void UpdateOffers()
	{
		sessionMNative.UpdateOffers();
	}

	/*! Returns the cached list of offers that the user can redeem. */
	public Offer[] GetOffers()
	{
		return GetOfferData(sessionMNative.GetOffers());
	}

	/*! Sends a request to the server to fetch the data for the content with the specified ID (external IDs are developer-defined). Content data is returned in NotifyContentFetched callback. */
	public void FetchContent(string contentID, bool isExternalID)
	{
		sessionMNative.FetchContent(contentID, isExternalID);
	}

	/*! Sets the object to use for executing Unity callback implementations. */
	public void SetCallback(ISessionMCallback callback)
	{
		sessionMNative.SetCallback(callback);
	}
	
	/*! Returns callback object. */
	public ISessionMCallback GetCallback() 
	{
		return sessionMNative.GetCallback();
	}
	
	// Unity Lifecycle
	private void Awake() 
	{
		SetSessionMNative();
		GameObject.DontDestroyOnLoad(this.gameObject);
		instance = this;
		SetLogLevel (logLevel);
	}
	
	private void SetSessionMNative()
	{
		if(sessionMNative != null)
			return;
		
		//Assign the appropiate Native Class to handle method calls here.
		#if UNITY_EDITOR
		sessionMNative = new ISessionM_Dummy();
		#elif UNITY_IOS
		sessionMNative = new ISessionM_iOS(this);
		#elif UNITY_ANDROID
		sessionMNative = new ISessionM_Android(this);
		#else
		sessionMNative = new ISessionM_Dummy();
		#endif
	}
	
	/*! This is a useful method you can call whenever you need to parse a JSON string into the IAchievementData custom class. */
	public static IAchievementData GetAchievementData(string jsonString) 
	{
		Dictionary<string, object> achievementDict = Json.Deserialize(jsonString) as Dictionary<string,object>;

		long mpointValue = (Int64)achievementDict["mpointValue"];
		long timesEarned = (Int64)achievementDict["timesEarned"];
		long unclaimedCount = (Int64)achievementDict["unclaimedCount"];
		long distance = (Int64)achievementDict["distance"];
		bool isCustom = (bool)achievementDict["isCustom"];
		string identifier = (string)achievementDict["identifier"];
		string importID = (string)achievementDict["importID"];
		string instructions = (string)achievementDict["instructions"];
		string achievementIconURL = (string)achievementDict["achievementIconURL"];
		string action = (string)achievementDict["action"];
		string name = (string)achievementDict["name"];
		string message = (string)achievementDict["message"];
		string limitText = (string)achievementDict["limitText"];
		DateTime lastEarnedDate = new DateTime((Int64)achievementDict["lastEarnedDate"], DateTimeKind.Utc);

		IAchievementData achievementData = new AchievementData(identifier, importID, instructions, achievementIconURL, action, name, message, limitText, (int)mpointValue, isCustom, lastEarnedDate, (int)timesEarned, (int)unclaimedCount, (int)distance);
		return achievementData;
	}

	private static UserData GetUserData(string jsonString)
	{
		Dictionary<string, object> userDict = Json.Deserialize(jsonString) as Dictionary<string, object>;
		bool isOptedOut = (bool)userDict["isOptedOut"];
		bool isRegistered = (bool)userDict["isRegistered"];
		bool isLoggedIn = (bool)userDict["isLoggedIn"];
		long userPointBalance = (Int64)userDict["getPointBalance"];
		long unclaimedAchievementCount = (Int64)userDict["getUnclaimedAchievementCount"];
		long unclaimedAchievementValue = (Int64)userDict["getUnclaimedAchievementValue"];

		string achievementsJSON = (string)userDict["getAchievementsJSON"];
		string[] achievementsJSONArray = UnpackJSONArray(achievementsJSON);

		AchievementData[] achievementsArray = new AchievementData[achievementsJSONArray.Length];
		for(int i = 0; i < achievementsJSONArray.Length; i++) {
			string achievement = achievementsJSONArray[i];
			if(achievement == "")
			{
				break;
			}
			achievementsArray[i] = GetAchievementData(achievement) as AchievementData;
		}
		List<AchievementData> achievements = new List<AchievementData>(achievementsArray);

		string achievementsListJSON = (string)userDict["getAchievementsListJSON"];
		string[] achievementsListJSONArray = UnpackJSONArray(achievementsListJSON);

		AchievementData[] achievementsListArray = new AchievementData[achievementsListJSONArray.Length];
                for(int i = 0; i < achievementsListJSONArray.Length; i++) {
                        string achievement = achievementsListJSONArray[i];
			if(achievement == "")
			{
				break;
			}
                        achievementsListArray[i] = GetAchievementData(achievement) as AchievementData;
                }
		List<AchievementData> achievementsList = new List<AchievementData>(achievementsListArray);

		string tierName = (string)userDict["getTierName"];
		string tierPercentage = (string)userDict["getTierPercentage"];
		string tierAnniversaryDate = (string)userDict["getTierAnniversaryDate"];
		UserData userData = new UserData(isOptedOut, isRegistered, isLoggedIn, (int)userPointBalance, (int)unclaimedAchievementCount, (int)unclaimedAchievementValue, achievements, achievementsList, tierName, tierPercentage, tierAnniversaryDate);

		return userData;
	}

	private static Tier[] GetTierData(string jsonString)
	{
		List<object> dictList = Json.Deserialize(jsonString) as List<object>;
		Tier[] tierArray = new Tier[dictList.Count];

		for(int i = 0; i < dictList.Count; i++) {
			Dictionary<string, object> dict = dictList[i] as Dictionary<string, object>;
			string tier = (string) dict["tier"];
			string name = (string) dict["name"];
			string instructions = (string) dict["instructions"];
			double multiplier = Convert.ToDouble(dict["multiplier"]);
			tierArray[i] = new Tier(tier, name, instructions, multiplier);
		}

		return tierArray;
	}

	private static Reward[] GetRewardData(string jsonString) 
	{
		List<object> dictList = Json.Deserialize(jsonString) as List<object>;
		Reward[] rewardArray = new Reward[dictList.Count];

		for (int i = 0; i < dictList.Count; i++) {
			Dictionary<string, object> dict = dictList[i] as Dictionary<string, object>;
			long id = (System.Int64) dict["id"];
			string name = (string) dict["name"];
			long points = (System.Int64) dict["points"];
			string imageURL = (string)dict["image"];
			string type = (string) dict["type"];
			string expiresAt = (string) dict["expires_at"];
			string url = (string) dict["url"];
			string tier = (string)dict ["tier"];

			rewardArray[i] = new Reward ((int)id, name, (int)points, imageURL, url, tier, type, expiresAt);
		}

		return rewardArray;
	}
	
	private static Offer[] GetOfferData(string jsonString)
	{
		List<object> dictList = Json.Deserialize(jsonString) as List<object>;
		Offer[] offerArray = new Offer[dictList.Count];

		for (int i = 0; i < dictList.Count; i++) {
			Dictionary<string, object> dict = dictList[i] as Dictionary<string, object>;
			Offer offer = new Offer(dict);
			offerArray[i] = offer;
		}

		return offerArray;
	}

	private static string[] UnpackJSONArray(string json)
	{
		string[] separatorArray = new string[] {"__"};
		string[] JSONArray = json.Split(separatorArray, StringSplitOptions.None);
		return JSONArray;
	}
}
