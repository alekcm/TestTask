using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coin;
    public static GameObject staticCoin;

    void Start()
    {
        staticCoin = coin;
        //SpawnCoins();
    }

    public static void SpawnCoins()
    {
        int coinchance = 10;
        for (float i = -7.7f; i < 7.7f; i+=1f)
            for (float f = 4f; f > -4f; f -= 1f)
            {
                int curschance = Random.Range(0, 100);
                if (coinchance >= curschance)
                {
                    PhotonNetwork.Instantiate(staticCoin.name,new Vector2(i,f),Quaternion.identity);
                    coinchance = 10;
                }
                else
                {
                    coinchance += 10;
                }
            }
    }
}
