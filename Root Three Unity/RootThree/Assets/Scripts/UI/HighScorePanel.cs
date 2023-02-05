using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScorePanel : MonoBehaviour
{
    [SerializeField]
    List<TextMeshProUGUI> ScoreTexts;

    private void OnEnable()
    {
        List<int> scores = HighScores.Scores;
        for(int i = 0; i < scores.Count; ++i)
        {
            ScoreTexts[i].text = $"{scores[i]:n0}";
        }
    }
}
