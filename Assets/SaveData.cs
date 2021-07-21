using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveData
{
    public double coins;
    public double totalCoins;
    public double coinsToPrestige;
    public double profitMultiplier;
    public double speedval;
    public double fameMultiplier;
    public double fame;
    public int[] numShops;
    public int[] shopLvls;
    public double[] shopRewards;
    public float[] shopRunTime;
    public int speedIndex;
    public string[] oldUpgrades;
    public bool[] shopAutomation;
    public string offlineTime;

    public SaveData (IdleGame idleGame)
    {
        coins = idleGame.coins;
        totalCoins = idleGame.totalCoins;
        coinsToPrestige = idleGame.coinsToPrestige;
        profitMultiplier = idleGame.profitMultiplier;
        speedval = idleGame.speedval;
        fameMultiplier = idleGame.fameMultiplier;
        fame = idleGame.fame;
        numShops = idleGame.numShops;
        speedIndex = idleGame.speedIndex;
        shopLvls = idleGame.shopLvls;
        shopRewards = idleGame.shopRewards;
        shopAutomation = idleGame.shopAutomation;
        shopRunTime = idleGame.shopRunTime;
        offlineTime = DateTime.Now.ToString();
        oldUpgrades = (string[])idleGame.oldUpgrades.ToArray(typeof( string ));
    }
}
