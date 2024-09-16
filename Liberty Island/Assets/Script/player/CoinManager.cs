using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int CoinCaunt = 0;
    public void AddCoin(int i)
    {
        CoinCaunt += i;
        Gamer_Controler.Instance.Updatescore(1);
    }

    private void OnEnable()
    {
        CoinObs.coin += AddCoin;
    }

    private void OnDisable()
    {
        CoinObs.coin -= AddCoin;
    }
}
