using UnityEngine;

public class FareResponse
{
	private float payment;

	public float Payment {
		get {
			return payment;
		}
		set {
			payment = FareTools.roundTwoDecimals (value);
		}
	}

	public string verbal;
}
