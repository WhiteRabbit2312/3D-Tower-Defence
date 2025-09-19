using UnityEngine;

namespace TowerDefense.UI
{
    /// <summary>
    /// An abstract base class for primary UI screens.
    /// This script automatically hooks up its Open and Close buttons to the UIManager.
    /// </summary>
    public abstract class UIScreen : UIWindowBase
    {
        protected override void Awake()
        {
            base.Awake();
            if (OpenButton != null)
            {
                OpenButton.onClick.AddListener(() => UiManager.OpenScreen(this));
            }

            if (CloseButton != null)
            {
                CloseButton.onClick.AddListener(() => UiManager.CloseScreen(this));
            }
        }

        private void OnDestroy()
        {
            if (OpenButton != null)
            {
                OpenButton.onClick.RemoveListener(() => UiManager.OpenScreen(this));
            }

            if (CloseButton != null)
            {
                CloseButton.onClick.RemoveListener(() => UiManager.CloseScreen(this));
            }
        }
    }
}