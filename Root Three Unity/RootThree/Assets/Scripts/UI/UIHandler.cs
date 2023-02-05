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

    private void Awake()
    {
        PressToStartTxt.enabled = false;
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
                break;
            default:
                break;
        }

    }

    public void UIResumeGame()
    {
        GameManager.Instance.UnPause();
        ResumeText.text = "Resume";
        PauseMenu.SetActive(false);
    }

}
