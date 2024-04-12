using System;
using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 2f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 1f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                print("Loading scene!");
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindFirstObjectByType<Fader>();

            DisablePlayerControl();
            yield return fader.FadeOut(fadeOutTime);

            SavingWrapper savingWrapper = FindFirstObjectByType<SavingWrapper>();
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            DisablePlayerControl();
            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            EnablePlayerControl();
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsByType<Portal>(FindObjectsSortMode.None))
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        void DisablePlayerControl()
        {
            GetPlayerController().DisableControl();
        }

        void EnablePlayerControl()
        {
            GetPlayerController().EnableControl();
        }

        private PlayerController GetPlayerController()
        {
            return GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

    }
}