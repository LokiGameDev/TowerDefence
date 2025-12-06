using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int towerHealth;
    public int playerScore;
    public int waveNumber;
    public Dictionary<int, int> inventoryItems;
    public List<bool> shopUnlockChecker;

    public PlayerData(int towerHealth, int playerScore, int waveNumber, Dictionary<int, int> inventoryItems, List<bool> shopUnlockChecker)
    {
        this.towerHealth = towerHealth;
        this.playerScore = playerScore;
        this.waveNumber = waveNumber;
        this.inventoryItems = inventoryItems;
        this.shopUnlockChecker = shopUnlockChecker;
    }

    public PlayerData()
    {
        towerHealth = 0;
        playerScore = 0;
        waveNumber = 0;
        inventoryItems = new Dictionary<int, int>();
        shopUnlockChecker = new List<bool>();
    }
}
