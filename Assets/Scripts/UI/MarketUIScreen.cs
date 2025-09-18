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

        // We use OnEnable/OnDisable to safely subscribe/unsubscribe to signals
        private void OnEnable()
        {
            _signalBus.Subscribe<TowerPlacedSignal>(OnTowerPlaced);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<TowerPlacedSignal>(OnTowerPlaced);
        }

        /// <summary>
        /// This method is called by the SignalBus when a TowerPlacedSignal is fired.
        /// </summary>
        private void OnTowerPlaced()
        {
            Debug.LogError("OnTowerPlaced");
            // Close the market screen when a tower is successfully built
            // We use the Close() method inherited from UIWindowBase
            Close();
        }
    }
}