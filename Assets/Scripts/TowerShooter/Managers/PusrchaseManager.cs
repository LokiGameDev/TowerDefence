using UnityEngine;

public class PusrchaseManager : MonoBehaviour
{
    [SerializeField]
    private InventoryManager inventoryManager;

    public void PurchaseItem(int id)
    {
        inventoryManager.AddItem(id, 1);
    }
}
