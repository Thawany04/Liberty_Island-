using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeObs
{
    public static event Action<float> timeEvent; // Evento para tempo extra

    public static void OnTime(float extraTime)
    {
        timeEvent?.Invoke(extraTime); // Invoca o evento com o valor do tempo extra
    }
}