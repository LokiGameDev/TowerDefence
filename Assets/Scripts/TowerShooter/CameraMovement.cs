using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 10f;
    private float rotationSpeed = 5f;

    [SerializeField]
    private float minY = 4f,maxY;

    [SerializeField]
    private Transform pivot;

    void Update()
    {
        if ((Input.GetAxis("Mouse ScrollWheel") != 0 && transform.position.y > minY) || (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y <= minY))
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
            Debug.Log(maxY);

            if ((transform.position.y > minY && transform.position.y < maxY) || (transform.position.y > maxY && mouseY > 0) || (transform.position.y < minY && mouseY < 0))
            {
                Vector3 right = transform.right;
                transform.RotateAround(pivot.position, right, -mouseY * rotationSpeed);
            }
            
        }

    }
}
