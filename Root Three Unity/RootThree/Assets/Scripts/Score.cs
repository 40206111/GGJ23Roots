using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private static Score _instance;
    public static Score Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        _instance = this;
    }

    [SerializeField]
    TextMeshProUGUI ScoreText;
    int _currentScore = 0;
    public int CurrentScore { get { return _currentScore; } }

    private void Start()
    {
        RenderScore();
    }

    private void RenderScore()
    {
        ScoreText.text = $"{_currentScore:n0}";
    }

    public void ChangeScore(int value)
    {
        _currentScore += value;
        RenderScore();
    }

    public void SetScore(int value)
    {
        _currentScore = value;
        RenderScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ChangeScore(180);
        }
    }
}
