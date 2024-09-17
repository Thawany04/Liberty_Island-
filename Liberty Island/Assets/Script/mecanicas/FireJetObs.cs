using System;
using UnityEngine;

public static class FireJetObs
{
    public static event Action<int> OnFireDamage;

    public static void FireDamage(int damage)
    {
        OnFireDamage?.Invoke(damage);
    }
}
