using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Unit Stats")]
public class Stats : ScriptableObject
{
    public List<StatInfo> instanceStatsInfo = new List<StatInfo>();
    public List<StatInfo> StatsInfo = new List<StatInfo>();
    private List<StatsUpgrade> appliedUpgrades = new List<StatsUpgrade>();

    public event Action<Stats, StatsUpgrade> upgradeApplied;
    public float GetStat(Stat stat)
    {
        foreach (var statInfo in StatsInfo)
        {
            if (statInfo.statType == stat)
            {
                return GetUpgradedValue(statInfo.statType, statInfo.statValue);
            }
        }
        foreach (var instanceStatInfo in instanceStatsInfo)
        {
            if (instanceStatInfo.statType == stat)
            {
                return GetUpgradedValue(instanceStatInfo.statType, instanceStatInfo.statValue);
            }
        }

        Debug.LogError($"No stat value found for {stat} on {this.name}");
        return 0;
    }

    public void UnlockUpgrade(StatsUpgrade upgrade)
    {
        if (!appliedUpgrades.Contains(upgrade))
        {
            appliedUpgrades.Add(upgrade);
            upgradeApplied?.Invoke(this, upgrade);
        }
    }

    private float GetUpgradedValue(Stat stat, float baseValue)
    {
        foreach (var upgrade in appliedUpgrades)
        {
            foreach (var upgradeToApply in upgrade.UpgradeToApply)
            {
                if (upgradeToApply.statType != stat)
                {
                    continue;
                }

                if (upgrade.isPercentUpgrade)
                {
                    baseValue *= (upgradeToApply.statValue / 100f) + 1f;
                }
                else
                {
                    baseValue += upgradeToApply.statValue;
                }
            }
        }

        return baseValue;
    }

    public void ResetAppliedUpgrades()
    {
        appliedUpgrades.Clear();
    }
    
}

public enum Stat
{
    Health, 
    Speed,
    SwitchHeroCooldown,
    Dash,
    ThunderUlt,
    TsunamiUlt,
    LongRangeBeam
}
