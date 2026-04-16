using System;
using _Scripts.Units.Player.Combat;
using UnityEngine;

namespace _Scripts.Units.Player.Core
{
    [System.Serializable]
   [CreateAssetMenu(menuName = "Objective/Find Objective")]
    public class FindObjectObjective : Objective
    {
        private EventBinding<OnItemFound> itemFound;
        public SearchItemType lookingForItem;
        public override void Initialize()
        {
            base.Initialize();
            itemFound = EventBus<OnItemFound>.Register(OnItemFoundEvent);
        }

        /*public FindObjectObjective(string objectiveName, SearchItemType item)
        {
            lookingForItem = item;
            ObjectiveName = objectiveName;
        }*/
        private void OnItemFoundEvent(OnItemFound obj)
        {
            
            if (lookingForItem == obj.SearchItemTypeFound)
            {
                NotifyChanged();
                Complete();
            }
            else
            {
                Debug.Log($"Wrong item found, received item {obj.SearchItemTypeFound} \n" +
                          $"Looking for {lookingForItem}");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            EventBus<OnItemFound>.Unregister(itemFound);
        }
    }
}