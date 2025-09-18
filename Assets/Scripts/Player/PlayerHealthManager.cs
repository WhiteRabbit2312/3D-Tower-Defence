using UnityEngine;
using Zenject;
using TowerDefense.Signals;
using System;

namespace TowerDefense.Managers
{
    /// <summary>
    /// Manages the player's lives, listens for enemies reaching the end, and signals game over.
    /// </summary>
    public class PlayerHealthManager : MonoBehaviour
    {
        [Tooltip("The number of lives the player starts with.")]
        [SerializeField] private int _initialLives = 10;

        public int CurrentLives { get; private set; }

        public event Action<int> OnLivesChanged;

        private SignalBus _signalBus;
        private bool _isGameOver = false;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            CurrentLives = _initialLives;
            OnLivesChanged?.Invoke(CurrentLives);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemyReachedEndSignal>(OnEnemyReachedEnd);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<EnemyReachedEndSignal>(OnEnemyReachedEnd);
        }

        private void OnEnemyReachedEnd(EnemyReachedEndSignal signal)
        {
            if (_isGameOver) return;

            CurrentLives--;
            OnLivesChanged?.Invoke(CurrentLives);

            Debug.Log($"Player lost a life! Lives remaining: {CurrentLives}");

            if (CurrentLives <= 0)
            {
                CurrentLives = 0;
                _isGameOver = true;
                _signalBus.Fire(new PlayerDefeatedSignal());
                Debug.Log("Player has been defeated!");
            }
        }
    }
}