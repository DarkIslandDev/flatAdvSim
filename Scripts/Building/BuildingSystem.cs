using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    #region Instance

    public static BuildingSystem instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;


    public GameObject testHouse;

    private PlaceableObject objectToPlace;

    private void Start()
    {
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            InitializeWithObject(testHouse);
            // Debug.Log("B");
        }

        if (!objectToPlace)
        {
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            objectToPlace.Rotate();
            Debug.Log("R");
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            if (CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                TakeArea(start, objectToPlace.Size);
                Debug.Log("space");
            }
        }
        else
        {
            Destroy(objectToPlace.gameObject);
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, whiteTile,
            start.x, start.y,
            start.x + size.x, start.y + size.y);
    }
}