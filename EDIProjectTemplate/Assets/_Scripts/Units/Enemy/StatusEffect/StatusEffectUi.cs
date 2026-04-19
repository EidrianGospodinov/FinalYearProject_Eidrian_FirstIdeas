using System;
using System.Collections.Generic;
using _Scripts.Units.Player.Core;
using GDX.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Units.Enemy.StatusEffect
{
    public class StatusEffectIconCache
    {
        public GameObject StatusIconContainer;
        public Image StatusBuildupFill;
        public Image StatusActiveTimerFill;
        public Image StatusIcon;

        public StatusEffectIconCache(GameObject go, Image buildup, Image timer, Image statusIcon)
        {
            StatusIconContainer = go;
            StatusBuildupFill = buildup;
            StatusActiveTimerFill = timer;
            StatusIcon = statusIcon;
        }
    }
    public class StatusEffectUi : MonoBehaviour
    {
        [Inject] private ICameraService cameraService;
        [SerializeField] private GameObject statusEffectIconTemplate;
        [SerializeField] private SerializableDictionary<StatusEffectType, Sprite> statusEffectSpriteDict;
        
        private Dictionary<StatusEffectSO, StatusEffectIconCache> statusEffectToDict;

        private StatusEffectManager statusEffectManager;
        private void Start()
        {
            statusEffectToDict = new Dictionary<StatusEffectSO, StatusEffectIconCache>();
            statusEffectManager = GetComponentInParent<StatusEffectManager>();

            statusEffectManager.ActiveStatus += OnActiveStatus;
            statusEffectManager.UpdateStatusEffect += OnUpdateStatusEffect;
            statusEffectManager.DeactivateStatusEffect += OnDeactivateStatusEffect;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.parent.position - cameraService.MainCamera.transform.position);
        }

        private StatusEffectIconCache CreateStatusIcon(StatusEffectSO statusEffect)
        {
            if (statusEffectToDict.ContainsKey(statusEffect))
            {
                statusEffectToDict[statusEffect].StatusIconContainer.SetActive(true);
                return statusEffectToDict[statusEffect];
            }

            GameObject createStatusIcon = Instantiate(statusEffectIconTemplate, transform);
            GameObject statusActiveTimer = createStatusIcon.transform.Find("StatusActiveTimer").gameObject;//todo: reference this trough a subclass
            Image statusBuildupRadialFill = createStatusIcon.GetComponent<Image>();
            statusBuildupRadialFill.fillAmount = 0;

            Image statusActiveTimerRadialFill = statusActiveTimer.GetComponent<Image>();
            statusActiveTimerRadialFill.fillAmount = 0;
            
            Image statusIcon = createStatusIcon.transform.Find("Icon").GetComponent<Image>();//todo: same here
            statusIcon.sprite = statusEffectSpriteDict[statusEffect.EffectType];

            createStatusIcon.SetActive(true);
            return new StatusEffectIconCache(createStatusIcon, statusBuildupRadialFill, statusActiveTimerRadialFill,
                statusIcon);

        }
        private void OnActiveStatus(StatusEffectSO statusEffect,float buildAmount)
        {
            StatusEffectIconCache statusEffectIconCache = CreateStatusIcon(statusEffect);
            statusEffectToDict[statusEffect] = statusEffectIconCache;

            OnUpdateStatusEffect(statusEffect, buildAmount, 10);
        }

        private void OnUpdateStatusEffect(StatusEffectSO statusEffect,float buildAmount, float duration)
        {
            statusEffectToDict[statusEffect].StatusBuildupFill.fillAmount = buildAmount;
            statusEffectToDict[statusEffect].StatusActiveTimerFill.fillAmount = duration;
        }

        private void OnDeactivateStatusEffect(StatusEffectSO statusEffect)
        {
            statusEffectToDict[statusEffect].StatusIconContainer.SetActive(false);
        }
        
    }
}