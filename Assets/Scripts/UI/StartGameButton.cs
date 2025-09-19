using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerDefense.UI
{
    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        private Button _button;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}