using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        private bool alreadyTriggered = true;

        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyTriggered && other.gameObject.CompareTag("Player"))
            {
                alreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }

        public object CaptureState()
        {
            return alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            alreadyTriggered = (bool)state;;
        }
    }
}
