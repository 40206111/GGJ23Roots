using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{

    EnemyData EnData;

    List<EnemyMover> Enemies = new List<EnemyMover>();

    public EnemyManager(EnemyData enData)
    {
        EnData = enData;
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
            Collider[] hits = Physics.OverlapCapsule(floorPos, highPos, 0.5f, layerMask: (1 << 3));

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

            if (found)
            {
                continue;
            }
            

            EnemyMover newEn = GameObject.Instantiate(EnData.prefabs[enemyId], pos, Quaternion.identity).GetComponent<EnemyMover>();

            Enemies.Add(newEn);


        }
    }


}
