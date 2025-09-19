using UnityEngine;
using TowerDefense.Signals;
using System;
using UnityEngine.Serialization;
using Zenject;

namespace TowerDefense.Managers
{
    /// <summary>
    /// Manages the player's currency.
    /// Listens for signals to add currency (e.g., when an enemy dies).
    /// Provides methods for spending currency and an event for UI to listen to.
    /// </summary>
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] private int _startingCurrency = 100;
        public int CurrentCurrency { get; private set; }
        
        public event Action<int> OnCurrencyChanged;
        
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void Awake()
        {
            CurrentCurrency = _startingCurrency;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemyDiedSignal>(HandleEnemyDeath);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<EnemyDiedSignal>(HandleEnemyDeath);
        }

        private void HandleEnemyDeath(EnemyDiedSignal signal)
        {
            AddCurrency(signal.Enemy.CurrencyValue);
        }

        public void AddCurrency(int amount)
        {
            CurrentCurrency += amount;
            OnCurrencyChanged?.Invoke(CurrentCurrency);
        }

        public bool TrySpendCurrency(int amount)
        {
            if (CurrentCurrency >= amount)
            {
                CurrentCurrency -= amount;
                OnCurrencyChanged?.Invoke(CurrentCurrency);
                return true;
            }
            return false;
        }
    }
}