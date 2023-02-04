using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyData", fileName = "New EnemyData")]
public class EnemyData : ScriptableObject
{
    public List<GameObject> prefabs;

    public int FallHeight;

    public int SpawnAttempts;

    public float MinSpawDelay;
    public float MaxSpawDelay;

}
