using TowerDefense.Data;
using TowerDefense.Factories;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
using TowerDefense.Towers;
using TowerDefense.UI;
using UnityEngine;
using Zenject;

namespace TowerDefense.Managers
{
    /// <summary>
    /// Manages the tower building process, including selecting, previewing, and placing towers.
    /// </summary>
    public class BuildManager : MonoBehaviour
    {
        private TowerData _selectedTowerData;
        private BaseTower _towerGhost;

        private ITowerFactory _towerFactory;
        private EconomyManager _economyManager;
        private UpgradeSellPopup _upgradeSellPopup;
        private SignalBus _signalBus;
        private Camera _mainCamera;
        
        private float _platformClickCooldown;

        [Inject]
        public void Construct(
            ITowerFactory towerFactory,
            EconomyManager economyManager,
            UpgradeSellPopup upgradeSellPopup,
            SignalBus signalBus)
        {
            _towerFactory = towerFactory;
            _economyManager = economyManager;
            _upgradeSellPopup = upgradeSellPopup;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void SelectTowerToBuild(TowerData towerData)
        {
            if (_economyManager.CurrentCurrency < towerData.BuildCost) return;

            _selectedTowerData = towerData;
            if (_towerGhost != null) Destroy(_towerGhost.gameObject);
            _towerGhost = Instantiate(_selectedTowerData.TowerPrefab);
            _towerGhost.enabled = false;
        }

        private void Update()
        {
            if (_platformClickCooldown > 0)
            {
                _platformClickCooldown -= Time.deltaTime;
            }

            if (_selectedTowerData != null)
            {
                MoveGhostToCursor();

                if (Input.GetMouseButtonDown(0))
                {
                    TryPlaceTower();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    CancelBuildMode();
                }
            }
            else
            {
                HandlePlatformClicks();
            }
        }

        private void HandlePlatformClicks()
        {
            if (_platformClickCooldown > 0)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                TowerPlatform platform = GetPlatformUnderCursor();
                if (platform != null && platform.IsOccupied)
                {
                    _upgradeSellPopup.Show(platform.PlacedTower);
                }
            }
        }

        private void TryPlaceTower()
        {
            TowerPlatform platform = GetPlatformUnderCursor();
            if (platform != null && !platform.IsOccupied)
            {
                if (_economyManager.TrySpendCurrency(_selectedTowerData.BuildCost))
                {
                    BaseTower newTower = _towerFactory.CreateTower(_selectedTowerData, platform.transform.position);
                    newTower.Initialize(_selectedTowerData, platform);
                    platform.SetPlacedTower(newTower);

                    _signalBus.Fire(new TowerPlacedSignal());

                    CancelBuildMode();
                }
                else
                {
                    CancelBuildMode();
                }
            }
        }

        private void CancelBuildMode()
        {
            if (_towerGhost != null) Destroy(_towerGhost.gameObject);
            _towerGhost = null;
            _selectedTowerData = null;
            _platformClickCooldown = 0.1f;
        }

        private void MoveGhostToCursor()
        {
            if (_towerGhost == null) return;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _towerGhost.transform.position = hit.point;
                TowerPlatform platform = hit.collider.GetComponent<TowerPlatform>();
                if (platform != null && !platform.IsOccupied)
                {
                    _towerGhost.transform.position = platform.transform.position;
                }
            }
        }

        private TowerPlatform GetPlatformUnderCursor()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.collider.GetComponent<TowerPlatform>();
            }

            return null;
        }
    }
}