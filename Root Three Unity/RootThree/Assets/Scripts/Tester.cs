using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(ListString(HighScores.Scores));
            HighScores.AddNewScore(Random.Range(10, 200));
            Debug.Log(ListString(HighScores.Scores));
        }
    }

    string ListString(List<int> l)
    {
        string outString = "";
        foreach(int i in l)
        {
            outString += $"{i},";
        }
        return outString.Substring(0, outString.Length - 1);
    }
}
