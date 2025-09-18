using TMPro; // Use TextMeshPro for modern UI text
using TowerDefense.Managers;
using UnityEngine;
using Zenject;

namespace TowerDefense.UI
{
    /// <summary>
    /// Updates the UI text element to display the player's current currency.
    /// This component should be placed on the TextMeshPro - UGUI object for currency.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CurrencyUI : MonoBehaviour
    {
        private TextMeshProUGUI _currencyText;
        private EconomyManager _economyManager;

        [Inject]
        public void Construct(EconomyManager economyManager)
        {
            _economyManager = economyManager;
        }

        private void Awake()
        {
            _currencyText = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _economyManager.OnCurrencyChanged += UpdateCurrencyText;
            
            if(_economyManager != null)
            {
                UpdateCurrencyText(_economyManager.CurrentCurrency);
            }
        }

        private void OnDisable()
        {
            if (_economyManager != null)
            {
                _economyManager.OnCurrencyChanged -= UpdateCurrencyText;
            }
        }

        /// <summary>
        /// This method is called by the OnCurrencyChanged event from the EconomyManager.
        /// </summary>
        /// <param name="newAmount">The new currency amount provided by the event.</param>
        private void UpdateCurrencyText(int newAmount)
        {
            // You can customize the format here (e.g., add a dollar sign)
            _currencyText.text = $"Coins: {newAmount}";
        }
    }
}