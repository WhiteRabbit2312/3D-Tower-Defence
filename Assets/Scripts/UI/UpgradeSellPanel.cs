using TowerDefense.Managers;
using TowerDefense.Towers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

namespace TowerDefense.UIMarket
{
    /// <summary>
    /// Manages the UI panel that appears when a built tower is clicked.
    /// Inherits from UIPopup to be part of the UI system.
    /// </summary>
    public class UpgradeSellPanel : UIPopup // Inherit from your UIPopup
    {
        [Header("Panel-Specific References")]
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _denyButton;
        [SerializeField] private TextMeshProUGUI _upgradeCostText;
        [SerializeField] private TextMeshProUGUI _sellValueText;
        [SerializeField] private TextMeshProUGUI _towerLevelText;

         private BaseTower _selectedTower;
        private EconomyManager _economyManager;

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
            _denyButton.onClick.AddListener(Hide); 
        }

        public void Show(BaseTower tower)
        {
            _selectedTower = tower;
            // You might want to position the panel above the tower
            // This requires converting world position to screen/canvas position
            // For now, we'll just show it.
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
            
            // Display current tower level
            if (_towerLevelText != null)
            {
                _towerLevelText.text = $"Level: {_selectedTower.CurrentLevel}";
            }

            // Update Sell Button
            int sellValue = _selectedTower.GetTotalInvestedCost() / 2; // 50% refund
            _sellValueText.text = $"Sell\n(${sellValue})";

            // --- CRITICAL FIX FOR INFINITE UPGRADES ---
            // We no longer check for a max level. We always calculate the next one.
            int nextLevel = _selectedTower.CurrentLevel + 1;
            int upgradeCost = _selectedTower.TowerData.GetUpgradeCost(nextLevel);

            _upgradeCostText.text = $"Upgrade\n(${upgradeCost})";
            _upgradeButton.interactable = _economyManager.CurrentCurrency >= upgradeCost;
        }

        private void OnUpgradePressed()
        {
            if (_selectedTower != null)
            {
                _selectedTower.Upgrade();
                // After upgrading, update the panel info again for the next level.
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
    }
}