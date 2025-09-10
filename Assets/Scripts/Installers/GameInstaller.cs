using UnityEngine;
using Zenject;
using TowerDefense.Managers;
using TowerDefense.Core;
using TowerDefense.Signals;
using UnityEngine.Serialization; // Import the signals namespace

namespace TowerDefense.Installers
{
    /// <summary>
    /// Main Zenject installer for the game scene.
    /// Now uses SignalBus for event handling.
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private EconomyManager _economyManagerPrefab;
        [SerializeField] private EnemyManager _enemyManagerPrefab;
        [SerializeField] private TowerFactory _towerFactoryPrefab;

        public override void InstallBindings()
        {
            // Install the SignalBus
            SignalBusInstaller.Install(Container);

            // Declare all signals that the SignalBus will manage.
            // This is required for Zenject to know about them.
            Container.DeclareSignal<EnemySpawnedSignal>();
            Container.DeclareSignal<EnemyDiedSignal>();
            
            // The rest of the bindings remain the same.
            Container.Bind<EconomyManager>().FromComponentInNewPrefab(_economyManagerPrefab).AsSingle().NonLazy();
            Container.Bind<EnemyManager>().FromComponentInNewPrefab(_enemyManagerPrefab).AsSingle().NonLazy();
            Container.Bind<TowerFactory>().FromComponentInNewPrefab(_towerFactoryPrefab).AsSingle().NonLazy();
        }
    }
}