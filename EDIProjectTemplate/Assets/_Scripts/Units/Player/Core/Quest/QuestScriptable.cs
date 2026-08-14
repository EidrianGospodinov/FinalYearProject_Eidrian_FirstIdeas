using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Units.Player.Core
{
    [CreateAssetMenu(menuName = "Objective/quest ")]
    public class QuestScriptable : ScriptableObject
    {
        [SerializeReference] public List<Objective> objectives;

        public string GetUniqueId()
        {
            string uniqueId ="";
            foreach (var obj in objectives)
            {
                uniqueId += obj.GetEntityId();
            }

            if (uniqueId == "")
            {
                Debug.LogError("No unique id found");
            }
            return uniqueId;
        }
    }
}