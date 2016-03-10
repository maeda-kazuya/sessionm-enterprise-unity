using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

[System.Serializable]
public class Offer
{
	private string id;
	private string name;
	private string offerType;
	private string description;
	private string startTime;
	private string endTime;
	private string tier;
	private string status;
	private string logo;
	private string terms;
	private Dictionary<string, object> data;
	private Dictionary<string, object>[] options;
	private long points;
	private long weight;
	private bool isFeatured;

	public Offer(Dictionary<string, object> offer)
	{
		if (offer.ContainsKey("id")) {
			this.id = Convert.ToString(offer["id"]);
		}
		if (offer.ContainsKey("name")) {
			this.name = (string) offer["name"];
		}
		if (offer.ContainsKey("type")) {
			this.offerType = (string) offer["type"];
		}
		if (offer.ContainsKey("description")) {
			this.description = (string) offer["description"];
		}
		if (offer.ContainsKey("start_time")) {
			this.startTime = (string) offer["start_time"];
		}
		if (offer.ContainsKey("end_time")) {
			this.endTime = (string) offer["end_time"];
		}
		if (offer.ContainsKey("tier")) {
			this.tier = (string) offer["tier"];
		}
		if (offer.ContainsKey("status")) {
			this.status = (string) offer["status"];
		}
		if (offer.ContainsKey("logo")) {
			this.logo = (string) offer["logo"];
		}
		if (offer.ContainsKey("terms")) {
			this.terms = (string) offer["terms"];
		}
		if (offer.ContainsKey("data")) {
			this.data = offer["data"] as Dictionary<string, object>;
		}
		if (offer.ContainsKey("options")) {
			List<object> optionsList = offer["options"] as List<object>;
			this.options = optionsList.ToArray() as Dictionary<string, object>[];
		}
		if (offer.ContainsKey("points")) {
			this.points = Convert.ToInt64(offer["points"]);
		}
		if (offer.ContainsKey("weight")) {
			this.weight = Convert.ToInt64(offer["weight"]);
		}
		if (offer.ContainsKey("featured")) {
			this.isFeatured = Convert.ToBoolean(offer["featured"]);
		}
	}

	public string GetID() { return id; }
	public string GetName() { return name; }
	public string GetOfferType() { return offerType; }
	public string GetDescription() { return description; }
	public string GetStartTime() { return startTime; }
	public string GetEndTime() { return endTime; }
	public string GetTier() { return tier; }
	public string GetStatus() { return status; }
	public string GetLogo() { return logo; }
	public string GetTerms() { return terms; }
	public Dictionary<string, object> GetData() { return data; }
	public Dictionary<string, object>[] GetOptions() { return options; }
	public long GetPoints() { return points; }
	public long GetWeight() { return weight; }
	public bool GetIsFeatured() { return isFeatured; }
}
