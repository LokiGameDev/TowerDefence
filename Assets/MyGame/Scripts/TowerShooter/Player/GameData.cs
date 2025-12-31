using System.Collections.Generic;
using System;

[Serializable]
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

[Serializable]
public class TowerData
{
    public int health;
    public int maxHealth;
    public float fireRate;
    public float damage;
    public float range;

    public TowerData(int health, int maxHealth, float fireRate, float damage, float range)
    {
        this.health = health;
        this.maxHealth = maxHealth;
        this.fireRate = fireRate;
        this.damage = damage;
        this.range = range;
    }

    public TowerData()
    {
        health = 500;
        maxHealth = 500;
        fireRate = 5f;
        damage = 1f;
        range = 10f;
    }
}

[Serializable]
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
        int[] TurretValues = {20, 40, 80, 160};
        costOfTurrets = TurretValues;
        int[] TowerValues = {30,30,30,30};
        costOfTowerUpgrades = TowerValues;
        int[] TurretUpgradeValues = {20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20};
        costOfTurretUpgrades = TurretUpgradeValues;
    }
}

[Serializable]
public class TurretData
{
    public string name;
    public float health;
    public int damage;
    public float fireRate;
    public float range;

    public TurretData(string name, float health, int damage, float fireRate, float range)
    {
        this.name = name;
        this.health = health;
        this.damage = damage;
        this.fireRate = fireRate;
        this.range = range;
    }
}

public class TurretSaveData
{
     public TurretData[] turrets = new TurretData[4];
}