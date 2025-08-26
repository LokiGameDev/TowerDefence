using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject building1;
    public GameObject building2;

    private PlacableObject objectToPlace;

    #region UnityMethods

    public void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            InitializeWithObject(building1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            InitializeWithObject(building2);
        }

        if (!objectToPlace)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStratPosition());
                TakeArea(start, objectToPlace.Size);
            }
            else
            {
                Destroy(objectToPlace.gameObject);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(objectToPlace.gameObject);
        }
    }

    #endregion

    #region Utils

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
        Vector3Int cellPos = grid.WorldToCell(position);
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

    #endregion

    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 pos = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlacableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    private bool CanBePlaced(PlacableObject placableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStratPosition());
        area.size = placableObject.Size;

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
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + start.x, start.y + start.y);
    }

    #endregion
}
