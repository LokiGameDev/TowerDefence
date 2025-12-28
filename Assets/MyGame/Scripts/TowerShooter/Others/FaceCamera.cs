using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.transform.forward);
        transform.rotation *= Quaternion.Euler(0,180,0);
    }
}
