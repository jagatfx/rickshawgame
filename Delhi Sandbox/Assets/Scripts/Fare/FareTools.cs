using UnityEngine;

public class FareTools {
	public static float getSpeedMph(float Length, float Duration)
	{
		return Length / (Duration);
	}
	// This is a dumb solution - there is probably some better way,
	// without using the usual double Math function with recasting
	public static float roundTwoDecimals(float price)
	{
		return Mathf.Round (price * 100) / 100;
	}
}
