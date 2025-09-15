using UnityEngine;

namespace TowerDefense.UI
{
    public class UIManager
    {
        private UIScreen _currentScreen;

        public void OpenScreen(UIScreen screen)
        {
            if (_currentScreen != null && _currentScreen != screen)
            {
                _currentScreen.Close();
            }

            _currentScreen = screen;
            _currentScreen.Open();
        }

        public void CloseScreen(UIScreen screen)
        {
            if (_currentScreen == screen)
            {
                _currentScreen.Close();
                _currentScreen = null;
            }
        }
    }
}