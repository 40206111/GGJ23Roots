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

    public enum eGameState
    {
        SetUp,
        PreStart,
        Paused,
        Running,
        Ended
    }

    [HideInInspector]
    public eGameState State = eGameState.SetUp;


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
        State = eGameState.SetUp;
    }


    private void Update()
    {

        switch (State)
        {
            case eGameState.SetUp:
                GameGrid.Instance.GenerateGrid();
                State = eGameState.PreStart;
                break;

            case eGameState.PreStart:
                if (Input.GetButtonDown("Root"))
                {
                    State = eGameState.Running;
                    StartCoroutine(EnMan.DoInfiniteEnemySpawn());
                }
                break;
            case eGameState.Running:
#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.I))
                {
                    EnMan.SpawnEnemies(1);
                }
#endif
                break;
            default:
                break;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"Generating new grid {GameGrid.Instance.Width}x{GameGrid.Instance.Height}");
            GameGrid.Instance.GenerateGrid();
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

    public void EnemyRooted(EnemyMover enemy)
    {
        EnMan.ActiveEnemies.Remove(enemy);
    }

}
