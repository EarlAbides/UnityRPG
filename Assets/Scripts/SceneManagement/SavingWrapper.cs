using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        [SerializeField] float fadeInTime = 0.2f;
        SavingSystem savingSystem;

        IEnumerator Start()
        {
            savingSystem = GetComponent<SavingSystem>();

            Fader fader = FindFirstObjectByType<Fader>();
            fader.FadeOutImmediate();
            yield return savingSystem.LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }
    }
}