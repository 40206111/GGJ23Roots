using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int Coords;
    public int Index;

    MeshRenderer Renderer;

    private void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
    }

    public void Initialise(int x, int y)
    {
        Coords = new Vector2Int(x, y);

        Index = x + y;
    }

    public void SetColour(Material mat)
    {
        Renderer.material = mat;
    }

}
