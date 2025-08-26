using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool _isbuildingMoving = false;

    private void OnMouseOver()
    {
        _isbuildingMoving = true;
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }

    private void Update()
    {
        if (_isbuildingMoving)
        {
            Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
            transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
        }
    }

    // private void OnMouseDown()
    // {
    //     _isPlaced = true;
    // }
}
