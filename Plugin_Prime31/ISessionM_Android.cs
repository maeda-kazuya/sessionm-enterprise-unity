using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SessionMMiniJSON;

/*!
 * SessionM Android Native Implementation.
 */ 
#if UNITY_ANDROID
public class ISessionM_Android : ISessionM
{	
	private SessionM sessionMGameObject;
	private ISessionMCallback callback;
	private SessionMEventListener listener;
	
	private static AndroidJavaObject androidInstance;
	private AndroidJavaClass sessionMObject = new AndroidJavaClass("com.sessionm.unity.SessionMPlugin");
	
	private Boolean isPresented = false;
	
	public ISessionM_Android(SessionM sessionMParent)
	{
		sessionMGameObject = sessionMParent;
		
		initAndroidInstance();
		
		CreateListenerObject();
		
		SetShouldAutoUpdateAchievementsList(SessionM.shouldAutoUpdateAchievementsList);
		SetMessagesEnabled(SessionM.shouldEnableMessages);
		SetLogLevel(sessionMParent.logLevel);
		if (SessionM.serviceRegion == ServiceRegion.Custom) {
			SetServerType(SessionM.serverURL);
		} else {
			SetServiceRegion(SessionM.serviceRegion);
		}
		if (SessionM.shouldAutoStartSession && sessionMGameObject.androidAppId != null) {
			StartSession(sessionMGameObject.androidAppId);
		}
	}
	
	private void CreateListenerObject()
	{
		listener = sessionMGameObject.gameObject.AddComponent<SessionMEventListener>();
		
		sessionMObject.CallStatic("setCallbackGameObjectName", sessionMGameObject.gameObject.name);

		listener.SetNativeParent(this);
		
		if(callback != null) {
			listener.SetCallback(callback);
		}
	}
	
	public void StartSession(string appId)
	{
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			if(appId != null) {
				androidInstance.Call("startSession", activityObject, appId);
			} else if(sessionMGameObject.androidAppId != null) {
				androidInstance.Call("startSession", activityObject, sessionMGameObject.androidAppId);
			}
		}
	}
	
	public SessionState GetSessionState()
	{
		SessionState state = SessionState.Stopped;
		
		using (AndroidJavaObject stateObject = androidInstance.Call<AndroidJavaObject>("getSessionState")) {
			string stateName = stateObject.Call<string>("name");
			if(stateName.Equals("STOPPED")) {
				state = SessionState.Stopped;
			} else if(stateName.Equals("STARTED_ONLINE")) {
				state = SessionState.StartedOnline;
			} else if(stateName.Equals("STARTED_OFFLINE")) {
				state = SessionState.StartedOffline;
			}
		}
		
		return state;
	}
	
	public string GetUser()
	{
		return sessionMObject.CallStatic<string>("getUser");
	}

	public bool LogInUserWithEmail(string email, string password)
	{
		return sessionMObject.CallStatic<bool>("logInUserWithEmail", email, password);
	}

	public void LogOutUser()
	{
		sessionMObject.CallStatic("logOutUser");
	}

	public bool SignUpUser(string email, string password, string birthYear, string gender, string zipCode)
	{
		return sessionMObject.CallStatic<bool>("signUpUser", email, password, birthYear, gender, zipCode);
	}

	public void SetUserOptOutStatus(bool status)
	{
		sessionMObject.CallStatic("setUserOptOutStatus", status);
	}
	
	public void SetShouldAutoUpdateAchievementsList(bool shouldAutoUpdate)
	{
		sessionMObject.CallStatic("setShouldAutoUpdateAchievementsList", shouldAutoUpdate);
	}

	public void SetSessionAutoStartEnabled(bool autoStart)
	{
		sessionMObject.CallStatic("setSessionAutoStartEnabled", autoStart);
	}

	public bool IsSessionAutoStartEnabled()
	{
		return sessionMObject.CallStatic<bool>("isSessionAutoStartEnabled");
	}
	
	public void UpdateAchievementsList()
	{
		sessionMObject.CallStatic("updateAchievementsList");
	}
	
	public int GetUnclaimedAchievementCount()
	{
		return sessionMObject.CallStatic<int>("getUnclaimedAchievementCount");
	}
	
	public string GetUnclaimedAchievementData()
	{
		return sessionMObject.CallStatic<string>("getUnclaimedAchievementJSON");
	}
	
	
	public void LogAction(string action)
	{
		androidInstance.Call("logAction", action);
	}
	
	public void LogAction(string action, int count)
	{
		androidInstance.Call("logAction", action, count);
	}

	public void LogAction(string action, int count, Dictionary<string, object> payloads)
	{
		string payloadsJSON = Json.Serialize(payloads);
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			activityObject.Call("logAction", action, count, payloadsJSON);
		}
	}
	
	public bool PresentActivity(ActivityType type)
	{
		using (AndroidJavaObject activityType = GetAndroidActivityTypeObject(type)) {
			isPresented = androidInstance.Call<bool>("presentActivity", activityType);
		}
		return isPresented;
	}
	
	public void DismissActivity()
	{
		if (isPresented) {
			androidInstance.Call ("dismissActivity");
			isPresented = false;
		}
	}
	
	public bool IsActivityPresented()
	{
		bool presented = false;
		presented = androidInstance.Call<bool>("isActivityPresented");
		
		return presented;
	}
	
	public bool IsActivityAvailable(ActivityType type)
	{
		bool available = false;
		using (AndroidJavaObject activityType = GetAndroidActivityTypeObject(type)) {
			available = sessionMObject.CallStatic<bool>("isActivityAvailable", activityType);
		}
		return available;
	}
	
	public void SetLogLevel(LogLevel level)
	{
		// use logcat on Android
	}
	
	public LogLevel GetLogLevel()
	{
		return LogLevel.Off;
	}

	public string GetSDKVersion()
	{
		return androidInstance.Call<string>("getSDKVersion");
	}
	
	public string GetRewards()
	{
		return sessionMObject.CallStatic<string>("getRewardsJSON");
	}

	public string GetMessagesList()
	{
		return sessionMObject.CallStatic<string>("getMessagesList");
	}

	public void SetMessagesEnabled(bool enabled)
	{
		sessionMObject.CallStatic("setMessagesEnabled", enabled);
	}
	
	public void SetMetaData(string data, string key)
	{
		androidInstance.Call("setMetaData", key, data);
	}

	public bool AuthenticateWithToken(string provider, string token)
	{
		return androidInstance.Call<bool>("authenticateWithToken", provider, token);
	}

	public void SetServiceRegion(ServiceRegion serviceRegion)
	{
		//Always 0 for now
		sessionMObject.CallStatic("setServiceRegion", 0);
	}

	public void SetServerType(string url)
	{
		sessionMObject.CallStatic("setServerType", url);
	}

	public void SetAppKey(string appKey)
	{
		sessionMObject.CallStatic("setAppKey", appKey);
	}
	
	public void NotifyPresented()
	{
		isPresented = sessionMObject.CallStatic<bool>("notifyCustomAchievementPresented");
	}
	
	public void NotifyDismissed()
	{
		if (isPresented) {
			sessionMObject.CallStatic("notifyCustomAchievementCancelled");
		}
	}
	
	public void NotifyClaimed()
	{
		if (isPresented) {
			sessionMObject.CallStatic("notifyCustomAchievementClaimed");
			isPresented = false;
		}
	}

	public void PresentTierList()
	{
		sessionMObject.CallStatic("presentTierList");
	}

	public string GetTiers()
	{
		return sessionMObject.CallStatic<string>("getTiers");
	}
	
	public double GetApplicationMultiplier()
	{
		return sessionMObject.CallStatic<double>("getApplicationMultiplier");
	}

	public void UpdateOffers()
	{
		sessionMObject.CallStatic("fetchOffers");
	}

	public string GetOffers()
	{
		return sessionMObject.CallStatic<string>("getOffers");
	}

	public void FetchContent(string contentID, bool isExternalID)
	{
		sessionMObject.CallStatic("fetchContent", contentID, isExternalID);
	}

	public void SetCallback(ISessionMCallback callback)
	{
		this.callback = callback;
		listener.SetCallback(callback);
	}
	
	public ISessionMCallback GetCallback()
	{
		return this.callback;
	}
	
	// MonoBehavior
	public AndroidJavaObject GetCurrentActivity()
	{
		using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			return playerClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
	}
	
	private AndroidJavaObject GetAndroidActivityTypeObject(ActivityType type)
	{
		if(Application.platform != RuntimePlatform.Android) {
			return null;
		}
		
		using (AndroidJavaClass typeClass = new AndroidJavaClass("com.sessionm.api.SessionM$ActivityType")) {
			string typeString = null;
			if(type == ActivityType.Achievement) {
				typeString = "ACHIEVEMENT";
			} else if(type == ActivityType.Portal) {
				typeString = "PORTAL";
			}
			
			AndroidJavaObject activityType = typeClass.CallStatic<AndroidJavaObject>("valueOf", typeString);
			return activityType;
		}
	}
	
	protected static void initAndroidInstance()
	{
		using (AndroidJavaClass sessionMClass = new AndroidJavaClass("com.sessionm.api.SessionM")) {
			androidInstance = sessionMClass.CallStatic<AndroidJavaObject>("getInstance");
		}
	}
}
#endif
