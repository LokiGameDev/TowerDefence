using System;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private InventoryManager inventoryManager;
    public PlayerTower playerTower;
    public GameObject shopPanel;
    public GameObject[] shopMenuPanels;
    private int[] costOfItem;
    private int[] costOfTowerUpgrade;
    [SerializeField]
    private int[] costOfTurretUpgradeIndividual;

    private int[][] costOfTurretUpgrade = new int[4][];
    public TMP_Text[] costOfTurretUpgradeTexts;
    public TMP_Text[] costOfTowerUpgradeTexts;
    public TMP_Text[] costOfTurretsTexts;

    private bool[] purchaseItemsUnlocked = new bool[4];
    public GameObject[] purchaseItemsLockPanels;
    public GameObject[] purchaseItemsLockPanels2;

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
        InitialiseShopCostValue();
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
        AudioManager.Instance.PlayTheAudioClip(AudioType.MouseClick);
        if (GameManager.Instance.Purchasing(costOfItem[id]))
        {
            inventoryManager.AddItem(id, 1);
            costOfItem[id] *= 4;
            SaveTheCurrentShopData();
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
        purchaseItemsLockPanels2[id].SetActive(false);
        purchaseItemsLockPanels[id].SetActive(false);
    }

    public void OpenUnlockedItemsPanel()
    {
        for (int i = 0; i < purchaseItemsUnlocked.Length; i++)
        {
            if (purchaseItemsUnlocked[i] == true)
            {
                purchaseItemsLockPanels[i].SetActive(false);
                purchaseItemsLockPanels2[i].SetActive(false);
            }
        }
    }

    #endregion

    #region Tower upgrade related methods

    public void UpgradeTower(int id)
    {
        AudioManager.Instance.PlayTheAudioClip(AudioType.MouseClick);
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
            SaveTheCurrentShopData();
            turretUpgradeEvent?.Invoke();
        }
    }

    #endregion

    #region Turret upgrade related methods

    public void UpgradeTurret(int number)
    {
        AudioManager.Instance.PlayTheAudioClip(AudioType.MouseClick);
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
            SaveTheCurrentShopData();
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

    private void InitialiseShopCostValue()
    {
        ShopData shopData = PlayerDataSaver.LoadShopData();
        costOfTurretUpgrade = TurretUpgradeValuesConvertor(shopData.costOfTurretUpgrades);
        costOfItem = shopData.costOfTurrets;
        costOfTowerUpgrade = shopData.costOfTowerUpgrades;
        LoadAllTheTurretUpgradeCostText(TurretUpgradeValuesConvertor(shopData.costOfTurretUpgrades));
        LoadTheTurretCostText(costOfItem);
        LoadTheTowerUpgradeCostText(costOfTowerUpgrade);
    }

    private void SaveTheCurrentShopData()
    {
        ShopData shopData = new ShopData(costOfItem, costOfTowerUpgrade, TurretValueToStoreConvertor(costOfTurretUpgrade));
        PlayerDataSaver.SaveShopData(shopData);
        LoadAllTheTurretUpgradeCostText(costOfTurretUpgrade);
        LoadTheTurretCostText(costOfItem);
        LoadTheTowerUpgradeCostText(costOfTowerUpgrade);
    }

    private void LoadTheTurretCostText(int[] cost)
    {
        for(int i=0;i<cost.Length;i++)
        {
            if(costOfTurretsTexts[i]!=null) costOfTurretsTexts[i].text = $"{cost[i]}";
        }
    }

    private void LoadTheTowerUpgradeCostText(int[] cost)
    {
        for(int i=0;i<cost.Length;i++)
        {
            if(costOfTowerUpgradeTexts[i]!=null) costOfTowerUpgradeTexts[i].text = $"{cost[i]}";
        }
    }

    private void LoadAllTheTurretUpgradeCostText(int[][] cost)
    {
        int textIndex = 0;
        for(int i =0;i< cost.Length;i++)
        {
            for(int j=0;j<cost[i].Length;j++)
            {
                if(costOfTurretUpgradeTexts[textIndex]==null) continue;
                costOfTurretUpgradeTexts[textIndex].text = $"{cost[i][j]}";
                textIndex++;
            }
        }
    }

    private int[][] TurretUpgradeValuesConvertor(int[] values)
    {
        int[][] result = new int[4][];
        for (int i = 0; i < 4; i++)
        {
            result[i] = new int[4];
            for (int j = 0; j < 4; j++)
            {
                result[i][j] = values[i * 4 + j];
            }
        }
        return result;
    }

    private int[] TurretValueToStoreConvertor(int[][] matrix)
    {
        int[] flat = new int[16];
        int index = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                flat[index++] = matrix[i][j];
            }
        }
        return flat;
    }

    private TurretSaveData InitialValueForTurrets()
    {
        TurretSaveData data = new TurretSaveData();
        data.turrets[0] = new TurretData("Shooter", 60, 15, 1.0f, 5.0f);
        data.turrets[1] = new TurretData("Slowdowner", 90, 30, 0.8f, 6.0f);
        data.turrets[2] = new TurretData("Stunner", 150, 45, 0.6f, 7.0f);
        data.turrets[3] = new TurretData("Destroyer", 210, 90, 0.5f, 8.0f);
        return data;
    }

    public TurretSaveData GetCurrentTurretData()
    {
        return currentTurretsData;
    }
}
