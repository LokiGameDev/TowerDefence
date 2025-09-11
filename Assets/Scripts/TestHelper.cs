using UnityEngine;

public class TestHelper : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.AddScore(100);
        }
    }
}
