using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Units.Player
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> SwordPresetList;
        private EventBinding<OnSwitchHeroEvent> playerEventBinding;

        private void OnEnable()
        {
            playerEventBinding = EventBus<OnSwitchHeroEvent>.Register(HandleHeroSwitchEvent);
        }

        private void HandleHeroSwitchEvent(OnSwitchHeroEvent obj)
        {
            DisableAllSwordPresets();
            var nextWeaponPreset = SwordPresetList.Find(x => x.name == obj.HeroData.weaponPrefab.name);
            nextWeaponPreset?.SetActive(true);
        }

        void DisableAllSwordPresets()
        {
            SwordPresetList.ForEach(x => x.gameObject.SetActive(false));
        }
    }
}