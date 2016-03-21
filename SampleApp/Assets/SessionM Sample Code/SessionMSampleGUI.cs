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

	public GUISet[] guiSets;

	public Text optOutLabel;
	public Text isRegisteredLabel;
	public Text isLoggedInLabel;
	public Text pointBalanceLabel;
	public Text tierPointBalanceLabel;
	public Text unclaimedAchCountLabel;
	public Text unclaimedAchValueLabel;
	public Text tierLabel;
	public Text tierPercentage;
	public Text tierAnniversary;

	public Text tiersText;

	public GridLayoutGroup rewardsGrid;
	public RewardObject rewardObjectPrefab;

	public enum GUIState
	{
		USER,
		REWARDS,
		TIER
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
		tierPointBalanceLabel.text = "Tier Point Balance: " + user.GetUserTierPointBalance();
		unclaimedAchCountLabel.text = "Unclaimed Achievement Count: " + user.GetUnclaimedAchievementCount();
		unclaimedAchValueLabel.text = "Unclaimed Achievement Value: " + user.GetUnclaimedAchievementValue();
		tierLabel.text = "Tier Name: " + user.GetTierName();
		tierPercentage.text = "Tier Percentage: " + user.GetTierPercentage();
		tierAnniversary.text = "Tier Anniversary: " + user.GetTierAnniversaryDate();
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

	//Unity Lifecycle
	private void Awake()
	{
		CurrentState = GUIState.USER;
	}
}
