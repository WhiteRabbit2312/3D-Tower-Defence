using UnityEngine;
using Zenject;
using TMPro;
using TowerDefense.Managers;

namespace TowerDefense.UI
{
    /// <summary>
    /// Updates a TextMeshPro UI element to display the player's current lives.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PlayerHealthUI : MonoBehaviour
    {
        private TextMeshProUGUI _livesText;
        private PlayerHealthManager _playerHealthManager;

        [Inject]
        public void Construct(PlayerHealthManager playerHealthManager)
        {
            _playerHealthManager = playerHealthManager;
        }

        private void Awake()
        {
            _livesText = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _playerHealthManager.OnLivesChanged += UpdateLivesText;
        }

        private void OnDisable()
        {
            _playerHealthManager.OnLivesChanged -= UpdateLivesText;
        }

        private void UpdateLivesText(int lives)
        {
            _livesText.text = $"Lives: {lives}";
        }
    }
}