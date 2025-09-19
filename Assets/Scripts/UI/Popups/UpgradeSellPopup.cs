using TowerDefense.Managers;
using TowerDefense.Towers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using TowerDefense.Data;

namespace TowerDefense.UI
{
    /// <summary>
    /// Manages the UI panel that appears when a built tower is clicked.
    /// Inherits from UIPopup to be part of the UI system.
    /// </summary>
    public class UpgradeSellPopup : UIPopup
    {
        [Header("Buttons")]
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _denyButton;
        [SerializeField] private Button _priorityButton;

        [Header("Info Texts")]
        [SerializeField] private TextMeshProUGUI _towerLevelText;
        [SerializeField] private TextMeshProUGUI _upgradeCostText;
        [SerializeField] private TextMeshProUGUI _sellValueText;
        
        [Header("Stat Display Texts")]
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _specialStatText;

        private BaseTower _selectedTower;
        private EconomyManager _economyManager;
        
        private readonly Color _upgradeColor = Color.green;

        [Inject]
        public void Construct(EconomyManager economyManager)
        {
            _economyManager = economyManager;
        }

        protected override void Awake()
        {
            base.Awake(); 
            _upgradeButton.onClick.AddListener(OnUpgradePressed);
            _sellButton.onClick.AddListener(OnSellPressed); 
            _priorityButton.onClick.AddListener(OnPriorityPressed);
            if (_denyButton != null) _denyButton.onClick.AddListener(Hide);
            
            Close(); 
        }

        public void Show(BaseTower tower)
        {
            _selectedTower = tower;
            UpdatePanelInfo();
            Open();
        }

        public void Hide()
        {
            Close();
            _selectedTower = null;
        }

        private void UpdatePanelInfo()
        {
            if (_selectedTower == null) return;
            
            var towerData = _selectedTower.TowerData;
            int currentLevel = _selectedTower.CurrentLevel;
            int nextLevel = currentLevel + 1;
            
            _towerLevelText.text = $"Level: {currentLevel}";
            int sellValue = _selectedTower.GetTotalInvestedCost() / 2;
            _sellValueText.text = $"Sell (${sellValue})";
            int upgradeCost = towerData.GetUpgradeCost(nextLevel);
            _upgradeCostText.text = $"Upgrade (${upgradeCost})";
            _upgradeButton.interactable = _economyManager.CurrentCurrency >= upgradeCost;

            UpdateStatText(_damageText, "Damage", towerData.GetDamage(currentLevel), towerData.GetDamage(nextLevel));
            
            if (_priorityButton != null)
            {
                _priorityButton.GetComponentInChildren<TMP_Text>().text = $"{_selectedTower.CurrentPriority}";
            }
            
            if (towerData is MachineGunTowerData)
            {
                _specialStatText.gameObject.SetActive(true);
                UpdateStatText(_specialStatText, "Range", towerData.GetRange(currentLevel), towerData.GetRange(nextLevel));
            }
            else if (towerData is SlowingTowerData)
            {
                _specialStatText.gameObject.SetActive(true);
                float currentSlowPercent = (1 - towerData.GetSlowMultiplier(currentLevel)) * 100;
                float nextSlowPercent = (1 - towerData.GetSlowMultiplier(nextLevel)) * 100;
                UpdateStatText(_specialStatText, "Slow", currentSlowPercent, nextSlowPercent, "%");
            }
            else
            {
                if (_specialStatText != null)
                {
                    _specialStatText.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// A helper method to format the stat text and color it green if it improves.
        /// </summary>
        private void UpdateStatText(TextMeshProUGUI textField, string label, float currentValue, float nextValue, string suffix = "")
        {
            if (textField == null) return;

            textField.color = Color.white;
            
            string baseText = $"{label}: {currentValue:F1}{suffix}";
            
            if (nextValue > currentValue)
            {
                textField.text = $"{baseText} -> <color=#{ColorUtility.ToHtmlStringRGB(_upgradeColor)}>{nextValue:F1}{suffix}</color>";
            }
            else
            {
                textField.text = baseText;
            }
        }

        private void OnUpgradePressed()
        {
            if (_selectedTower != null)
            {
                _selectedTower.Upgrade();
                UpdatePanelInfo();
            }
        }

        private void OnSellPressed()
        {
            if (_selectedTower != null)
            {
                int sellValue = _selectedTower.GetTotalInvestedCost() / 2;
                _economyManager.AddCurrency(sellValue);
                _selectedTower.Platform.ClearPlacedTower(); 
                Destroy(_selectedTower.gameObject);
                Hide();
            }
        }
        
        private void OnPriorityPressed()
        {
            if (_selectedTower != null)
            {
                _selectedTower.CycleTargetingPriority();
                UpdatePanelInfo();
            }
        }
    }
}