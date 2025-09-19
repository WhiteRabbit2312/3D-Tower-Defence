using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerDefense.Data;
using TowerDefense.Enemies;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
using Zenject;

namespace TowerDefense.Managers
{
    /// <summary>
    /// Manages the procedural generation and spawning of infinite enemy waves.
    /// It increases the difficulty over time by scaling enemy count and health,
    /// and spawns different enemy types based on defined probabilities.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        [Header("Wave Progression")]
        [SerializeField] private int _initialEnemyCount = 10;
        [SerializeField] [Range(1.0f, 2.0f)] private float _enemyCountMultiplier = 1.5f;
        [SerializeField] [Range(1.0f, 2.0f)] private float _healthMultiplier = 1.1f;

        [Header("Timing")]
        [SerializeField] private float _initialDelay = 5f;
        [SerializeField] private float _timeBetweenWaves = 5f;
        [SerializeField] private float _timeBetweenEnemies = 0.5f;

        [Header("Enemy Probabilities")]
        [Tooltip("A list of enemies and their spawn chances. The sum of chances should ideally be 100.")]
        [SerializeField] private List<ProbabilisticEnemy> _spawnableEnemies = new List<ProbabilisticEnemy>();

        public int CurrentWaveNumber { get; private set; } = 0;
        
        private float _currentHealthMultiplier = 1f;
        private int _enemiesForNextWave;
        private bool _isSpawningStopped = false; 

        private Transform _spawnPoint;
        private IEnemyFactory _enemyFactory;
        private EnemyManager _enemyManager;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(
            [Inject(Id = "SpawnPoint")] Transform spawnPoint, 
            IEnemyFactory enemyFactory,
            EnemyManager enemyManager,
            SignalBus signalBus)
        {
            _spawnPoint = spawnPoint;
            _enemyFactory = enemyFactory;
            _enemyManager = enemyManager;
            _signalBus = signalBus;
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<WaveClearedSignal>(OnWaveCleared);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<WaveClearedSignal>(OnWaveCleared);
        }

        private void Start()
        {
            _enemiesForNextWave = _initialEnemyCount;
            StartCoroutine(StartFirstWaveWithDelay());
        }

        private IEnumerator StartFirstWaveWithDelay()
        {
            yield return new WaitForSeconds(_initialDelay);
            StartNextWave();
        }
        
        /// <summary>
        /// Public method called by the GameManager to halt all spawning activities.
        /// </summary>
        public void StopSpawning()
        {
            _isSpawningStopped = true;
            StopAllCoroutines();
        }

        private void OnWaveCleared()
        {
            StartCoroutine(StartNextWaveWithDelay());
        }

        private IEnumerator StartNextWaveWithDelay()
        {
            if (_isSpawningStopped) yield break;
            yield return new WaitForSeconds(_timeBetweenWaves);
            StartNextWave();
        }
        
        private void StartNextWave()
        {
            StopAllCoroutines();
            StartCoroutine(SpawnWaveCoroutine());
        }
        
        // The main coroutine that handles the spawning of enemies for a single wave.
        private IEnumerator SpawnWaveCoroutine()
        {
            CurrentWaveNumber++;
            
            _enemyManager.PrepareForWave(_enemiesForNextWave);
           
            for (int i = 0; i < _enemiesForNextWave; i++)
            {
                if (_isSpawningStopped) yield break;
                EnemyData enemyToSpawn = GetRandomEnemy();
                if (enemyToSpawn != null)
                {
                    SpawnEnemy(enemyToSpawn);
                }
                yield return new WaitForSeconds(_timeBetweenEnemies);
            }
            
            // Prepare for the next wave
            _enemiesForNextWave = Mathf.CeilToInt(_enemiesForNextWave * _enemyCountMultiplier);
            _currentHealthMultiplier *= _healthMultiplier;
        }

        // Handles the creation of a single enemy instance.
        private void SpawnEnemy(EnemyData enemyData)
        {
            BaseEnemy newEnemy = _enemyFactory.Create(enemyData, _spawnPoint.position);
            if (newEnemy != null)
            {
                newEnemy.Setup(enemyData, _currentHealthMultiplier);
            }
        }

        // Selects an enemy to spawn based on the weighted probabilities.
        private EnemyData GetRandomEnemy()
        {
            if (_spawnableEnemies.Count == 0)
            {
                return null;
            }

            float totalChance = _spawnableEnemies.Sum(e => e.SpawnChance);
            float randomValue = Random.Range(0, totalChance);

            float currentChance = 0f;
            foreach (var enemy in _spawnableEnemies)
            {
                currentChance += enemy.SpawnChance;
                if (randomValue <= currentChance)
                {
                    return enemy.EnemyData;
                }
            }
            
            // Fallback in case of rounding errors
            return _spawnableEnemies.Last().EnemyData;
        }
    }
}