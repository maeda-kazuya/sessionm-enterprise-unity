using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
