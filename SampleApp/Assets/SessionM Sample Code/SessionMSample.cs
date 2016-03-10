using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SessionMSample : MonoBehaviour 
{
	public SessionM sessionM;

	public Text sessionMSDK;
	public Text sessionMStateLabel;

	public SessionMSampleGUI gui;

	public string action1;
	public string action2;
	public string action3;

	public AchievementToast toaster;

	//Exposed Methods

	public void OnAction1Clicked()
	{
		sessionM.LogAction(action1);
	}

	public void OnAction2Clicked()
	{
		sessionM.LogAction(action2);
	}

	public void OnAction3Clicked()
	{
		sessionM.LogAction(action3);
	}

	public void OnPortalClicked()
	{
		sessionM.ShowPortal();
	}

	//Helper Methods 

	private void NotifySessionError(int errorCode, string error)
	{
		Debug.LogError("SessionM Error Reported: " + errorCode.ToString() + " - " + error);
	}

	private void NotifySessionStateChanged(SessionState state)
	{
		Debug.Log("Event: NotifySessionStateChanged Fired");
		sessionMStateLabel.text = "SessionM State: " + state.ToString();
		if (state == SessionState.StartedOnline) {
			gui.OnPopulateTiers(sessionM.GetTiers());
			sessionM.UpdateOffers();
			sessionM.FetchContent("gsn_content_1", true);
		}
	}

	private void NotifyUnclaimedAchievementDataUpdated(IAchievementData achievementData)
	{
		Debug.Log("Recieved New Achievement: " + achievementData.GetName() + " - Worth: " + achievementData.GetMpointValue() + "\n With Message: " + achievementData.GetMessage());
		toaster.ShowAchievementToast(achievementData.GetName(), achievementData.GetMpointValue());
	}

	private void UserChanged(IDictionary<string, object> userInfo)
	{
		UserData user = SessionM.GetInstance().GetUserData();

		if(user == null)
			return;

		gui.OnPopulateUser(user);
	}

	private void OffersUpdated(Dictionary<string, object>[] offers)
	{
		gui.OnPopulateOffers(offers);
	}

	private void ContentFetched(Dictionary<string, object> content)
	{
		Debug.Log("Content fetched: " + content.Keys);
	}

	//Unity Lifecycle

	private void Awake()
	{
		//Set service region or server type before SessionM instance is activated
		SessionM.SetServerType("https://api.sessionm.com");
		SessionM.SetSessionAutoStartEnabled(false);
		sessionM.gameObject.SetActive(true);
	}


	private void OnEnable()
	{
		//Assign useful events to Helper Functions in the class.
		SessionMEventListener.NotifySessionStateChanged += NotifySessionStateChanged;
		SessionMEventListener.NotifySessionError += NotifySessionError;
		SessionMEventListener.NotifyUnclaimedAchievementDataUpdated += NotifyUnclaimedAchievementDataUpdated;
		SessionMEventListener.NotifyUserInfoChanged += UserChanged;
		SessionMEventListener.NotifyOffersUpdated += OffersUpdated;
		SessionMEventListener.NotifyContentFetched += ContentFetched;
		sessionM.StartSession("1a852d1384411299aa9b5d12254ff628198e6674");
		sessionMSDK.text = "SDK VERSION: " + sessionM.GetSDKVersion();
		UserChanged(null);
	}

	private void OnDisable()
	{
		//Clean Up the events in case this object is destroyed.
		SessionMEventListener.NotifySessionStateChanged -= NotifySessionStateChanged;
		SessionMEventListener.NotifySessionError -= NotifySessionError;
		SessionMEventListener.NotifyUnclaimedAchievementDataUpdated -= NotifyUnclaimedAchievementDataUpdated;
		SessionMEventListener.NotifyUserInfoChanged -= UserChanged;
	}

}
