using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int Coords;
    public int Index;

    MeshRenderer Renderer;

    public EnemyMover RootedEnemy;

    private void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
    }

    public void Initialise(int x, int y)
    {
        Coords = new Vector2Int(x, y);

        Index = y * GameGrid.Instance.Width + x;
    }

    public void SetColour(Material mat)
    {
        Renderer.material = mat;
    }

    public void Root(EnemyMover enemy)
    {
        RootedEnemy = enemy;
        CheckForAMatch();
    }

    void CheckForAMatch()
    {
        int count = MatchedEnemies(0, -1);
        if (count >= 3)
        {
            Debug.Log($"wooh we found a {count} match!!");
        }
    }

    int MatchedEnemies(int current, int prev)
    {
        current++;
        int width = GameGrid.Instance.Width;
        int height = GameGrid.Instance.Height;

        if (Index > 0)
        {
            int checkIndex = Index - 1;
            current = CheckIndex(checkIndex, prev, current);
        }
        if (Index + width < (width * height) - 1)
        {
            int checkIndex = Index + width;
            current = CheckIndex(checkIndex, prev, current);
        }
        if (Index < (width * height) - 1)
        {
            int checkIndex = Index + 1;
            current = CheckIndex(checkIndex, prev, current);
        }
        if (Index - width > 0)
        {
            int checkIndex = Index - width;
            current = CheckIndex(checkIndex, prev, current);
        }

        return current;
    }

    int CheckIndex(int index, int prev, int current)
    {
        Tile neighbour = GameGrid.Instance.TheGrid[index];
        if (neighbour.Index != index)
        {
            Debug.LogError($"AAAAAAAAAAAAAAAAAAAAAAAAA sposed: {index} was: {neighbour.Index}");
        }
        if (prev != index && neighbour.RootedEnemy != null)
        {
            return neighbour.MatchedEnemies(current, Index);
        }

        return current;
    }

}
