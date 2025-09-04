using System;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float hoverHeight = 0.06f;

    [SerializeField]
    private GameObject cellIndicatorPrefab;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicatorPrefab.SetActive(false);
        cellIndicatorRenderer = cellIndicatorPrefab.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicatorPrefab.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicatorPrefab.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject preview)
    {
        Renderer[] renderers = preview.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
        cellIndicatorPrefab.SetActive(false);
    }

    public void UpdatePreviewPosition(Vector3 position, bool canPlace)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(canPlace);
        }
        MoveCursor(position);
        ApplyFeedbackToCursor(canPlace);
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicatorPrefab.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        if (previewObject != null)
        {
            previewObject.transform.position = new Vector3(position.x, hoverHeight, position.z);
        }
    }

    private void ApplyFeedbackToPreview(bool canPlace)
    {
        Color color = canPlace ? Color.white : Color.red;
        color.a = 0.5f;
        previewMaterialInstance.color = color;
    }

    private void ApplyFeedbackToCursor(bool canPlace)
    {
        Color color = canPlace ? Color.white : Color.red;
        color.a = 0.5f;
        cellIndicatorRenderer.material.color = color;   
    }

    public void StartShowingRemovePreview()
    {
        cellIndicatorPrefab.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}
