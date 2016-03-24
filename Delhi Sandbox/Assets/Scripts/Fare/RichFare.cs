using UnityEngine;

public class RichFare : Fare
{
    private float ReasonableSpeed = 4f;

    public RichFare() : base()
    {
        type = "Rich";
    }

    public override FareResponse GetResponse (float pathDistance, float journeyTime, float price)
    {
        FareResponse response = new FareResponse();
        float speed = FareTools.getSpeedMph(pathDistance, journeyTime);
        if (speed >= ReasonableSpeed) {
            response.Payment = price * speed / ReasonableSpeed;
            response.verbal = "Keep the change!";
        } else if (speed >= 0.5f * ReasonableSpeed) {
            response.Payment = price;
            response.verbal = "That was adequate.";
        } else 
        {
            response.Payment = 0.5f * price;
            response.verbal = "Forget it! You're lucky I'm paying you at all!";
        }
        return response;
    }
}
