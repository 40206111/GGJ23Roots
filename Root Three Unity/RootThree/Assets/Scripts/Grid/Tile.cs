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
        List<Tile> connected = new List<Tile>();
        connected.Add(this);
        MatchedEnemies(ref connected);
        if (connected.Count >= 3)
        {
            Debug.Log($"wooh we found a {connected.Count} match!!");
            for (int i = 0; i < connected.Count; i++)
            {
                connected[i].RootedEnemy.DefeatEnemy();
                connected[i].RootedEnemy = null;
            }
        }
    }

    void MatchedEnemies(ref List<Tile> connections)
    {
        int width = GameGrid.Instance.Width;
        int height = GameGrid.Instance.Height;

        //left
        if (Index % width != 0)
        {
            int checkIndex = Index - 1;
            CheckIndex(checkIndex, ref connections);
        }
        //up
        if (Index + width < (width * height) - 1)
        {
            int checkIndex = Index + width;
            CheckIndex(checkIndex, ref connections);
        }
        //right
        if (Index % width != width - 1)
        {
            int checkIndex = Index + 1;
            CheckIndex(checkIndex, ref connections);
        }
        //down
        if (Index - width > 0)
        {
            int checkIndex = Index - width;
            CheckIndex(checkIndex, ref connections);
        }
    }

    void CheckIndex(int index, ref List<Tile> connections)
    {
        Tile neighbour = GameGrid.Instance.TheGrid[index];
        if (neighbour.Index != index)
        {
            Debug.LogError($"AAAAAAAAAAAAAAAAAAAAAAAAA sposed: {index} was: {neighbour.Index}");
        }
        if (!connections.Contains(neighbour) && neighbour.RootedEnemy != null)
        {
            connections.Add(neighbour);
            neighbour.MatchedEnemies(ref connections);
        }
    }

}
