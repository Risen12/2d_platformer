using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public void CollectCoin(Coin coin)
    { 
        Destroy(coin.gameObject);
    }
}
