using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI PressToStartTxt;
    [SerializeField]
    GameObject PauseMenu;
    [SerializeField]
    TextMeshProUGUI ResumeText;
    [SerializeField]
    GameObject ResumeBtn;

    [SerializeField]
    GameObject QuitButton;

    bool Paused = false;

    private void Awake()
    {
        PressToStartTxt.enabled = false;
#if PLATFORM_STANDALONE_WIN && !UNITY_EDITOR_WIN
        QuitButton.SetActive(true);
#else
        QuitButton.SetActive(false);
#endif
    }

    private void Start()
    {
        GameManager.Instance.SetState(GameManager.eGameState.Paused);
        ResumeText.text = "Start";
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(ResumeBtn);
        }
    }

    public void UpdateFromGameState(GameManager.eGameState nowState)
    {
        switch (nowState)
        {
            case GameManager.eGameState.Running:
                PressToStartTxt.enabled = false;
                break;
            case GameManager.eGameState.SetUp:
                PressToStartTxt.enabled = true;
                break;
            case GameManager.eGameState.Paused:
                PauseMenu.SetActive(true);
                Paused = true;
                StartCoroutine(WaitForUnPause());
                break;
            default:
                break;
        }

    }

    IEnumerator<YieldInstruction> WaitForUnPause()
    {
        while (Paused)
        {
            yield return null;
            if (Input.GetButtonDown("Pause"))
            {
                UIResumeGame();
            }
        }

    }

    public void UIResumeGame()
    {
        GameManager.Instance.UnPause();
        ResumeText.text = "Resume";
        PauseMenu.SetActive(false);
        Paused = false;
    }

    public void UIQuitGame()
    {
        HighScores.SaveToPrefs();
        Application.Quit();
    }

}
