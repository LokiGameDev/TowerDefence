using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewAround : MonoBehaviour
{
    public float   rotationSpeed = 5f;
    public GameObject uiBlocker;
    void Start()
    {
        uiBlocker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            uiBlocker.SetActive(true);
            float mouseX = Input.GetAxis("Mouse X");

            Vector3 rot = transform.rotation.eulerAngles;

            rot.y -= mouseX * rotationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Euler(rot);
        }
        else
        {
            uiBlocker.SetActive(false);
        }
    }
}
