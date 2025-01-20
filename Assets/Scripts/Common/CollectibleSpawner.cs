using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public Collectible collectible;
    Collectible[] pooledCollectibles; 

    // Start is called before the first frame update
    void Start()
    {
        pooledCollectibles = new Collectible[collectible.MaxAmount];
        for (int j = 0; j <= collectible.MaxAmount - 1; j++)
        {
            var newCollectible = Instantiate(collectible, transform);
            pooledCollectibles[j] = newCollectible;
            //if (j > 0) { newEnemy.gameObject.SetActive(false); }
            newCollectible.gameObject.SetActive(false);
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        foreach (Collectible collectible in pooledCollectibles)
        {
            if (!collectible.gameObject.activeInHierarchy)
            {
                float waitTime = Random.Range(collectible.MinSpawnDelay, collectible.MaxSpawnDelay);
                yield return new WaitForSeconds(waitTime);
                collectible.gameObject.SetActive(true);
            }
        }

        yield return null;
        StopCoroutine(SpawnLoop());
        StartCoroutine(SpawnLoop());
    }
}
