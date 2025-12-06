using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private InventoryManager inventoryManager;
    public PlayerTower playerTower;
    public GameObject shopPanel;
    public GameObject[] shopMenuPanels;

    [SerializeField]
    private int[] costOfItem;
    [SerializeField]
    private int[] costOfTowerUpgrade;

    private bool[] purchaseItemsUnlocked;
    public GameObject[] purchaseItemsLockPanels;


    void Start()
    {
        for (int i = 0; i < shopMenuPanels.Length; i++)
        {
            shopMenuPanels[i].SetActive(false);
        }
        shopPanel.SetActive(false);
    }

    public void ShopPanelActivate()
    {
        bool status = !shopPanel.activeSelf;
        shopPanel.SetActive(status);
        if(status)
        {
            for (int i = 1; i < shopMenuPanels.Length; i++)
            {
                shopMenuPanels[i].SetActive(false);
            }
            shopMenuPanels[0].SetActive(true);
        }
    }

    #region Turret purchase related methods

    public void PurchaseItem(int id)
    {
        if (GameManager.Instance.Purchasing(costOfItem[id]))
        {
            inventoryManager.AddItem(id, 1);
            costOfItem[id] *= 2;
        }
    }

    public void UnlockTurretPurchaseItem(int id)
    {
        if (purchaseItemsUnlocked[id] == true) return;
        purchaseItemsUnlocked[id] = true;
        purchaseItemsLockPanels[id].SetActive(false);
    }

    public void OpenUnlockedItemsPanel()
    {
        for (int i = 0; i < purchaseItemsUnlocked.Length; i++)
        {
            if (purchaseItemsUnlocked[i] == false)
            {
                purchaseItemsLockPanels[i].SetActive(true);
            }
        }
    }

    #endregion

    #region Tower upgrade related methods

    public void UpgradeTower(int id)
    {
        if (GameManager.Instance.Purchasing(costOfTowerUpgrade[id]))
        {
            playerTower.GetComponent<PlayerTower>();
            switch (id)
            {
                case 0:
                    playerTower.TowerHealthUpgrade();
                    break;
                case 1:
                    playerTower.TowerFireRateUpgrade();
                    break;
                case 2:
                    playerTower.TowerDamageUpgrade();
                    break;
                case 3:
                    playerTower.TowerRangeUpgrade();
                    break;
            }
            costOfTowerUpgrade[id] *= 2;
        }
    }

    #endregion

    public void LoadMenuPanel(int panelIndex)
    {
        for (int i = 0; i < shopMenuPanels.Length; i++)
        {
            shopMenuPanels[i].SetActive(i == panelIndex);
        }
    }
}
