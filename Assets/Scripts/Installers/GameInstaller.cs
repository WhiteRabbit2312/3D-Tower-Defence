using UnityEngine;
using Zenject;
using TowerDefense.Managers;
using TowerDefense.Factories;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
using TowerDefense.UIMarket;

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
        
        [Header("Scene References")]
        [Tooltip("The Transform where enemies will be spawned.")]
        [SerializeField] private Transform _spawnPoint;
        [Tooltip("The Transform that enemies will use as their destination.")]
        [SerializeField] private Transform _pathTarget;

        public override void InstallBindings()
        {
            // Install the SignalBus
            SignalBusInstaller.Install(Container);

            // Declare all signals
            Container.DeclareSignal<EnemySpawnedSignal>();
            Container.DeclareSignal<EnemyDiedSignal>();
            Container.DeclareSignal<EnemyReachedEndSignal>();
            Container.DeclareSignal<WaveClearedSignal>();
            
            // Bind managers from prefabs
            Container.Bind<EconomyManager>().FromComponentInNewPrefab(_economyManagerPrefab).AsSingle().NonLazy();
            Container.Bind<EnemyManager>().FromComponentInNewPrefab(_enemyManagerPrefab).AsSingle().NonLazy();
            Container.Bind<BuildManager>().FromComponentInNewPrefab(_buildManagerPrefab).AsSingle().NonLazy();

            Container.Bind<UIManager>().AsSingle();
            
            // Bind factories to their interfaces
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<ITowerFactory>().To<TowerFactory>().AsSingle();

            // Bind scene references so they can be injected using WithId
            // We remove .AsSingle() because Zenject 6+ doesn't allow multiple .AsSingle bindings for the same type.
            // .FromInstance() is sufficient here.
            Container.Bind<Transform>().WithId("SpawnPoint").FromInstance(_spawnPoint);
            Container.Bind<Transform>().WithId("PathTarget").FromInstance(_pathTarget);
        }
    }
}