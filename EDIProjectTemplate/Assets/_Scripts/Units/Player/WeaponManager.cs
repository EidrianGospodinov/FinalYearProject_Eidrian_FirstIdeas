using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Units.Player
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> SwordPresetList;
        
        private EventBinding<OnSwitchHeroEvent> playerEventBinding;
        
        GameObject nextWeaponPreset;
        private void OnEnable()
        {
            playerEventBinding = EventBus<OnSwitchHeroEvent>.Register(HandleHeroSwitchEvent);

        }
        
        public void UpdateSwordIntensity(float intensity)
        {
            if (nextWeaponPreset != null)
            {
                var material=nextWeaponPreset.GetComponent<MeshRenderer>().material;
                material.SetColor("_EmissionColor", Color.white * Mathf.Pow(2, intensity));
            }
        }

        private void OnDisable()
        {
            EventBus<OnSwitchHeroEvent>.Unregister(playerEventBinding);
        }
        

        private void HandleHeroSwitchEvent(OnSwitchHeroEvent obj)
        {
            DisableAllSwordPresets();
            nextWeaponPreset = SwordPresetList.Find(x => x.name == obj.HeroData.weaponPrefab.name);
            nextWeaponPreset?.SetActive(true);
        }

        void DisableAllSwordPresets()
        {
            SwordPresetList.ForEach(x => x.gameObject.SetActive(false));
        }
        
    }
}