using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public string upgradeName;
    public string description;
    public int cost;

    public abstract void DoUpgrade();
}
