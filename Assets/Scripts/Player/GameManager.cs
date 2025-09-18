using TowerDefense.Signals;
using TowerDefense.UI;
using UnityEngine;
using Zenject;

namespace TowerDefense.Managers
{
    /// <summary>
    /// A high-level manager that listens for critical game state changes and coordinates the game over sequence.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private GameOverScreen _gameOverScreen;
        private WaveManager _waveManager;
        private EnemyManager _enemyManager;

        [Inject]
        public void Construct(
            SignalBus signalBus, 
            GameOverScreen gameOverScreen,
            WaveManager waveManager,
            EnemyManager enemyManager)
        {
            _signalBus = signalBus;
            _gameOverScreen = gameOverScreen;
            _waveManager = waveManager;
            _enemyManager = enemyManager;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerDefeatedSignal>(OnPlayerDefeated);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerDefeatedSignal>(OnPlayerDefeated);
        }

        private void OnPlayerDefeated()
        {
            Debug.LogError("GAME OVER - Starting cleanup sequence.");

            _waveManager.StopSpawning();
            _enemyManager.DestroyAllEnemies();
            _gameOverScreen.Open();
        }
    }
}