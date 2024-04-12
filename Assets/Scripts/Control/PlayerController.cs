
using System;
using System.Linq;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        Mover mover;

        [Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float navMeshProjectionDistance = 1f;
        [SerializeField] float maxNavPathLength = 40f;

        private void Awake()
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (DeathBehavior()) return;

            if (InteractWithComponent()) return;
            if (MovementBehavior()) return;

            SetCursor(CursorType.None);
        }

        public void DisableControl()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<PlayerController>().enabled = false;
        }

        public void EnableControl()
        {
            GetComponent<PlayerController>().enabled = true;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool DeathBehavior()
        {
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return true;
            }

            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastReceivers = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable receiver in raycastReceivers)
                {
                    if (receiver.HandleRaycast(this))
                    {
                        SetCursor(receiver.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            return Physics.RaycastAll(GetMouseRay())
                .OrderBy(h => h.distance)
                .ToArray();
        }

        private bool MovementBehavior()
        {
            if (RaycastNavMesh(out Vector3 target))
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, navMeshProjectionDistance, NavMesh.AllAreas
            );
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            return CanReachMeshTarget(target);
        }

        private bool CanReachMeshTarget(Vector3 target)
        {
            NavMeshPath path = new();
            if (NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete && path.corners.Length >= 2)
                {
                    float distance = 0f;
                    for (int i = 1; i < path.corners.Length; i++)
                    {
                        distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                    }
                    if (distance < maxNavPathLength) return true;
                }
            }

            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            return cursorMappings.Where(m => m.type == type).First();
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}