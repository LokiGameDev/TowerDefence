using UnityEngine;

public class TestHelper : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.AddScore(100);
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            GameManager.Instance.RegenTowerHealth();
        }
        if(Input.GetKeyDown(KeyCode.U))
        {
            UnlockAllShopItems();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            GameManager.Instance.ClearSavedGridData();
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            GameManager.Instance.ReduceInventory();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            GameManager.Instance.ReduceScoreToZero();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            GameManager.Instance.IncreaseDayCount();
        }
    }

    void UnlockAllShopItems()
    {
        GameManager.Instance.shopManager.UnlockTurretPurchaseItem(0);
        GameManager.Instance.shopManager.UnlockTurretPurchaseItem(1);
        GameManager.Instance.shopManager.UnlockTurretPurchaseItem(2);
        GameManager.Instance.shopManager.UnlockTurretPurchaseItem(3);
    }
}
