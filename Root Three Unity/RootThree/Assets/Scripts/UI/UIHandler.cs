using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI PressToStartTxt;

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
            default:
                break;
        }

    }

}
