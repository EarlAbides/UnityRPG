using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        Fighter fighter;
        GameObject player;
        Health health;

        void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            if (health.IsDead()) return;

            if (CombatBehavior()) return;
        }

        private bool CombatBehavior()
        {
            bool playerInRange = Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
            if (playerInRange && fighter.CanAttack(player))
            {
                fighter.Attack(player);
                return true;
            }

            fighter.Cancel();
            return false;
        }
    }
}