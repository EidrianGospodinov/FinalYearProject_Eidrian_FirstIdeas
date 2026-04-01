using System;
using System.Resources;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.New_Folder.SkillTree
{
    public class SkillAmount : MonoBehaviour
    {
        [SerializeField] private TMP_Text availableText;
        [Inject] private CurrencyManager CurrencyManager;
        private int available => CurrencyManager.CurrentCurrency;
        private void Start()
        {
        }

        public bool CanSpend(int spend) => available >= spend;

        public bool TrySpend(int spend)
        {
            var result =CurrencyManager.TrySpend(spend);
            UpdateSkillAmountText();
            return result;
        }

        public void UpdateSkillAmountText()
        {
            availableText.text = available.ToString();
        }
    }
}