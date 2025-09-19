using UnityEngine;
using System;
using System.Collections.Generic;

namespace TowerDefense.Data
{
    /// <summary>
    /// A ScriptableObject that defines a single wave of enemies.
    /// </summary>
    [CreateAssetMenu(fileName = "NewWaveData", menuName = "Tower Defense/Wave Data", order = 1)]
    public class WaveData : ScriptableObject
    {
        [Serializable]
        public class SpawnGroup
        {
            [Tooltip("The type of enemy to spawn.")]
            public EnemyData EnemyData;
            
            [Tooltip("The number of enemies to spawn in this group.")]
            public int Count = 5;
            
            [Tooltip("The time delay between each enemy spawn in this group.")]
            public float SpawnInterval = 0.5f;
        }

        [Tooltip("A list of enemy groups to spawn in this wave.")]
        public List<SpawnGroup> SpawnGroups = new List<SpawnGroup>();
    }
}