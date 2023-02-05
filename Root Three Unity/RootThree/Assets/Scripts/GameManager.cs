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
    PlayerAimer PlayerAim;

    //UI Stuff
    [SerializeField]
    UIHandler TheUI;

    public enum eGameState
    {
        Uninitialised,
        SetUp,
        PreStart,
        Paused,
        Running,
        Ended
    }

    [HideInInspector]
    public eGameState State {get; private set;}
    eGameState PrePauseState = eGameState.Uninitialised;


    private void Awake()
    {
        Cursor.visible = false;
        if (Instance != null)
        {
            Debug.LogError($"Too many {this.GetType()} instances");
        }
        Instance = this;
    }

    private void Start()
    {
        EnMan = new EnemyManager(TheEnemyData);
        StartCoroutine(WaitForFirstTimeSetUp());
    }

    IEnumerator<YieldInstruction> WaitForFirstTimeSetUp()
    {
        yield return null;
        SetState(eGameState.SetUp);
    }

    public void UnPause()
    {
        if (PrePauseState == eGameState.Uninitialised)
        {
            SetState(eGameState.Running);
            return;
        }

        State = PrePauseState;
        PrePauseState = eGameState.Uninitialised;
    }


    private void Update()
    {
        switch (State)
        {
            case eGameState.PreStart:
                if (Input.GetButtonDown("Root"))
                {
                    SetState(eGameState.Running);
                }
                CheckForPause();
                break;
            case eGameState.Running:
                CheckForPause();
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

    void CheckForPause()
    {
        if (Input.GetButtonDown("Pause"))
        {
            SetState(eGameState.Paused);
        }

    }

    public void SetState(eGameState state)
    {
        if (State == state) return;

        if (state == eGameState.Paused)
        {
            PrePauseState = State;
        }

        State = state;
        TheUI?.UpdateFromGameState(State);
        switch (State)
        {
            case eGameState.Uninitialised:
                Debug.LogError("No don't set Game State to uninitialised! naughty! stop it!");
                break;
            case eGameState.SetUp:
                GameGrid.Instance.GenerateGrid();
                SetState(eGameState.PreStart);
                Player.SetUp();
                break;
            case eGameState.Running:
                Score.Instance.SetScore(0);
                StartCoroutine(EnMan.DoInfiniteEnemySpawn());
                break;
            case eGameState.Ended:
                PlayerAim.HideReticule();
                StartCoroutine(WaitBeforeSetUp());
                break;
            default:
                break;
        }

    }

    IEnumerator<YieldInstruction> WaitBeforeSetUp()
    {
        yield return null;
        Player.Stop();
        EnMan.Reset();

        yield return new WaitForSeconds(1f);
        SetState(eGameState.SetUp);
    }

    public void GridReadyForPlayer()
    {
        if (Player == null)
        {
            Player = Instantiate(PlayerPrefab).GetComponent<PlayerMover>();
        }
        if (PlayerAim == null)
        {
            PlayerAim = Player.GetComponent<PlayerAimer>();
        }

        float halfWidth = (GameGrid.Instance.Width - 1) * 0.5f;
        float halfHeight = (GameGrid.Instance.Height - 1) * 0.5f;

        Player.transform.position = new Vector3(halfWidth, 0.6f, halfHeight);
    }

    public void EnemyRooted(EnemyMover enemy)
    {
        EnMan.RootedEnemies.Add(enemy);
        EnMan.ActiveEnemies.Remove(enemy);
    }

    public void EnemyDestroyed(EnemyMover enemy)
    {
        EnMan.RootedEnemies.Remove(enemy);
    }

}
