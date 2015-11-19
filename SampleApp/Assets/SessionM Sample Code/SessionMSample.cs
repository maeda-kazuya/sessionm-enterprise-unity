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
			gui.OnPopulateRewards(sessionM.GetRewards());
			gui.OnPopulateTiers(sessionM.GetTiers());
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

	//Unity Lifecycle

	private void Awake()
	{
		//Set service region before SessionM instance is activated
		// SessionM.SetServiceRegion(ServiceRegion.USA);
		SessionM.SetServerType("https://api.tour-sessionm.com");
		SessionM.SetShouldAutoUpdateAchievementsList(true);
		SessionM.SetMessagesEnabled(true);
		SessionM.SetSessionAutoStartEnabled(false);
		sessionM.gameObject.SetActive(true);
	}


	private void OnEnable()
	{
		//Assign useful events to Helper Functions in the class.
		sessionM.StartSession("0bfeb00013f0f634420a04ed5806a66a58d49d8b");
		SessionMEventListener.NotifySessionStateChanged += NotifySessionStateChanged;
		SessionMEventListener.NotifySessionError += NotifySessionError;
		SessionMEventListener.NotifyUnclaimedAchievementDataUpdated += NotifyUnclaimedAchievementDataUpdated;
		SessionMEventListener.NotifyUserInfoChanged += UserChanged;
		sessionMSDK.text = "SDK VERSION: " + sessionM.GetSDKVersion();
		Test();
		UserChanged(null);
	}

	public Tier[] tiers;

	private void Test()
	{
		string s = 
			"[{" +
			"\"tier\": \"silver\"," +
			"\"name\": \"Silver\"," +
			"\"instructions\": null" +
			"}," +
			"{" +
			"\"tier\": \"gold\"," +
			"\"name\": \"Gold\"," +
			"\"instructions\": null" +
			"}," +
			"{" +
			"\"tier\": \"platinum\"," +
			"\"name\": \"Platinum\"," +
			"\"instructions\": null" +
			"}]";
		
		var dictList = MiniJSON.Json.Deserialize(s) as List<System.Object>;
		Debug.Log(dictList.Count);
		Tier[] tierArray = new Tier[dictList.Count];

		for(int i = 0; i < dictList.Count; i++) {
			Debug.Log(dictList[i].GetType());
			var dict = dictList[i] as Dictionary<System.String, System.Object>;
			string tier = (string) dict["tier"];
			string name = (string) dict["name"];
			string instructions = (string) dict["instructions"];
			tierArray[i] = new Tier(tier, name, instructions);
		}
		tiers = tierArray;
	}

//	public Reward r;
//	private void Test()
//	{
//		Debug.Log ("Testing Rewards");
//		string s = "{" +
//		           "\"id\": 7749," +
//		           "\"name\": \"$2 iTunes Gift Card\"," +
//		           "\"points\": 5000," +
//		           "\"image\": \"https://cdn2.getm.pt/1/9c/2/648115/itunes.png\"," +
//		           "\"type\": \"Offer\"," +
//		           "\"expires_at\": null," +
//		           "\"url\": \"https://api.tour-sessionm.com/offers/7749\"," +
//		           "\"tier\": \"platinum\"" +
//		           "}";
//		
//		var dict = MiniJSON.Json.Deserialize(s) as Dictionary<string, object>;
//		long id = (System.Int64) dict["id"];
//		string name = (string) dict["name"];
//		long points = (System.Int64) dict["points"];
//		string imageURL = (string)dict["image"];
//		string type = (string) dict["type"];
//		string expiresAt = (string) dict["expires_at"];
//		string url = (string) dict["url"];
//		string tier = (string)dict ["tier"];
//
//		r = new Reward ((int)id, name, (int)points, imageURL, url, tier, type, expiresAt);
//		Reward[] rewards = new Reward[2];
//		rewards [0] = r;
//		rewards [1] = r;
//		gui.OnPopulateRewards (rewards);
//	}

	private void OnDisable()
	{
		//Clean Up the events in case this object is destroyed.
		SessionMEventListener.NotifySessionStateChanged -= NotifySessionStateChanged;
		SessionMEventListener.NotifySessionError -= NotifySessionError;
		SessionMEventListener.NotifyUnclaimedAchievementDataUpdated -= NotifyUnclaimedAchievementDataUpdated;
		SessionMEventListener.NotifyUserInfoChanged -= UserChanged;
	}

}
