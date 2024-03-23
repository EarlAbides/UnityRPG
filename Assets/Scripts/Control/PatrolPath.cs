using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float gizmoWaypointRadius = 0.2f;

        void OnDrawGizmos() {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawSphere(GetWaypoint(i), gizmoWaypointRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        private Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        private int GetNextIndex(int i) 
        {
            return (i < transform.childCount - 1) ? i + 1 : 0;
        }
    }
}
