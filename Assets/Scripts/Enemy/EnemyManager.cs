using UnityEngine;
using Zenject;
using TowerDefense.Enemies;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
using System.Collections.Generic;
using System.Linq;


namespace TowerDefense.Managers
{
    /// <summary>
    /// Subscribes to signals from the SignalBus to manage the list of active enemies.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        public List<ITargetable> ActiveEnemies { get; private set; } = new List<ITargetable>();
        
        private SignalBus _signalBus;
        private int _enemiesRemainingInWave;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemySpawnedSignal>(HandleEnemySpawn);
            _signalBus.Subscribe<EnemyDiedSignal>(HandleEnemyDeath);
            _signalBus.Subscribe<EnemyReachedEndSignal>(HandleEnemyReachedEnd); // Handle enemies that reach the end
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<EnemySpawnedSignal>(HandleEnemySpawn);
            _signalBus.Unsubscribe<EnemyDiedSignal>(HandleEnemyDeath);
            _signalBus.Unsubscribe<EnemyReachedEndSignal>(HandleEnemyReachedEnd);
        }

        public void PrepareForWave(int enemyCount)
        {
            _enemiesRemainingInWave = enemyCount;
        }

        private void HandleEnemySpawn(EnemySpawnedSignal signal)
        {
            if (signal.Enemy != null && !ActiveEnemies.Contains(signal.Enemy))
            {
                ActiveEnemies.Add(signal.Enemy);
            }
        }

        private void HandleEnemyDeath(EnemyDiedSignal signal)
        {
            RemoveActiveEnemy(signal.Enemy);
        }

        private void HandleEnemyReachedEnd(EnemyReachedEndSignal signal)
        {
            // An enemy that reaches the end should also be removed from the active list
            RemoveActiveEnemy(signal.Enemy);
        }
        
        private void RemoveActiveEnemy(ITargetable enemy)
        {
            if (enemy != null && ActiveEnemies.Contains(enemy))
            {
                ActiveEnemies.Remove(enemy);
            }
            
            _enemiesRemainingInWave--;
            
            // Check if this was the last enemy of the wave
            if (_enemiesRemainingInWave <= 0)
            {
                // Fire a signal so other systems (like WaveManager) know the wave is cleared.
                _signalBus.Fire(new WaveClearedSignal());
                Debug.Log("Wave Cleared!");
            }
        }
        
        public void DestroyAllEnemies()
        {
            // We iterate over a copy of the list because the original list will be modified
            // inside the loop when enemies are destroyed and fire their death signals.
            foreach (var enemy in ActiveEnemies.ToList())
            {
                if (enemy != null && enemy.GetTransform() != null)
                {
                    Destroy(enemy.GetTransform().gameObject);
                }
            }
            // Clear the list to ensure a clean state.
            ActiveEnemies.Clear();
        }
    }
}