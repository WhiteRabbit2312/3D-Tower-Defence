using UnityEngine;
using TowerDefense.Signals;
using System;
using Zenject;

namespace TowerDefense.Managers
{
    /// <summary>
    /// Subscribes to the EnemyDiedSignal to grant currency.
    /// </summary>
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] private int startingCurrency = 100;
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
            CurrentCurrency = startingCurrency;
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
            AddCurrency(signal.Enemy.currencyValue);
        }

        public void AddCurrency(int amount)
        {
            CurrentCurrency += amount;
            OnCurrencyChanged?.Invoke(CurrentCurrency);
        }

        public bool SpendCurrency(int amount)
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