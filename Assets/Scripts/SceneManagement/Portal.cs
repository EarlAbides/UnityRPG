using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;

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
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            print("Scene loaded");
            Destroy(gameObject);
        }
    }
}