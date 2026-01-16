using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Units.Player
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> SwordPresetList;
        private EventBinding<OnSwitchHeroEvent> playerEventBinding;
        GameObject nextWeaponPreset;
[Range(0,7)]
[SerializeField] private float intensity = 2;
        private void OnEnable()
        {
            playerEventBinding = EventBus<OnSwitchHeroEvent>.Register(HandleHeroSwitchEvent);
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

        private void Update()
        {
            if (nextWeaponPreset != null)
            {
                var material=nextWeaponPreset.GetComponent<MeshRenderer>().material;
                //Mathf.Clamp01( powerupPercent & 7) clamp between 0 and 7 the intensity based on power up loaded level
                material.SetColor("_EmissionColor", Color.white * Mathf.Pow(2, intensity));
            }
        }
    }
}