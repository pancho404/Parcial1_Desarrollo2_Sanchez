using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectCoin : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject player;


    [SerializeField] TMP_Text coinText;
    private int currentCoins = 0;
    void Start()
    {
        coinText.text = "COINS: " + currentCoins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Destroy(gameObject);
        }
        AddCoin(1);
    }

    private void AddCoin(int value)
    {
        currentCoins += value;
        coinText.text = "COINS: " + currentCoins.ToString();
    }
}
