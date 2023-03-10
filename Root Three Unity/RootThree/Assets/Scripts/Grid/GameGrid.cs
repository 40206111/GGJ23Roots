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
    public List<Material> Colours = new List<Material>();

    public List<Tile> TheGrid = new List<Tile>();

    CameraResize CameraControl;

    [SerializeField] Transform PlaneFor;
    [SerializeField] Transform PlaneBack;
    [SerializeField] Transform PlaneRight;
    [SerializeField] Transform PlaneLeft;

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
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 pos = new(x , 0, y);
                Tile newTile = Instantiate(Tile, pos, Quaternion.identity, transform).GetComponent<Tile>();
                int colourId = ChooseTileColour(x, y);
                newTile.SetColour(Colours[colourId], (eColours)(1 << colourId));
                newTile.Initialise(x, y);
                StartCoroutine(newTile.Waiting());
                TheGrid.Add(newTile);
            }
        }

        CameraControl?.ResizeCamera(Width, Height);
        GameManager.Instance.GridReadyForPlayer();

        float unitsPerScale = 10f;
        Vector3 scaleOnX = new Vector3(Width / unitsPerScale + 1f, 1f, 1f);
        Vector3 scaleOnZ = new Vector3(1f, 1f, Height / unitsPerScale + 1f);
        float yPos = unitsPerScale / 2f - 1;
        Vector3 forPos = new Vector3(Width / 2f - 0.5f, yPos, Height - 0.5f);
        Vector3 backPos = new Vector3(Width / 2f - 0.5f, yPos, -0.5f);
        Vector3 rightPos = new Vector3(Width - 0.5f, yPos, Height / 2f - 0.5f);
        Vector3 leftPos = new Vector3(-0.5f, yPos, Height / 2f - 0.5f);

        ConfigurePlane(PlaneFor, forPos, scaleOnX);
        ConfigurePlane(PlaneBack, backPos, scaleOnX);
        ConfigurePlane(PlaneRight, rightPos, scaleOnZ);
        ConfigurePlane(PlaneLeft, leftPos, scaleOnZ);
    }

    int ChooseTileColour(int x, int y)
    {
        int output = 0;

        bool isNeutral = Random.Range(0, 2) == 1;

        if (isNeutral ||
            y % Height == Height - 1 ||
            y == 0 ||
            x % Width == Width - 1 ||
            x == 0)
        {
            output = 0;
        }
        else
        {
            output = Random.Range(1, Colours.Count);
        }

        return output;
    }

    void ConfigurePlane(Transform plane, Vector3 pos, Vector3 scale)
    {
        plane.position = pos;
        plane.localScale = scale;
    }
}
