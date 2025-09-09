using UnityEngine;
using Zenject;
using TowerDefense.Managers;
using TowerDefense.Core;
using TowerDefense.Signals; // Import the signals namespace

namespace TowerDefense.Installers
{
    /// <summary>
    /// Main Zenject installer for the game scene.
    /// Now uses SignalBus for event handling.
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private EconomyManager economyManagerPrefab;
        [SerializeField] private EnemyManager enemyManagerPrefab;
        [SerializeField] private TowerFactory towerFactoryPrefab;

        public override void InstallBindings()
        {
            // Install the SignalBus
            SignalBusInstaller.Install(Container);

            // Declare all signals that the SignalBus will manage.
            // This is required for Zenject to know about them.
            Container.DeclareSignal<EnemySpawnedSignal>();
            Container.DeclareSignal<EnemyDiedSignal>();
            
            // The rest of the bindings remain the same.
            Container.Bind<EconomyManager>().FromComponentInNewPrefab(economyManagerPrefab).AsSingle().NonLazy();
            Container.Bind<EnemyManager>().FromComponentInNewPrefab(enemyManagerPrefab).AsSingle().NonLazy();
            Container.Bind<TowerFactory>().FromComponentInNewPrefab(towerFactoryPrefab).AsSingle().NonLazy();
        }
    }
}