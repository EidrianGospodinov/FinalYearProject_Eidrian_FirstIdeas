using TMPro;
using UnityEngine;

public class SkillLevel : MonoBehaviour
    {
        public const int DefaultMaxLevel = 1;
        public const int DefaultCurrentLevel = 0;
        
        [SerializeField] public TMP_Text levelMaxText;
        [SerializeField] public TMP_Text currentLevelText;
        [SerializeField] public int initialCurrentLevel = -1;
        [SerializeField] public Color activeTextColor;
        [SerializeField] public Color disabledTextColor;
        
        
        private int _currentLevel;
        /// <summary>
        /// max level is set from the skillTree button based on the upgrades available
        /// </summary>
        private int _maxLevel;

        public void Awake()
        {
            InitValues();
        }

        private void InitValues()
        {
            var currentLevel = initialCurrentLevel >= 0 ? initialCurrentLevel : DefaultCurrentLevel;
            SetCurrentLevel(currentLevel);
        }

        public void SetMaxLevel(int value)
        {
            _maxLevel = value;
            
            if (_maxLevel < _currentLevel)
                SetCurrentLevel(_maxLevel);
            
            levelMaxText.text = _maxLevel.ToString();
        }

        public void SetCurrentLevel(int value)
        {
            if (value < 0 || value > _maxLevel) return;
            
            _currentLevel = value;
            currentLevelText.text = _currentLevel.ToString();
            SetColors();
        }

        private void SetColors()
        {
            if (IsEmptyLevels())
            {
                currentLevelText.color = disabledTextColor;
                levelMaxText.color = disabledTextColor;
            }
            else
            {
                currentLevelText.color = activeTextColor;
                levelMaxText.color = activeTextColor;
            }
        }

        public void IncrementLevel() => SetCurrentLevel(_currentLevel + 1);

        public void DecrementLevel() => SetCurrentLevel(_currentLevel - 1);

        public int GetCurrentLevel() => _currentLevel;

        public int GetMaxLevel() => _maxLevel;

        public bool HaveLevels() => _currentLevel > 0;

        public bool IsFullLevels() => _currentLevel == _maxLevel;

        public bool IsEmptyLevels() => _currentLevel == 0;

        
    }