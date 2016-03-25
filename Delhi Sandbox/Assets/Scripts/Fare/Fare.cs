﻿using UnityEngine;

public class Fare
{
    public GameObject destination;
    public string type;

    public virtual FareResponse GetResponse (float directDistance, float journeyTime, float price)
    {
        return null;
    }
}
