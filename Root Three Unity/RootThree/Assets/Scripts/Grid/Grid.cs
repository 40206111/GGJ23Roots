using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Range(1, 50)]
    [SerializeField] int Width;
    [Range(1, 50)]
    [SerializeField] int Height;
    [SerializeField] GameObject Tile;
    [SerializeField] List<Material> Colours = new List<Material>();

    List<Tile> TheGrid = new List<Tile>();

    private void Start()
    {
        GenerateGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"Generating new grid {Width}x{Height}");
            GenerateGrid();
        }
    }

    void ClearGrid()
    {
        for (int i = 0; i < TheGrid.Count; i++)
        {
            Destroy(TheGrid[i].gameObject);
        }
        TheGrid.Clear();
    }


    void GenerateGrid()
    {
        ClearGrid();
        float halfWidth = 0;//  Width * 0.5f;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector3 pos = new(x - halfWidth, 0, y);
                int matInt = Random.Range(0, Colours.Count);
                Tile newTile = Instantiate(Tile, pos, Quaternion.identity, transform).GetComponent<Tile>();
                newTile.SetColour(Colours[matInt]);
                newTile.Initialise(x, y);
                TheGrid.Add(newTile);
            }
        }
    }
}
