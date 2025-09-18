using UnityEngine;
using Zenject;
using TowerDefense.Managers;
using TowerDefense.Factories;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
using TowerDefense.Towers;
using TowerDefense.Towers.Projectiles;
using TowerDefense.UI;

namespace TowerDefense.Installers
{
    /// <summary>
    /// Main Zenject installer for the game scene.
    /// Combines bindings for core managers, signals, and the enemy factory system.
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
        [Header("Manager Prefabs")]
        [SerializeField] private EconomyManager _economyManagerPrefab;
        [SerializeField] private EnemyManager _enemyManagerPrefab;
        [SerializeField] private BuildManager _buildManagerPrefab;
        [SerializeField] private PlayerHealthManager _playerHealthManagerPrefab;
        [SerializeField] private GameManager _gameManagerPrefab;
        [SerializeField] private WaveManager _waveManagerPrefab;
        
        [Header("Scene References")]
        [Tooltip("The Transform where enemies will be spawned.")]
        [SerializeField] private Transform _spawnPoint;
        [Tooltip("The Transform that enemies will use as their destination.")]
        [SerializeField] private Transform _pathTarget;

        public override void InstallBindings()
        {
            Debug.LogError("InstallBindings");
            // Install the SignalBus
            SignalBusInstaller.Install(Container);

            // Declare all signals
            Container.DeclareSignal<EnemySpawnedSignal>();
            Container.DeclareSignal<EnemyDiedSignal>();
            Container.DeclareSignal<EnemyReachedEndSignal>();
            Container.DeclareSignal<WaveClearedSignal>();
            Container.DeclareSignal<TowerPlacedSignal>();
            Container.DeclareSignal<PlayerDefeatedSignal>();
            
            // Bind managers from prefabs, making them available for injection
            Container.Bind<EconomyManager>().FromComponentInNewPrefab(_economyManagerPrefab).AsSingle().NonLazy();
            Container.Bind<EnemyManager>().FromComponentInNewPrefab(_enemyManagerPrefab).AsSingle().NonLazy();
            Container.Bind<BuildManager>().FromComponentInNewPrefab(_buildManagerPrefab).AsSingle().NonLazy();
            Container.Bind<PlayerHealthManager>().FromComponentInNewPrefab(_playerHealthManagerPrefab).AsSingle().NonLazy();
            Container.Bind<GameManager>().FromComponentInNewPrefab(_gameManagerPrefab).AsSingle().NonLazy();
            
            // --- THIS WAS MISSING ---
            // Now Zenject knows how to create and provide the WaveManager
            Container.Bind<WaveManager>().FromComponentInNewPrefab(_waveManagerPrefab).AsSingle().NonLazy();

            // Bind UI components
            Container.Bind<UIManager>().AsSingle();
            
            // Bind UI panels that exist in the scene
            Container.Bind<UpgradeSellPanel>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameOverScreen>().FromComponentInHierarchy().AsSingle();
            
            // Bind factories to their interfaces
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<ITowerFactory>().To<TowerFactory>().AsSingle();
            Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle();

            // Bind scene references with specific IDs
            Container.Bind<Transform>().WithId("SpawnPoint").FromInstance(_spawnPoint);
            Container.Bind<Transform>().WithId("PathTarget").FromInstance(_pathTarget);
        }
    }
}