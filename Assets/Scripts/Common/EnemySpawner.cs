using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Random enemy spawning system

    [Header("Enemy Toggles")]
    public Enemy[] Enemies;
    Enemy[][] pooledEnemies;

    // Start is called before the first frame update
    void Start()
    {
        // Object pooling
        pooledEnemies = new Enemy[Enemies.Length][];
        for (int i = 0; i <= Enemies.Length - 1; i++)
        {
            pooledEnemies[i] = new Enemy[Enemies[i].MaxAmount];
            for (int j = 0; j <= Enemies[i].MaxAmount - 1; j++)
            {
                var newEnemy = Instantiate(Enemies[i], transform);
                pooledEnemies[i][j] = newEnemy;
                //if (j > 0) { newEnemy.gameObject.SetActive(false); }
                newEnemy.gameObject.SetActive(false);
            }

            StartCoroutine(EnemySpawnLoop(pooledEnemies[i]));
        }
    }

    IEnumerator EnemySpawnLoop(Enemy[] enemyList)
    {
        foreach (Enemy enemy in enemyList)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                float waitTime = Random.Range(enemy.MinSpawnDelay, enemy.MaxSpawnDelay);
                yield return new WaitForSeconds(waitTime);
                enemy.gameObject.SetActive(true);
            }
        }

        yield return null;
        StopCoroutine(EnemySpawnLoop(enemyList));
        StartCoroutine(EnemySpawnLoop(enemyList));
    }
}
