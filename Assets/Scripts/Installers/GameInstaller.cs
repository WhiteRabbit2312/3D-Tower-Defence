using TowerDefense.Core;
using UnityEngine;
using Zenject;
using TowerDefense.Managers;
using TowerDefense.Factories;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
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
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<EnemySpawnedSignal>();
            Container.DeclareSignal<EnemyDiedSignal>();
            Container.DeclareSignal<EnemyReachedEndSignal>();
            Container.DeclareSignal<WaveClearedSignal>();
            Container.DeclareSignal<TowerPlacedSignal>();
            Container.DeclareSignal<PlayerDefeatedSignal>();
            
            Container.Bind<EconomyManager>().FromComponentInNewPrefab(_economyManagerPrefab).AsSingle().NonLazy();
            Container.Bind<EnemyManager>().FromComponentInNewPrefab(_enemyManagerPrefab).AsSingle().NonLazy();
            Container.Bind<BuildManager>().FromComponentInNewPrefab(_buildManagerPrefab).AsSingle().NonLazy();
            Container.Bind<PlayerHealthManager>().FromComponentInNewPrefab(_playerHealthManagerPrefab).AsSingle().NonLazy();
            Container.Bind<GameManager>().FromComponentInNewPrefab(_gameManagerPrefab).AsSingle().NonLazy();
            
            Container.Bind<WaveManager>().FromComponentInNewPrefab(_waveManagerPrefab).AsSingle().NonLazy();

            Container.Bind<UIManager>().AsSingle();
            
            Container.Bind<UpgradeSellPopup>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameOverScreen>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<ITowerFactory>().To<TowerFactory>().AsSingle();
            Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle();

            Container.Bind<Transform>().WithId(Constants.SpawnPointKey).FromInstance(_spawnPoint);
            Container.Bind<Transform>().WithId(Constants.PathTargetKey).FromInstance(_pathTarget);
        }
    }
}