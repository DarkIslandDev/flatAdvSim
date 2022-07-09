using System;
using UnityEngine;


public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    public void OnMouseDown()
    {
        transform.position = BuildingSystem.GetMouseWorldPosition();
    }

    public void OnMouseDrag()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        transform.position = BuildingSystem.instance.SnapCoordinateToGrid(pos);
    }
}
