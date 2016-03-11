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

	/*! Creates a new offer with the specified data. */
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

	/*! Returns offer ID. */
	public string GetID() { return id; }
	/*! Returns offer name. */
	public string GetName() { return name; }
	/*! Returns offer type. */
	public string GetOfferType() { return offerType; }
	/*! Returns offer description. */
	public string GetDescription() { return description; }
	/*! Returns date of when offer became available for redemption. */
	public string GetStartTime() { return startTime; }
	/*! Returns date of when offer will no longer be available for redemption. */
	public string GetEndTime() { return endTime; }
	/*! Returns the tier the user must achieve to redeem the offer. */
	public string GetTier() { return tier; }
	/*! Returns offer status. */
	public string GetStatus() { return status; }
	/*! Returns offer image URL. */
	public string GetLogo() { return logo; }
	/*! Returns offer redemption terms. */
	public string GetTerms() { return terms; }
	/*! Returns additional data associated with offer. */
	public Dictionary<string, object> GetData() { return data; }
	/*! Returns additional options associated with offer. */
	public Dictionary<string, object>[] GetOptions() { return options; }
	/*! Returns amount of points needed to redeem offer. */
	public long GetPoints() { return points; }
	/*! Returns offer weight (can be used for sorting). */
	public long GetWeight() { return weight; }
	/*! Returns whether the offer is listed as a featured offer in the Rewards Store. */
	public bool GetIsFeatured() { return isFeatured; }
}
