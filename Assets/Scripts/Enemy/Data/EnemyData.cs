using UnityEngine;

namespace TowerDefense.Data
{
    /// <summary>
    /// A ScriptableObject to hold the base stats for a type of enemy.
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Tower Defense/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Tooltip("The prefab for this enemy, containing the model and necessary components.")]
        public GameObject EnemyPrefab;
        
        [Tooltip("The base health of the enemy at wave 1.")]
        public float BaseHealth = 100f;

        [Tooltip("The base movement speed of the enemy.")]
        public float BaseMoveSpeed = 3.5f;

        [Tooltip("The amount of currency awarded for killing this enemy.")]
        public int CurrencyValue = 10;
    }
}