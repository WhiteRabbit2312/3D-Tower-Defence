using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Core
{
    /// <summary>
    /// Defines a path for enemies to follow using a series of waypoints.
    //  This component should be placed on an empty GameObject in the scene.
    /// You can visually edit the path by moving the Transform children of this object.
    /// </summary>
    public class Path : MonoBehaviour
    {
        [Tooltip("The waypoint transforms. Should be ordered.")]
        [SerializeField] private List<Transform> _waypoints = new List<Transform>();

        /// <summary>
        /// The total number of waypoints in the path.
        /// </summary>
        public int WaypointCount => _waypoints.Count;

        private void OnDrawGizmos()
        {
            // Draws lines between waypoints in the editor for easy visualization.
            for (int i = 0; i < _waypoints.Count - 1; i++)
            {
                if(_waypoints[i] != null && _waypoints[i+1] != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
                    Gizmos.DrawSphere(_waypoints[i].position, 0.2f);
                }
            }
            if (_waypoints.Count > 0 && _waypoints[_waypoints.Count-1] != null)
            {
                Gizmos.DrawSphere(_waypoints[_waypoints.Count-1].position, 0.2f);
            }
        }

        /// <summary>
        /// Gets the position of a waypoint by its index.
        /// </summary>
        /// <param name="index">The index of the waypoint.</param>
        /// <returns>The position of the waypoint, or Vector3.zero if invalid.</returns>
        public Vector3 GetWaypointPosition(int index)
        {
            if (index >= 0 && index < _waypoints.Count)
            {
                return _waypoints[index].position;
            }
            return Vector3.zero; // Should not happen in normal flow
        }
    }
}