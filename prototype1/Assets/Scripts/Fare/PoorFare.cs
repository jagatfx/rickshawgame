using UnityEngine;

public class PoorFare : Fare
{

    private const float ReasonableSpeed = 3.0f;
    private const float FastRideMultiple = 1.2f;
    private const float SlowRideMultiple = 1.1f;

    public FareResponse GetResponse (float directDistance, float journeyTime, float price)
    {
        FareResponse response = new FareResponse();
        float speed = FareTools.getSpeedMph(directDistance, journeyTime);
        if (speed >= ReasonableSpeed)
        {
            response.Payment = price * FastRideMultiple;
            response.verbal = "Thanks a bunch!";
        }
        else
        {
            response.Payment = price * SlowRideMultiple;
            response.verbal = "Thanks.";
        }
        return response;
    }
}
