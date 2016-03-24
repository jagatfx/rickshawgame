using UnityEngine;

public class PoorFare : Fare
{

    private float ReasonableSpeed = 3f;

    public PoorFare() : base()
    {
        type = "Poor";
    }

    public override FareResponse GetResponse (float pathDistance, float journeyTime, float price)
    {
        FareResponse response = new FareResponse();
        float speed = FareTools.getSpeedMph(pathDistance, journeyTime);
        if (speed >= ReasonableSpeed) {
            response.Payment = price * 1.2f;
            response.verbal = "Thanks a bunch!";
        } else {
            response.Payment = price * 1.1f;
            response.verbal = "Thanks.";
        }
        return response;
    }
}
