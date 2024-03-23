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
                Gizmos.DrawSphere(transform.GetChild(i).position, gizmoWaypointRadius);
            }
        }
    }
}
