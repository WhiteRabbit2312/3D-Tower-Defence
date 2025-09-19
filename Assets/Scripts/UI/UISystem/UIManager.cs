using UnityEngine;

namespace TowerDefense.UI
{
    public class UIManager
    {
        private UIScreen _currentScreen;
        private UIPopup _currentPopup;

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
        
        public void OpenPopup(UIPopup popup)
        {
            if (_currentPopup != null && _currentPopup != popup)
            {
                _currentPopup.Close();
            }

            _currentPopup = popup;
            _currentPopup.Open();
        }

        public void ClosePopup(UIPopup popup)
        {
            if (_currentPopup == popup)
            {
                _currentPopup.Close();
                _currentPopup = null;
            }
        }
    }
}