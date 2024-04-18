using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinCount; //counter for coin, can be referenced anywhere
    public Text coinText; //UI text for the coin
    void Start()
    {

    }


    void Update()
    {
        coinText.text = ":" + coinCount.ToString();
    }
}