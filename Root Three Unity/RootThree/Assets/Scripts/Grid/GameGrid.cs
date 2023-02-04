using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance { get; private set; }

    [Range(1, 50)]
    public int Width;
    [Range(1, 50)]
    public int Height;
    [SerializeField] GameObject Tile;
    [SerializeField] List<Material> Colours = new List<Material>();

    public List<Tile> TheGrid = new List<Tile>();

    CameraResize CameraControl;

    private void Start()
    {
        CameraControl = Camera.main.GetComponent<CameraResize>();
        if (Instance != null)
        {
            Debug.LogError($"Too many {this.GetType()} instances");
        }
        Instance = this;
    }

    void ClearGrid()
    {
        for (int i = 0; i < TheGrid.Count; i++)
        {
            Destroy(TheGrid[i].gameObject);
        }
        TheGrid.Clear();
    }


    public void GenerateGrid()
    {
        ClearGrid();
        for (int y = 0; y < Width; y++)
        {
            for (int x = 0; x < Height; x++)
            {
                Vector3 pos = new(x , 0, y);
                int matInt = Random.Range(0, Colours.Count);
                Tile newTile = Instantiate(Tile, pos, Quaternion.identity, transform).GetComponent<Tile>();
                newTile.SetColour(Colours[matInt]);
                newTile.Initialise(x, y);
                TheGrid.Add(newTile);
            }
        }

        CameraControl?.ResizeCamera(Width, Height);
        GameManager.Instance.GridReadyForPlayer();
    }

}
