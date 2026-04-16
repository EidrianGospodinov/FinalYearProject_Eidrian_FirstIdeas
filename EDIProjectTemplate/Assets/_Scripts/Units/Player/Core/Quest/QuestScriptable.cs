using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Units.Player.Core
{
    [CreateAssetMenu(menuName = "Objective/quest ")]
    public class QuestScriptable : ScriptableObject
    {
        public List<Objective> objectives;

        public string GetUniqueId()
        {
            string uniqueId ="";
            foreach (var obj in objectives)
            {
                uniqueId += obj.GetInstanceID();
            }

            if (uniqueId == "")
            {
                Debug.LogError("No unique id found");
            }
            return uniqueId;
        }
    }
}