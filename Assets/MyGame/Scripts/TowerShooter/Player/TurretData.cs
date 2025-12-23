using System;
using System.Collections.Generic;

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
