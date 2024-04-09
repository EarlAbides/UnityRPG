
using System;
using System.Linq;
using RPG.Attributes;
using RPG.Movement;
using UnityEngine;
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
            if (Physics.Raycast(GetMouseRay(), out RaycastHit hit))
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(hit.point, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
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