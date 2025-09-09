using UnityEngine;

public class PusrchaseManager : MonoBehaviour
{
    [SerializeField]
    private InventoryManager inventoryManager;

    [SerializeField]
    private int[] costOfItem;

    public void PurchaseItem(int id)
    {
        if (GameManager.Instance.Purchasing(costOfItem[id]))
        {
            inventoryManager.AddItem(id, 1);
            costOfItem[id] *= 2;
        }
    }
}
