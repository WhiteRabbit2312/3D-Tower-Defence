using UnityEngine;
using System.Collections;
using TowerDefense.Data;
using TowerDefense.Enemies;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
using Zenject;

namespace TowerDefense.Managers
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Wave Template")]
        [Tooltip("A ScriptableObject that defines the base composition of enemies for each wave.")]
        [SerializeField] private WaveData _waveTemplate;
        
        [Header("Wave Timing")]
        [SerializeField] private float _timeBetweenWaves = 5f;
        [Tooltip("The initial delay before the very first wave starts.")]
        [SerializeField] private float _initialDelay = 3f;

        [Header("Difficulty Scaling Per Wave")]
        [Tooltip("Each wave, total health will be multiplied by this value. 1.1 = +10% health per wave.")]
        [SerializeField] private float _healthMultiplier = 1.1f;
        [Tooltip("Each wave, this many extra enemies will be added to each spawn group.")]
        [SerializeField] private int _extraEnemiesPerGroup = 1;

        public int CurrentWaveNumber { get; private set; } = 0;
        
        private Transform _spawnPoint;
        private IEnemyFactory _enemyFactory;
        private EnemyManager _enemyManager;
        private SignalBus _signalBus;
        
        private float _currentHealthMultiplier = 1.0f;

        [Inject]
        public void Construct(
            SignalBus signalBus,
            [Inject(Id = "SpawnPoint")] Transform spawnPoint, 
            IEnemyFactory enemyFactory,
            EnemyManager enemyManager)
        {
            _signalBus = signalBus;
            _spawnPoint = spawnPoint;
            _enemyFactory = enemyFactory;
            _enemyManager = enemyManager;
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
            StartCoroutine(StartFirstWaveAfterDelay());
        }

        private void OnWaveCleared()
        {
            // When a wave is cleared, start the next one after a delay.
            StartCoroutine(StartNextWaveAfterDelay());
        }

        private IEnumerator StartFirstWaveAfterDelay()
        {
            yield return new WaitForSeconds(_initialDelay);
            StartNextWave();
        }

        private IEnumerator StartNextWaveAfterDelay()
        {
            yield return new WaitForSeconds(_timeBetweenWaves);
            StartNextWave();
        }

        private void StartNextWave()
        {
            CurrentWaveNumber++;
            
            if (CurrentWaveNumber > 1)
            {
                // Apply health scaling for all waves after the first
                _currentHealthMultiplier *= _healthMultiplier;
            }

            StopAllCoroutines();
            StartCoroutine(SpawnWaveCoroutine());
        }
        
        private IEnumerator SpawnWaveCoroutine()
        {
            if (_waveTemplate == null)
            {
                Debug.LogError("Wave Template is not assigned in the WaveManager!", this);
                yield break;
            }
            
            int totalEnemiesInWave = 0;
            // Calculate total enemies for EnemyManager based on the current wave number
            foreach (var group in _waveTemplate.SpawnGroups)
            {
                totalEnemiesInWave += group.Count + (_extraEnemiesPerGroup * (CurrentWaveNumber - 1));
            }

            _enemyManager.PrepareForWave(totalEnemiesInWave);

            Debug.Log($"Starting Wave {CurrentWaveNumber} with {totalEnemiesInWave} enemies. Health Multiplier: {_currentHealthMultiplier:F2}x");
            
            // Spawn enemies based on the template
            foreach (var group in _waveTemplate.SpawnGroups)
            {
                int enemiesInGroup = group.Count + (_extraEnemiesPerGroup * (CurrentWaveNumber - 1));
                
                for (int i = 0; i < enemiesInGroup; i++)
                {
                    SpawnEnemy(group.EnemyData);
                    yield return new WaitForSeconds(group.SpawnInterval);
                }
            }
            
            Debug.Log($"Wave {CurrentWaveNumber} spawning complete.");
        }

        private void SpawnEnemy(EnemyData enemyData)
        {
            BaseEnemy newEnemy = _enemyFactory.Create(enemyData, _spawnPoint.position);

            if (newEnemy != null)
            {
                newEnemy.Setup(enemyData, _currentHealthMultiplier);
            }
        }
    }
}