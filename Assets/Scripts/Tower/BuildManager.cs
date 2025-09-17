using TowerDefense.Data;
using TowerDefense.Factories;
using TowerDefense.Interfaces;
using TowerDefense.Towers;
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
        private BaseTower _towerGhost; // The semi-transparent preview of the tower

        private ITowerFactory _towerFactory;
        private EconomyManager _economyManager;
        private Camera _mainCamera;

        [Inject]
        public void Construct(ITowerFactory towerFactory, EconomyManager economyManager)
        {
            _towerFactory = towerFactory;
            _economyManager = economyManager;
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        /// <summary>
        /// Called from UI buttons to select which tower to build.
        /// </summary>
        public void SelectTowerToBuild(TowerData towerData)
        {
            if (_economyManager.CurrentCurrency < towerData.BuildCost)
            {
                Debug.Log("Not enough currency to select this tower!");
                return;
            }

            _selectedTowerData = towerData;

            if (_towerGhost != null)
            {
                Destroy(_towerGhost.gameObject);
            }

            // Create a "ghost" preview of the tower
            _towerGhost = Instantiate(_selectedTowerData.TowerPrefab);
            _towerGhost.enabled = false; // Disable the tower's logic
            // You can add logic here to make the ghost semi-transparent
        }

        private void Update()
        {
            if (_selectedTowerData == null)
            {
                HandlePlatformClicks(); // Allow upgrading even when not in build mode
                return; // Not in build mode
            }
            
            MoveGhostToCursor();

            if (Input.GetMouseButtonDown(0)) // Left Click to place
            {
                TryPlaceTower();
            }

            if (Input.GetMouseButtonDown(1)) // Right Click to cancel
            {
                CancelBuildMode();
            }
        }

        private void HandlePlatformClicks()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TowerPlatform platform = GetPlatformUnderCursor();
                if (platform != null && platform.IsOccupied)
                {
                    platform.TryUpgradeTower();
                }
            }
        }

        private void MoveGhostToCursor()
        {
            if (_towerGhost == null) return;
            
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            // Raycast against a layer mask for the ground/platforms for better accuracy
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _towerGhost.transform.position = hit.point;
                
                TowerPlatform platform = hit.collider.GetComponent<TowerPlatform>();
                if (platform != null && !platform.IsOccupied)
                {
                    // Snap ghost to the platform's center
                    _towerGhost.transform.position = platform.transform.position;
                    // Here you would change the ghost's color to indicate valid placement (e.g., green)
                }
                else
                {
                    // Here you would change the ghost's color to indicate invalid placement (e.g., red)
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
                    newTower.Initialize(_selectedTowerData);
                    platform.SetPlacedTower(newTower);
                    // After placing, deselect the tower to prevent building multiple with one purchase
                    CancelBuildMode(); 
                }
                else
                {
                    Debug.Log("Something went wrong, not enough money!");
                    CancelBuildMode();
                }
            }
        }

        private void CancelBuildMode()
        {
            if (_towerGhost != null)
            {
                Destroy(_towerGhost.gameObject);
                _towerGhost = null;
            }
            _selectedTowerData = null;
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