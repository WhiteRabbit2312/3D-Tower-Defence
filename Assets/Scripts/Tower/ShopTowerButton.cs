using TowerDefense.Data;
using TowerDefense.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TowerDefense.UIMarket
{
    /// <summary>
    /// This script should be placed on a UI Button in your shop.
    /// It tells the BuildManager which tower the player wants to build.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ShopTowerButton : MonoBehaviour
    {
        [Tooltip("The data asset for the tower this button represents.")]
        [SerializeField] private TowerData _towerData;

        private BuildManager _buildManager;
        private Button _button;

        [Inject]
        public void Construct(BuildManager buildManager)
        {
            _buildManager = buildManager;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (_towerData != null)
            {
                _buildManager.SelectTowerToBuild(_towerData);
            }
        }
    }
}