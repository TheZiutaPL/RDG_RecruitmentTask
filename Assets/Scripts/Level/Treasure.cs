using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] private List<Coin> coinPrefabs = new List<Coin>();
    [SerializeField] private float coinSpreadRange;
    [SerializeField] private int diggedCoinsMin, diggedCoinsMax;
    private List<GameObject> buriedCoins = new List<GameObject>();

    public void SetTreasureContent(int overallValue, Transform coinsParent = null)
    {
        //Sorts coins by their value (asc)
        coinPrefabs.Sort((a, b) => a.Value.CompareTo(b.Value));

        int spawnedValue = 0;
        while (spawnedValue < overallValue) 
        {
            int leftValue = overallValue - spawnedValue;

            //Decrements coin index if a value of a coin of current index is too big
            int index = 0;
            for (index = Random.Range(0, coinPrefabs.Count); coinPrefabs[index].Value > leftValue; index--)
            {
                //It means there couldn't spawn any coin
                if (index == 0)
                    break;
            }

            //Spawn coins and hides them
            GameObject temp = Instantiate(coinPrefabs[index], transform.position, Quaternion.identity, coinsParent).gameObject;
            temp.SetActive(false);
            buriedCoins.Add(temp);

            int addedValue = coinPrefabs[index].Value;
            if(addedValue == 0)
            {
                Debug.LogError("There is a coin with 0 value, which is not intended and could cause an infinite loop.");
                break;
            }

            spawnedValue += coinPrefabs[index].Value;
        }
    }

    private void SpreadCoinAtRandom(Transform coinTransform)
    {
        coinTransform.position = transform.position + (Vector3)(coinSpreadRange * Random.insideUnitCircle);
    }

    [ContextMenu("Dig up coins")]
    public void DigUpCoins()
    {
        if (buriedCoins.Count == 0)
            return;

        int diggedCoins = Random.Range(diggedCoinsMin, diggedCoinsMax);
        int iterations = Mathf.Min(diggedCoins, buriedCoins.Count) - 1;
        for (int i = iterations; i >= 0; i--)
        {
            SpreadCoinAtRandom(buriedCoins[i].transform);
            buriedCoins[i].SetActive(true);
            buriedCoins.RemoveAt(i);
        }

        if (buriedCoins.Count == 0)
            Destroy(gameObject);
    }
}
