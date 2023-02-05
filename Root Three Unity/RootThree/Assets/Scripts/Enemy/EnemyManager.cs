using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{

    EnemyData EnData;

    public List<EnemyMover> ActiveEnemies = new List<EnemyMover>();
    public List<EnemyMover> RootedEnemies = new List<EnemyMover>();

    float TotalSpawnTime = 0;

    public EnemyManager(EnemyData enData)
    {
        EnData = enData;
    }

    public void Reset()
    {
        for (int i = 0; i < ActiveEnemies.Count; i++)
        {
            ActiveEnemies[i].DefeatEnemy(doEffects: false);
        }
        for (int i = 0; i < RootedEnemies.Count; )
        {
            RootedEnemies[i].DefeatEnemy(doEffects: false);
        }

        ActiveEnemies.Clear();
        RootedEnemies.Clear();
        TotalSpawnTime = 0;
    }

    public IEnumerator<YieldInstruction> DoInfiniteEnemySpawn()
    {
        float timePassed = 0;

        float minDelay = EnData.MaxMinSpawDelay; 
        float maxDelay = EnData.MaxMaxSpawDelay;

        float spawnTime = Random.Range(minDelay, maxDelay);

        while (GameManager.Instance.State != GameManager.eGameState.Ended)
        {
            yield return null;
            if (GameManager.Instance.State != GameManager.eGameState.Running)
            {
                continue;
            }

            timePassed += Time.deltaTime;

            if (ActiveEnemies.Count == 0 || timePassed >= spawnTime)
            {
                SpawnEnemies(1);

                float progress = 1 - (TotalSpawnTime / EnData.TimeToMaxDifficulty);
                minDelay = EnData.MinMinSpawDelay + progress * (EnData.MaxMinSpawDelay - EnData.MinMinSpawDelay);
                maxDelay = EnData.MinMaxSpawDelay + progress * (EnData.MaxMaxSpawDelay - EnData.MinMaxSpawDelay);
                spawnTime = Random.Range(minDelay, maxDelay);
                timePassed = 0;
            }
            TotalSpawnTime += Time.deltaTime;
            TotalSpawnTime = Mathf.Clamp(TotalSpawnTime, TotalSpawnTime, EnData.TimeToMaxDifficulty);
        }
    }

    public void SpawnEnemies(int number)
    {
        for (int i = 0; i < number; i++)
        {
            int enemyId = Random.Range(0, EnData.prefabs.Count);

            int x = Random.Range(0, GameGrid.Instance.Width);
            int y = Random.Range(0, GameGrid.Instance.Height);

            Vector3 pos = new Vector3(x, EnData.FallHeight, y);
            Vector3 floorPos = pos;
            floorPos.y = -1;
            Vector3 highPos = pos;
            highPos.y += 1;
            Collider[] hits = Physics.OverlapCapsule(floorPos, highPos, 0.45f, layerMask: (1 << 3));

            bool found = false;
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j].CompareTag("Enemy"))
                {
                    found = true;
                }

                if (found)
                {
                    break;
                }
            }

            int startX = x;
            int startY = y;
            bool noSpaces = false;
            int attempts = 1;
            while (found)
            {
                if (attempts > EnData.SpawnAttempts)
                {
                    Debug.Log("No attempts left to spawn enemy");
                    noSpaces = true;
                    break;
                }

                x = x == GameGrid.Instance.Width - 1 ? 0 : x + 1;
                if (x == startX)
                {
                    y = y == GameGrid.Instance.Height - 1 ? 0 : y + 1;

                    if (y == startY)
                    {
                        Debug.Log("No spaces found for enemy");
                        noSpaces = true;
                        break;
                    }
                }

                pos = new Vector3(x, EnData.FallHeight, y);
                floorPos = pos;
                floorPos.y = -1;
                highPos = pos;
                highPos.y += 1;
                hits = Physics.OverlapCapsule(floorPos, highPos, 0.45f, layerMask: (1 << 3));

                found = false;
                for (int j = 0; j < hits.Length; j++)
                {
                    if (hits[j].CompareTag("Enemy"))
                    {
                        found = true;
                    }

                    if (found)
                    {
                        break;
                    }
                }
                attempts++;
            }
            if (noSpaces)
            {
                continue;
            }


            EnemyMover newEn = GameObject.Instantiate(EnData.prefabs[enemyId], pos, Quaternion.identity).GetComponent<EnemyMover>();

            ActiveEnemies.Add(newEn);
            break;


        }
    }


}
