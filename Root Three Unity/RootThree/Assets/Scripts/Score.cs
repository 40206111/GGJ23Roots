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
    int CurrentScore = 0;

    private void Start()
    {
        RenderScore();
    }

    private void RenderScore()
    {
        ScoreText.text = $"{CurrentScore:n0}";
    }

    public void ChangeScore(int value)
    {
        CurrentScore += value;
        RenderScore();
    }

    public void SetScore(int value)
    {
        CurrentScore = value;
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
