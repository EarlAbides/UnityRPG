using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        void Update()
        {
            if (health.IsDead()) return;

            if (PlayerInRange() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0f;
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                GuardBehavior();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private bool PlayerInRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }

        private void AttackBehavior()
        {
            fighter.Attack(player);
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void GuardBehavior()
        {
            mover.StartMoveAction(guardPosition);
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}