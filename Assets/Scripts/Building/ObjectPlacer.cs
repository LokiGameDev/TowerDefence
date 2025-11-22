using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject playerTurret = Instantiate(prefab);
        playerTurret.transform.position = position;
        playerTurret.GetComponent<PlayerTurret>()?.TurretUpgradeStatus(true);
        placedGameObjects.Add(playerTurret);
        return placedGameObjects.Count - 1;
    }

    internal void RemoveObject(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null) return;
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}
