using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SessionMSampleGUI : MonoBehaviour {
	[System.Serializable]
	public class GUISet
	{
		public GUIState guiState;
		public CanvasGroup canvas;
		public Button button;
	}

	public SessionMSample sample;

	public GUISet[] guiSets;

	//Output Labels
	public Text optOutLabel;
	public Text isRegisteredLabel;
	public Text isLoggedInLabel;
	public Text pointBalanceLabel;
	public Text unclaimedAchCountLabel;
	public Text unclaimedAchValueLabel;
	public Text tierLabel;
	public Text tierPercentage;
	public Text tierAnniversary;
	public Text contentFetch;

	//Filliable Text Forms
	public Text authenticateProvider;
	public Text authenticateTokenString;
	public Text fetchContentID;
	public Text manualLogActionName;
	public Text manualLogActionCount;
	public Text manualLogActionPayloadJSON;

	public Text tiersText;

	public GridLayoutGroup rewardsGrid;
	public RewardObject rewardObjectPrefab;

	public enum GUIState
	{
		USER,
		REWARDS,
		TIER,
		AUTH_WITH_TOKEN,
		FETCH_CONTENT
	}

	private GUIState currentState;
	public GUIState CurrentState {
		get { return currentState; }
		set 
		{ 
			currentState = value;
			UpdateGUIState();
		}
	}

	private void UpdateGUIState()
	{
		Debug.Log (currentState);
		foreach (GUISet gui in guiSets) {
			bool isCurrent = (gui.guiState == currentState);
			gui.canvas.alpha = isCurrent ? 1.0f : 0.0f;
			gui.button.interactable = !isCurrent;
		}
	}

	public void OnPopulateTiers(Tier[] tiers)
	{
		Debug.Log("OnPopulateTiers");
		tiersText.text = "Available Tiers: \n";

		for(int i = 0; i < tiers.Length; i++) {
			tiersText.text += "Tier " + i + " : " + tiers[i].name + " (Multiplier: " + tiers[i].multiplier + ")\n";
			if(tiers[i].instructions != null) {
				tiersText.text += "Instructions: " + tiers[i].instructions + "\n";
			}
		}
	}

	public void OnPopulateRewards(Reward[] rewards)
	{
		int childCount = rewardsGrid.transform.childCount;
		for (int i = childCount - 1; i >= 0; i--) {
			Debug.Log("Deleting i: " + i);
			GameObject.DestroyImmediate(rewardsGrid.transform.GetChild(i).gameObject);
		}

		for (int i = 0; i < rewards.Length; i++) {
			Reward reward = rewards[i];
			RewardObject rewardGO = (RewardObject) GameObject.Instantiate (rewardObjectPrefab);
			rewardGO.transform.SetParent(rewardsGrid.transform);
			rewardGO.transform.localScale = Vector3.one;
			rewardGO.SetReward (reward);
		}
	}

	public void OnPopulateOffers(Offer[] offers)
	{
		int childCount = rewardsGrid.transform.childCount;
		for (int i = childCount - 1; i >= 0; i--) {
			Debug.Log("Deleting i: " + i);
			GameObject.DestroyImmediate(rewardsGrid.transform.GetChild(i).gameObject);
		}

		for (int i = 0; i < offers.Length; i++) {
			Offer offer = offers[i];
			RewardObject rewardGO = (RewardObject) GameObject.Instantiate(rewardObjectPrefab);
			rewardGO.transform.SetParent(rewardsGrid.transform);
			rewardGO.transform.localScale = Vector3.one;
			rewardGO.SetOffer(offer);
		}
	}

	public void OnPopulateUser(UserData user)
	{
		optOutLabel.text = "Opt Out: " + user.IsOptedOut().ToString();
		isRegisteredLabel.text = "Is Registered: " + user.IsRegistered();
		isLoggedInLabel.text = "Is Logged In: " + user.IsLoggedIn();
		pointBalanceLabel.text = "Point Balance: " + user.GetUserPointBalance();
		unclaimedAchCountLabel.text = "Unclaimed Achievement Count: " + user.GetUnclaimedAchievementCount();
		unclaimedAchValueLabel.text = "Unclaimed Achievement Value: " + user.GetUnclaimedAchievementValue();
		tierLabel.text = "Tier Name: " + user.GetTierName();
		tierPercentage.text = "Tier Percentage: " + user.GetTierPercentage();
		tierAnniversary.text = "Tier Anniversary: " + user.GetTierAnniversaryDate();
	}

	public void OnPopulateContent(Dictionary<string, object> content)
	{
		string contentOutput = "";
		foreach(KeyValuePair<string, object> entry in content) {
			contentOutput += " : ";
			contentOutput += entry.Key + " | ";
			contentOutput += entry.Value + "\n";
		}
	}

	public void OnUserHit()
	{
		CurrentState = GUIState.USER;
	}

	public void OnRewardsHit()
	{
		CurrentState = GUIState.REWARDS;
	}

	public void OnTierHit()
	{
		CurrentState = GUIState.TIER;
	}

	public void OnFetchContent()
	{
		CurrentState = GUIState.FETCH_CONTENT;
		sample.OnFetchContent(fetchContentID.text);
	}

	public void OnManualLogAction()
	{
		var payloads = new Dictionary<string, object>();
		payloads.Add("0", (object) manualLogActionPayloadJSON.text);
		int count = int.Parse(manualLogActionCount.text);
		sample.OnLogActionWithPayloads(manualLogActionName.text, count, payloads);
	}

	public void OnAuthenticateToken()
	{
		CurrentState = GUIState.AUTH_WITH_TOKEN;
	}

	//Unity Lifecycle
	private void Awake()
	{
		CurrentState = GUIState.USER;
	}
}
