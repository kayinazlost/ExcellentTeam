using UnityEngine;
using UnityEngine.UI;

public class UI_HpGauge : MonoBehaviour
{
    [SerializeField] private Image gaugeImage;

    private void Update()
    {
        if (gaugeImage == null || GameManager.Instance == null) return;

        float maxTime = GameManager.Instance.MaxTime;
        float elapsedTime = GameManager.Instance.GetElapsedTime();
        float remainingRatio = Mathf.Clamp01(1f - (elapsedTime / maxTime));
        gaugeImage.fillAmount = remainingRatio;

        if (remainingRatio <= 0f)
        {
            GameManager.Instance.GameOver();
        }
    }
}
