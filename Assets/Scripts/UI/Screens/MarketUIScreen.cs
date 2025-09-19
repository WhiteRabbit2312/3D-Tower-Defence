using TowerDefense.Signals;
using UnityEngine;
using Zenject;

namespace TowerDefense.UI
{
    public class MarketUIScreen : UIScreen
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<TowerPlacedSignal>(OnTowerPlaced);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<TowerPlacedSignal>(OnTowerPlaced);
        }
        
        private void OnTowerPlaced()
        {
            Close();
        }
    }
}