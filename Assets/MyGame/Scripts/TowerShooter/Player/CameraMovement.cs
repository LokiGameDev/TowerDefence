using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float   zoomSpeed = 10f,
                    rotationSpeed = 5f,
                    minZoomOut = 10f,
                    maxZoomOut = 25f;

    [SerializeField]
    private Transform pivot;

    private float maxLowPoint;

    #endregion

    void Update()
    {
        if(GameManager.Instance.MenuPanelStatus()) return;

        maxLowPoint = Vector3.Distance(transform.position,pivot.transform.position) * 0.75f;

        if (transform.GetComponent<Camera>().orthographicSize >= maxZoomOut && Input.GetAxis("Mouse ScrollWheel") > 0 ||
                    (transform.GetComponent<Camera>().orthographicSize <= minZoomOut && Input.GetAxis("Mouse ScrollWheel") < 0) ||
                        (transform.GetComponent<Camera>().orthographicSize > minZoomOut && transform.GetComponent<Camera>().orthographicSize < maxZoomOut))
        {
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            transform.GetComponent<Camera>().orthographicSize -= scrollAmount;
        }

        if (Input.GetMouseButton(2) && pivot != null)
        {
            CameraPivotRotation();
        }

    }

    private void CameraPivotRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.RotateAround(pivot.position, Vector3.up, mouseX * rotationSpeed);
        
        float maxY = Vector3.Distance(transform.position, pivot.position) - (Vector3.Distance(transform.position, pivot.position) * 0.1f);

        if ((transform.position.y > maxLowPoint && transform.position.y < maxY) || (transform.position.y > maxY && mouseY > 0) || (transform.position.y < maxLowPoint && mouseY < 0))
        {
            Vector3 right = transform.right;
            transform.RotateAround(pivot.position, right, -mouseY * rotationSpeed);
        }
    }

}
