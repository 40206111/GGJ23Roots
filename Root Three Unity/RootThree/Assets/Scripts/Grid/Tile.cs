using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int Coords;
    public int Index;

    MeshRenderer Renderer;

    public EnemyMover RootedEnemy;

    eColours Colour;

    private void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
    }

    public void Initialise(int x, int y)
    {
        Coords = new Vector2Int(x, y);

        Index = y * GameGrid.Instance.Width + x;
    }

    public void SetColour(Material mat, eColours colour)
    {
        Renderer.material = mat;
        Colour = colour;
    }

    public void Root(EnemyMover enemy)
    {
        RootedEnemy = enemy;
        CheckForAMatch();
    }

    void CheckForAMatch()
    {
        List<Tile> connected = new List<Tile>();
        connected.Add(this);
        bool scoreBonus = MatchedEnemies(ref connected, RootedEnemy.Colour);
        if (connected.Count >= 3)
        {
            Debug.Log($"wooh we found a {connected.Count} match!!");
            int score = 10 * Mathf.RoundToInt(connected.Count * Mathf.Pow(2, connected.Count - 3));
            score = scoreBonus ? score * 2 : score;
            Score.Instance.ChangeScore(score);
            StartCoroutine(WaitToDestroy(connected));
        }
    }

    IEnumerator WaitToDestroy(List<Tile> connected)
    {
        yield return null;

        for (int i = 0; i < connected.Count; i++)
        {
            connected[i].RootedEnemy.DefeatEnemy();
            connected[i].RootedEnemy = null;
        }
    }

    bool MatchedEnemies(ref List<Tile> connections, eColours colour)
    {
        int width = GameGrid.Instance.Width;
        int height = GameGrid.Instance.Height;
        bool output = false;

        if (colour == Colour)
        {
            output |= true;
        }

        //left
        if (Index % width != 0)
        {
            int checkIndex = Index - 1;
            output |= CheckIndex(checkIndex, ref connections, colour);
        }
        //up
        if (Index + width < (width * height) - 1)
        {
            int checkIndex = Index + width;
            output |= CheckIndex(checkIndex, ref connections, colour);
        }
        //right
        if (Index % width != width - 1)
        {
            int checkIndex = Index + 1;
            output |= CheckIndex(checkIndex, ref connections, colour);
        }
        //down
        if (Index - width > 0)
        {
            int checkIndex = Index - width;
            output |= CheckIndex(checkIndex, ref connections, colour);
        }

        return output;
    }

    bool CheckIndex(int index, ref List<Tile> connections, eColours colour)
    {
        Tile neighbour = GameGrid.Instance.TheGrid[index];
        if (neighbour.Index != index)
        {
            Debug.LogError($"AAAAAAAAAAAAAAAAAAAAAAAAA sposed: {index} was: {neighbour.Index}");
        }
        if (!connections.Contains(neighbour) && neighbour.RootedEnemy != null)
        {
            if (neighbour.RootedEnemy.Colour.HasFlag(colour))
            {
                connections.Add(neighbour);
                return neighbour.MatchedEnemies(ref connections, colour);
            }
        }

        return false;
    }

}
