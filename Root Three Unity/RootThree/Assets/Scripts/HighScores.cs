using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HighScores
{
    private static List<int> _scores = new List<int>();
    public static List<int> Scores
    {
        get
        {
            if (_scores.Count < MaxScores)
            {
                LoadFromPrefs();
            }
            return _scores;
        }
    }

    static int MaxScores = 10;

    public static void AddNewScore(int score)
    {
        _scores.Add(score);
        _scores.Sort((x, y) => x <= y ? 1 : -1);
        while (_scores.Count > MaxScores)
        {
            _scores.RemoveAt(_scores.Count - 1);
        }
    }

    private static void LoadFromPrefs()
    {
        for (int i = _scores.Count; i < MaxScores; ++i)
        {
            _scores.Add(PlayerPrefs.GetInt(PrefsKey(i), 0));
        }
    }

    public static void SaveToPrefs()
    {
        for (int i = 0; i < _scores.Count; ++i)
        {
            PlayerPrefs.SetInt(PrefsKey(i), _scores[i]);
        }
    }

    public static void ClearScorePrefs()
    {
        PlayerPrefs.DeleteAll();
        //for(int i = 0; i < MaxScores; ++i)
        //{
        //    if (PlayerPrefs.HasKey(PrefsKey(i)))
        //    {
        //        PlayerPrefs.DeleteKey(PrefsKey(i));
        //    }
        //}
    }

    static string PrefsKey(int i)
    {
        return $"highscore_{i}";
    }
}
