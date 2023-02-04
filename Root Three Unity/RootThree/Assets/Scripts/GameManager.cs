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

    //Player stuff
    [SerializeField]
    GameObject PlayerPrefab;
    PlayerMover Player;


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

    public void GridReadyForPlayer()
    {
        if (Player == null)
        {
            Player = Instantiate(PlayerPrefab).GetComponent<PlayerMover>();
        }

        float halfWidth = (GameGrid.Instance.Width - 1) * 0.5f;
        float halfHeight = (GameGrid.Instance.Height - 1) * 0.5f;

        Player.transform.position = new Vector3(halfWidth, 0.6f, halfHeight);
    }

}
