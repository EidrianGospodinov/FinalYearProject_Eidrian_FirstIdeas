using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] List<Stats> statsList;

    [ContextMenu("DELETE Saved Stats Data")]
    private void ClearAllStats()
    {
        foreach (var stat in statsList)
        {
            stat.ResetAppliedUpgrades();
        }
        Debug.Log("All Stats DataCleared");
    }

    private void OnApplicationQuit()
    {
        ClearAllStats();
    }
}
