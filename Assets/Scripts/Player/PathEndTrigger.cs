using UnityEngine;
using Zenject;
using TowerDefense.Enemies;
using TowerDefense.Signals;

namespace TowerDefense.Core
{
    /// <summary>
    /// This component should be placed on a trigger collider at the end of the enemy path.
    /// It detects when an enemy enters it, fires a signal, and destroys the enemy.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PathEndTrigger : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();

            if (enemy != null && enemy.IsAlive)
            {
                _signalBus.Fire(new EnemyReachedEndSignal(enemy));

                Destroy(enemy.gameObject);
            }
        }
    }
}