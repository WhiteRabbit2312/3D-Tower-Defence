using UnityEngine;

namespace TowerDefense.UIMarket
{
    public abstract class UIScreen : UIWindowBase
    {
        protected override void Awake()
        {
            base.Awake();
            if (OpenButton != null)
            {
                if (UiManager == null)
                {
                    Debug.LogError("UiManager is null");
                }
                
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