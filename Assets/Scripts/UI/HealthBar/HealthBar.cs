using RPG.Attributes;
using UnityEngine;

namespace RPG.UI.HealthBar
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        private void Update()
        {
            float healthPercent = health.GetPercentHealth();

            if (!IsCanvasEnabled(healthPercent)) return;
            foreground.localScale = new Vector3(healthPercent, 1, 1);
            rootCanvas.enabled = (healthPercent > 0f) && (healthPercent < 1f);
        }

        private bool IsCanvasEnabled(float healthPercent)
        {
            if (Mathf.Approximately(healthPercent, 0) || Mathf.Approximately(healthPercent, 1))
            {
                rootCanvas.enabled = false;
            }
            else
            {
                rootCanvas.enabled = true;
            }

            return rootCanvas.enabled;
        }
    }
}
