using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerDefense.UI
{
    /// <summary>
    /// A UI screen that is shown when the player is defeated.
    /// Inherits from UIScreen to be managed by the UIManager.
    /// </summary>
    public class GameOverScreen : UIScreen
    {
        [Tooltip("Button to restart the current level.")]
        [SerializeField] private Button _restartButton;

        protected override void Awake()
        {
            base.Awake();

            if (_restartButton != null)
            {
                _restartButton.onClick.AddListener(RestartGame);
            }
            
            Close();
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}