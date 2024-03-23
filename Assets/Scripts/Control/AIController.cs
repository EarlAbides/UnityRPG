using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        GameObject player;
        Fighter fighter;

        void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            if (ChaseBehavior()) return;
        }

        private bool ChaseBehavior()
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