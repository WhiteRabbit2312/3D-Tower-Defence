using UnityEngine;
using Zenject;
using TowerDefense.Enemies;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
using System.Collections.Generic;


namespace TowerDefense.Managers
{
    /// <summary>
    /// Subscribes to signals from the SignalBus to manage the list of active enemies.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        public IReadOnlyList<ITargetable> ActiveEnemies => _activeEnemies;
        private readonly List<ITargetable> _activeEnemies = new List<ITargetable>();

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemySpawnedSignal>(OnEnemySpawned);
            _signalBus.Subscribe<EnemyDiedSignal>(OnEnemyDied);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<EnemySpawnedSignal>(OnEnemySpawned);
            _signalBus.Unsubscribe<EnemyDiedSignal>(OnEnemyDied);
        }

        private void OnEnemySpawned(EnemySpawnedSignal signal)
        {
            var enemy = signal.Enemy;
            if (!_activeEnemies.Contains(enemy))
            {
                _activeEnemies.Add(enemy);
            }
        }
        
        private void OnEnemyDied(EnemyDiedSignal signal)
        {
            var enemy = signal.Enemy;
            if (_activeEnemies.Contains(enemy))
            {
                _activeEnemies.Remove(enemy);
            }
        }
    }
}