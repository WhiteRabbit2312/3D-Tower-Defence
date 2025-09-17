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
        [SerializeField] private TextMeshProUGUI _upgradeCostText;
        [SerializeField] private TextMeshProUGUI _sellValueText;

        private BaseTower _selectedTower;
        private EconomyManager _economyManager;

        // The UIManager is already injected in the base class (UIWindowBase)
        // We just need to inject our other dependencies.
        [Inject]
        public void Construct(EconomyManager economyManager)
        {
            _economyManager = economyManager;
        }

        /// <summary>
        /// We override Awake to provide our own logic instead of the base UIPopup's logic.
        /// </summary>
        protected override void Awake()
        {
            // Call the base Awake from UIWindowBase to initialize the CanvasGroup
            base.Awake(); 
            
            _upgradeButton.onClick.AddListener(OnUpgradePressed);
            _sellButton.onClick.AddListener(OnSellPressed);
            
            // Start hidden using the inherited method
            Close(); 
        }

        /// <summary>
        /// Shows the panel for a specific tower. This is the entry point called by BuildManager.
        /// </summary>
        public void Show(BaseTower tower)
        {
            _selectedTower = tower;
            transform.position = _selectedTower.transform.position;
            UpdatePanelInfo();
            
            // Use the inherited Open() method to show the UI
            Open();
        }

        /// <summary>
        /// Hides the panel from view.
        /// </summary>
        public void Hide()
        {
            // Use the inherited Close() method to hide the UI
            Close();
            _selectedTower = null;
        }

        /// <summary>
        /// Updates the text on the buttons based on the selected tower's state.
        /// </summary>
        private void UpdatePanelInfo()
        {
            if (_selectedTower == null) return;
            
            // Update Sell Button
            int sellValue = _selectedTower.GetTotalInvestedCost() / 2; // 50% refund
            _sellValueText.text = $"Sell\n(${sellValue})";

            // Update Upgrade Button
            int nextLevel = _selectedTower.CurrentLevel + 1;
            if (nextLevel >= _selectedTower.TowerData.GetMaxLevel())
            {
                _upgradeButton.interactable = false;
                _upgradeCostText.text = "MAX";
            }
            else
            {
                _upgradeButton.interactable = true;
                int upgradeCost = _selectedTower.TowerData.GetUpgradeCost(nextLevel);
                _upgradeCostText.text = $"Upgrade\n(${upgradeCost})";
                // Optionally, disable button if player can't afford it
                _upgradeButton.interactable = _economyManager.CurrentCurrency >= upgradeCost;
            }
        }

        private void OnUpgradePressed()
        {
            if (_selectedTower != null)
            {
                _selectedTower.Upgrade();
                // After upgrading, update the panel info again.
                // If the player can't afford the next upgrade, the button will become disabled.
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
        
        // We don't need OnDestroy because we are not using the base popup's button logic.
    }
}