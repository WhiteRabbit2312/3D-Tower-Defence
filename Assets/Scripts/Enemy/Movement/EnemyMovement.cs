using UnityEngine;
using Zenject;
using TowerDefense.Core;
using TowerDefense.Signals;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// Handles enemy movement using Unity's NavMeshAgent.
    /// </summary>
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        private BaseEnemy _baseEnemy;
        private SignalBus _signalBus;
        private UnityEngine.AI.NavMeshAgent _agent;
        private Transform _target;

        // We use WithId to avoid conflicts if we decide to inject other Transforms later.
        [Inject]
        public void Construct([Inject(Id = "PathTarget")] Transform target, SignalBus signalBus)
        {
            _target = target;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _baseEnemy = GetComponent<BaseEnemy>();
            _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        private void Start()
        {
            if (_target == null)
            {
                Debug.LogError("Path Target is not set in GameInstaller!", this);
                return;
            }
            
            // Configure the agent based on enemy stats
            _agent.speed = _baseEnemy.MoveSpeed;
            
            // Set the destination for the agent
            _agent.SetDestination(_target.position);
        }

        private void Update()
        {
            // The agent handles movement automatically. We just need to check if it has reached the destination.
            // We check if the agent is active, has a path, and the remaining distance is less than a small threshold.
            if (_agent.hasPath && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                ReachEnd();
            }
        }
        
        private void ReachEnd()
        {
            // To prevent this from firing multiple times, we disable the component.
            this.enabled = false; 

            _signalBus.Fire(new EnemyReachedEndSignal(_baseEnemy));
            
            // Destroy the enemy object.
            Destroy(gameObject);
        }
    }
}