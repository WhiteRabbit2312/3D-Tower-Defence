using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TowerDefense.UI
{
    /// <summary>
    /// An abstract base class for all UI windows in the game.
    /// It provides the core functionality for opening and closing a UI panel
    /// using a CanvasGroup, and handles the injection of the UIManager.
    /// </summary>
    public abstract class UIWindowBase : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup Window;
        [SerializeField] protected Button OpenButton;
        [SerializeField] protected Button CloseButton;

        protected UIManager UiManager;

        [Inject]
        public void Construct(UIManager uiManager)
        {
            UiManager = uiManager;
        }

        protected virtual void Awake()
        {
            if (Window == null)
            {
                Window = this.GetComponent<CanvasGroup>();
            }
        }

        public virtual void Open()
        {
            Window.alpha = 1f;
            Window.interactable = true;
            Window.blocksRaycasts = true;
        }

        public virtual void Close()
        {
            Window.alpha = 0f;
            Window.interactable = false;
            Window.blocksRaycasts = false;
        }
    }
}