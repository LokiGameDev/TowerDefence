using System;
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
    [SerializeField]
    private int[] costOfTurretUpgradeIndividual;

    private int[][] costOfTurretUpgrade = new int[4][];

    private bool[] purchaseItemsUnlocked = new bool[4];
    public GameObject[] purchaseItemsLockPanels;

    private TurretSaveData currentTurretsData;

    public static event Action turretUpgradeEvent;


    void Start()
    {
        for (int i = 0; i < shopMenuPanels.Length; i++)
        {
            shopMenuPanels[i].SetActive(false);
        }
        shopPanel.SetActive(false);
        currentTurretsData = TurretDataSaver.LoadTurretData();
        if(currentTurretsData ==  null)
        {
            currentTurretsData = InitialValueForTurrets();
            TurretDataSaver.SaveTurretData(currentTurretsData);
        }
        InitialiseTurretUpgradeCostValue();
        OpenUnlockedItemsPanel();
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

    public bool ShopStatus()
    {
        return shopPanel.activeSelf;
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
        if (purchaseItemsUnlocked[id] == true)
        {
            OpenUnlockedItemsPanel();
            return;
        }
        purchaseItemsUnlocked[id] = true;
        purchaseItemsLockPanels[id].SetActive(false);
    }

    public void OpenUnlockedItemsPanel()
    {
        for (int i = 0; i < purchaseItemsUnlocked.Length; i++)
        {
            if (purchaseItemsUnlocked[i] == true)
            {
                purchaseItemsLockPanels[i].SetActive(false);
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
                    playerTower.TowerProductivityUpgrade();
                    break;
            }
            costOfTowerUpgrade[id] *= 2;
            turretUpgradeEvent?.Invoke();
        }
    }

    #endregion

    #region Turret upgrade related methods

    public void UpgradeTurret(int number)
    {
        if(number<4)
        {
            UpgradeTurret(0,number);
        }
        else if(number<8)
        {
            number-=4;
            UpgradeTurret(1,number);
        }
        else if(number<12)
        {
            number-=8;
            UpgradeTurret(2,number);
        }
        else if(number<16)
        {
            number-=12;
            UpgradeTurret(3,number);
        }
    }

    public void UpgradeTurret(int id, int statID)
    {
        if (GameManager.Instance.Purchasing(costOfTurretUpgrade[id][statID]))
        {
            TurretData turret = currentTurretsData.turrets[id];
            switch (statID)
            {
                case 0:
                    turret.health += 50;
                    break;
                case 1:
                    turret.fireRate *= 0.9f;
                    break;
                case 2:
                    turret.damage += 10;
                    break;
                case 3:
                    turret.range += 1.0f;
                    break;
            }
            costOfTurretUpgrade[id][statID] *= 2;
            TurretDataSaver.SaveTurretData(currentTurretsData);
            Debug.Log($"Turret upgraded {id} , {statID}");
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

    private void InitialiseTurretUpgradeCostValue()
    {
        costOfTurretUpgrade[0] = costOfTurretUpgradeIndividual;
        costOfTurretUpgrade[1] = costOfTurretUpgradeIndividual;
        costOfTurretUpgrade[2] = costOfTurretUpgradeIndividual;
        costOfTurretUpgrade[3] = costOfTurretUpgradeIndividual;
    }

    private TurretSaveData InitialValueForTurrets()
    {
        TurretSaveData data = new TurretSaveData();
        data.turrets[0] = new TurretData(100, 20, 1.0f, 5.0f);
        data.turrets[1] = new TurretData(150, 30, 0.8f, 6.0f);
        data.turrets[2] = new TurretData(200, 40, 0.6f, 7.0f);
        data.turrets[3] = new TurretData(250, 50, 0.5f, 8.0f);
        return data;
    }

    public TurretSaveData GetCurrentTurretData()
    {
        return currentTurretsData;
    }
}
