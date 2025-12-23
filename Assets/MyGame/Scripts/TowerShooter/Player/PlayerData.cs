using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int towerHealth;
    public int playerScore;
    public int waveNumber;
    public Dictionary<int, int> inventoryItems;

    public PlayerData(int towerHealth, int playerScore, int waveNumber, Dictionary<int, int> inventoryItems)
    {
        this.towerHealth = towerHealth;
        this.playerScore = playerScore;
        this.waveNumber = waveNumber;
        this.inventoryItems = inventoryItems;
    }

    public PlayerData()
    {
        towerHealth = 0;
        playerScore = 0;
        waveNumber = 0;
        inventoryItems = new Dictionary<int, int>();
    }
}


[System.Serializable]
public class ShopData
{
    public int[] costOfTurrets;
    public int[] costOfTowerUpgrades;
    public int[] costOfTurretUpgrades;

    public ShopData(int[] costOfTurrets,int[] costOfTowerUpgrades, int[] costOfTurretUpgrades)
    {
        this.costOfTurrets = costOfTurrets;
        this.costOfTowerUpgrades = costOfTowerUpgrades;
        this.costOfTurretUpgrades = costOfTurretUpgrades;
    }

    public ShopData()
    {
        int[] TurretValues = {20, 20, 20, 20};
        costOfTurrets = TurretValues;
        int[] TowerValues = {20,20,20,20};
        costOfTowerUpgrades = TowerValues;
        int[] TurretUpgradeValues = {20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20};
        costOfTurretUpgrades = TurretUpgradeValues;
    }
}