using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Enemy stuff
    [SerializeField]
    EnemyData TheEnemyData;
    EnemyManager EnMan;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Too many {this.GetType()} instances");
        }
        Instance = this;
    }

    private void Start()
    {
        EnMan = new EnemyManager(TheEnemyData);
    }


    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.I))
        {
            EnMan.SpawnEnemies(1);
        }
#endif
    }

}
