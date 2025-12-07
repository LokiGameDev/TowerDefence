using System;
using System.Collections.Generic;

[Serializable]
public class TurretData
{
    public int health;
    public int damage;
    public float fireRate;
    public float range;

    public TurretData(int health, int damage, float fireRate, float range)
    {
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
