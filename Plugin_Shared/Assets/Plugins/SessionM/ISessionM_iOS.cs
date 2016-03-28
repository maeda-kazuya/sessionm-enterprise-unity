using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SessionMMiniJSON;

/*!
 * SessionM iOS Native Implementation.
 */ 
#if UNITY_IOS
public class ISessionM_iOS : ISessionM
{	
	private SessionM sessionMGameObject;
	private ISessionMCallback callback;
	private SessionMEventListener listener;
	
	[DllImport ("__Internal")]
	protected static extern void SMSetCallbackGameObjectName(string gameObjectName);
	public ISessionM_iOS(SessionM sessionMParent)
	{
		sessionMGameObject = sessionMParent;
		
		SetShouldAutoUpdateAchievementsList(SessionM.shouldAutoUpdateAchievementsList);
		SetMessagesEnabled(SessionM.shouldEnableMessages);
		SetLogLevel(sessionMParent.logLevel);
		if (SessionM.serviceRegion == ServiceRegion.Custom) {
			SetServerType(SessionM.serverURL);
		} else {
			SetServiceRegion(SessionM.serviceRegion);
		}
		if (SessionM.shouldAutoStartSession && sessionMParent.iosAppId != null) {
			StartSession(sessionMParent.iosAppId);
		}
		
		CreateListenerObject();
	}
	
	private void CreateListenerObject()
	{
		listener = sessionMGameObject.gameObject.AddComponent<SessionMEventListener>();
		
		SMSetCallbackGameObjectName(sessionMGameObject.gameObject.name);
		Debug.Log("Setting Callback Object: " + sessionMGameObject.gameObject.name);
		listener.SetNativeParent(this);
		
		if(callback != null) {
			listener.SetCallback(callback);
		}
	}
	
	[DllImport ("__Internal")]
	private static extern void SMStartSession(string appId);
	public void StartSession(string appId)
	{
		if(appId != null) {
			SMStartSession(appId);
		} else if(sessionMGameObject.iosAppId != null) {
			SMStartSession(sessionMGameObject.iosAppId);
		}
	}
	
	[DllImport ("__Internal")]
	private static extern int SMGetSessionState();
	public SessionState GetSessionState()
	{
		return (SessionState)SMGetSessionState();
	}

	[DllImport ("__Internal")]
	private static extern string SMGetUserJSON();
	public string GetUser() 
	{
		return SMGetUserJSON(); 
	}

	[DllImport ("__Internal")]
	private static extern bool SMLogInUserWithEmail(string email, string password);
	public bool LogInUserWithEmail(string email, string password)
	{
		return SMLogInUserWithEmail(email, password);
	}

	[DllImport ("__Internal")]
	private static extern void SMLogOutUser();
	public void LogOutUser()
	{
		SMLogOutUser();
	}

	[DllImport ("__Internal")]
	private static extern bool SMSignUpUser(string email, string password, string birthYear, string gender, string zipCode);
	public bool SignUpUser(string email, string password, string birthYear, string gender, string zipCode)
	{
		return SMSignUpUser(email, password, birthYear, gender, zipCode);
	}

	[DllImport ("__Internal")]
	private static extern void SMPlayerDataSetUserOptOutStatus(bool status);
	public void SetUserOptOutStatus(bool status) 
	{
		SMPlayerDataSetUserOptOutStatus(status);
	}

	[DllImport ("__Internal")]
	private static extern void SMSetShouldAutoUpdateAchievementsList(bool shouldAutoUpdate);
	public void SetShouldAutoUpdateAchievementsList(bool shouldAutoUpdate)
	{
		SMSetShouldAutoUpdateAchievementsList(shouldAutoUpdate);
	}

	[DllImport ("__Internal")]
	private static extern void SMUpdateAchievementsList();
	public void UpdateAchievementsList()
	{
		SMUpdateAchievementsList();
	}

	[DllImport ("__Internal")]
	private static extern int SMPlayerDataGetUnclaimedAchievementCount();
	public int GetUnclaimedAchievementCount()
	{
		return SMPlayerDataGetUnclaimedAchievementCount();
	}
	
	[DllImport ("__Internal")]
	private static extern string SMGetUnclaimedAchievementJSON();
	public string GetUnclaimedAchievementData() 
	{
		return SMGetUnclaimedAchievementJSON(); 
	}
	
	[DllImport ("__Internal")]
	private static extern void SMLogAction(string action);
	public void LogAction(string action) 
	{
		SMLogAction(action);		
	}
	
	[DllImport ("__Internal")]
	private static extern void SMLogActions(string action, int count);
	public void LogAction(string action, int count) 
	{
		SMLogActions(action, count);		
	}

	[DllImport ("__Internal")]
	private static extern void SMLogActionsWithPayloads(string action, int count, string payloadsJSON);
	public void LogAction(string action, int count, Dictionary<string, object> payloads)
	{
		string payloadsJSON = Json.Serialize(payloads);
		SMLogActionsWithPayloads(action, count, payloadsJSON);
	}
	
	[DllImport ("__Internal")]
	private static extern bool SMPresentActivity(int type);
	public bool PresentActivity(ActivityType type)
	{
		return SMPresentActivity((int)type);
	}
	
	[DllImport ("__Internal")]
	private static extern void SMDismissActivity();
	public void DismissActivity()
	{
		SMDismissActivity();
	}
	
	[DllImport ("__Internal")]
	private static extern bool SMIsActivityPresented();
	public bool IsActivityPresented()
	{
		return SMIsActivityPresented();
	}
	
	[DllImport ("__Internal")]
	private static extern bool SMIsActivityAvailable(int type); 
	public bool IsActivityAvailable(ActivityType type)
	{
		return SMIsActivityAvailable((int)type); 
	}
	
	[DllImport ("__Internal")]
	private static extern void SMSetLogLevel(int level);
	public void SetLogLevel(LogLevel level)
	{
		SMSetLogLevel((int)level);	
	}
	
	[DllImport ("__Internal")]
	private static extern int SMGetLogLevel();
	public LogLevel GetLogLevel()
	{
		return (LogLevel)SMGetLogLevel();
	}

	[DllImport ("__Internal")]
	private static extern void SMSetServiceRegion(int region);
	public void SetServiceRegion(ServiceRegion region)
	{
		SMSetServiceRegion((int)region);
	}

	[DllImport ("__Internal")]
	private static extern void SMSetServerType(string url);
	public void SetServerType(string url)
	{
		SMSetServerType(url);
	}

	public void SetAppKey(string appKey)
	{
	}
	
	[DllImport ("__Internal")]
	private static extern string SMGetSDKVersion();
	public string GetSDKVersion()
	{
		return SMGetSDKVersion();
	}

	[DllImport ("__Internal")]
	private static extern string SMGetRewardsJSON();
	public string GetRewards()
	{
		string rewardsJSON = SMGetRewardsJSON();
		return rewardsJSON;
	}
	
	[DllImport ("__Internal")]
	private static extern void SMSetMessagesEnabled(bool enabled);
	public void SetMessagesEnabled(bool enabled)
	{
		SMSetMessagesEnabled(enabled);
	}

	[DllImport ("__Internal")]
	private static extern string SMGetMessagesList();
	public string GetMessagesList()
	{
		string messages = SMGetMessagesList();
		return messages;
	}

	[DllImport ("__Internal")]
	private static extern void SMSetMetaData(string data, string key);
	public void SetMetaData(string data, string key)
	{
		SMSetMetaData(data, key);
	}
	
	[DllImport ("__Internal")]
	private static extern bool SMAuthenticateWithToken(string provider, string token);
	public bool AuthenticateWithToken(string provider, string token)
	{
		return SMAuthenticateWithToken(provider, token);
	}

	[DllImport ("__Internal")]
	private static extern void SMNotifyCustomAchievementPresented();
	public void NotifyPresented()
	{
		SMNotifyCustomAchievementPresented();
	}
	
	[DllImport ("__Internal")]
	private static extern void SMNotifyCustomAchievementDismissed();
	public void NotifyDismissed()
	{
		SMNotifyCustomAchievementDismissed();
	}
	
	[DllImport ("__Internal")]
	private static extern void SMNotifyCustomAchievementClaimed();
	public void NotifyClaimed()
	{
		SMNotifyCustomAchievementClaimed();
	}

	[DllImport ("__Internal")]
	private static extern void SMPresentTierList();
	public void PresentTierList()
	{
		SMPresentTierList();
	}

	[DllImport ("__Internal")]
	private static extern string SMGetTiers();
	public string GetTiers()
	{
		string tiers = SMGetTiers();
		return tiers;
	}

	[DllImport ("__Internal")]
	private static extern double SMGetApplicationMultiplier();
	public double GetApplicationMultiplier()
	{
		return SMGetApplicationMultiplier();
	}
	
	[DllImport ("__Internal")]
	private static extern void SMUpdateOffers();
	public void UpdateOffers()
	{
		SMUpdateOffers();
	}

	[DllImport ("__Internal")]
	private static extern string SMGetOffers();
	public string GetOffers()
	{
		return SMGetOffers();
	}

	[DllImport ("__Internal")]
	private static extern void SMFetchContentWithID(string contentID, bool isExternalID);
	public void FetchContent(string contentID, bool isExternalID)
	{
		SMFetchContentWithID(contentID, isExternalID);
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
	
}
#endif
