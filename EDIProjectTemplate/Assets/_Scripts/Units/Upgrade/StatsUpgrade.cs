using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Stats/StatsUpgrade")]
public class StatsUpgrade : Upgrade
{
    public List<Stats> UnitsToUpgrade = new List<Stats>();
    public List<StatInfo> UpgradeToApply = new List<StatInfo>();
    public bool isPercentUpgrade = false;
    
    public override void DoUpgrade()
    {
        foreach (var unitToUpgrade in UnitsToUpgrade)
        {
            unitToUpgrade.UnlockUpgrade(this);
        }
    }
}
