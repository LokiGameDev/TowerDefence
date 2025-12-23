using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float   scrollSpeed = 10f,
                    rotationSpeed = 5f,
                    minY = 4f,
                    maxY,
                    pivotMoveSpeed,
                    maxZoom = 35f;

    [SerializeField]
    private Transform pivot;

    #endregion

    void Start()
    {
        GameManager.Instance.onModeChange.AddListener(SetPivotToOrigin);
    }
    void Update()
    {
        if(GameManager.Instance.MenuPanelStatus()) return;
        if (((Input.GetAxis("Mouse ScrollWheel") != 0 && transform.position.y > minY) || Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y <= minY)
                && (Vector3.Distance(transform.position, pivot.position) < maxZoom ||
                    (Vector3.Distance(transform.position, pivot.position) > maxZoom && Input.GetAxis("Mouse ScrollWheel") > 0)))
        {
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            transform.position += transform.forward * scrollAmount;
        }

        if (Input.GetMouseButton(2) && pivot != null)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            transform.RotateAround(pivot.position, Vector3.up, mouseX * rotationSpeed);
            
            maxY = Vector3.Distance(transform.position, pivot.position) - (Vector3.Distance(transform.position, pivot.position) * 0.3f);

            if ((transform.position.y > minY && transform.position.y < maxY) || (transform.position.y > maxY && mouseY > 0) || (transform.position.y < minY && mouseY < 0))
            {
                Vector3 right = transform.right;
                transform.RotateAround(pivot.position, right, -mouseY * rotationSpeed);
            }
            
        }
        if((!GameManager.Instance.IsWaveGoing() || !GameManager.Instance.gamePaused) && !GameManager.Instance.isBuildMode)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direction = transform.right * horizontal + transform.forward * vertical;
            direction.y = 0;

            pivot.position += direction.normalized * pivotMoveSpeed * Time.deltaTime;
        }

    }

    public void SetPivotToOrigin()
    {
        pivot.position = new Vector3(0,0,0);
    }
}
