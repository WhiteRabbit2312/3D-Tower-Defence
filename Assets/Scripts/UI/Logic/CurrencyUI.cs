using TMPro;
using TowerDefense.Managers;
using UnityEngine;
using Zenject;

namespace TowerDefense.UI
{
    /// <summary>
    /// Updates the UI text element to display the player's current currency.
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

        private void UpdateCurrencyText(int newAmount)
        {
            _currencyText.text = $"Coins: {newAmount}";
        }
    }
}