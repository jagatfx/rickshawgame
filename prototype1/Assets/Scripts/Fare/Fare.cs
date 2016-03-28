using UnityEngine;

public interface Fare
{
    FareResponse GetResponse (float directDistance, float journeyTime, float price);
}
