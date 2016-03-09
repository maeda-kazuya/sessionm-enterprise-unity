using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class RewardObject : MonoBehaviour 
{
	public Text title;
	public Text tier;
	public Text points;
	public Text type;
	public RawImage image;
	public string url;

	public void SetReward(Reward reward)
	{
		title.text = reward.name;
		tier.text = "Tier: " + reward.tier;
		type.text = "Type: " + reward.type;
		points.text = reward.ToString ();
		this.url = reward.url;

		StartCoroutine (DownloadImage (reward.imageURL));
	}

	public void SetOffer(Dictionary<string, object> offer)
	{
		title.text = (string) offer["name"];
		type.text = "Type: " + (string) offer["type"];
		points.text = Convert.ToString((Int64) offer["points"]);

		if(offer["tier"] != null) {
			tier.text = "Tier: " + (string) offer["tier"];
		}

		StartCoroutine (DownloadImage ((string) offer["logo"]));
	}

	private IEnumerator DownloadImage(string imageURL)
	{
		WWW imageDownload = new WWW (imageURL);
		yield return imageDownload;
		image.texture = imageDownload.texture;
	}

	public void OnRewardClicked()
	{
		Debug.Log ("Opening URL: " + url);
		Application.OpenURL(url);
	}
}
