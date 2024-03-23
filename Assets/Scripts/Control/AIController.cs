using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        GameObject player;

        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            ChaseBehavior();
        }

        private void ChaseBehavior()
        {
            bool playerInRange = Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
            if (playerInRange)
            {
                print(gameObject.name + " should chase!");
            }
        }
    }
}