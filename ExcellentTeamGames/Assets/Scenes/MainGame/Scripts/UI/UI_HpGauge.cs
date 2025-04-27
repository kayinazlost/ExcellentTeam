using UnityEngine;

public class UI_HpGauge : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] gauges;
    [SerializeField] private float minWidth = 3.5f; // 最小幅（0%のとき）
    [SerializeField] private Animator animator;

    private float maxWidth; // 初期幅（100%のとき）

    private int state = 0;

    private void Start()
    {
        if (gauges != null && gauges.Length > 0)
        {
            // 最初のgaugeから初期のXサイズを取得
            maxWidth = gauges[0].size.x;
        }
    }

    private void Update()
    {
        if (gauges == null || gauges.Length == 0 || GameManager.Instance == null) return;

        float maxTime = GameManager.Instance.MaxTime;
        float elapsedTime = GameManager.Instance.GetElapsedTime();
        float remainingRatio = Mathf.Clamp01(1f - (elapsedTime / maxTime));

        float newWidth = Mathf.Lerp(minWidth, maxWidth, remainingRatio);

        foreach (var gauge in gauges)
        {
            if (gauge != null)
            {
                Vector2 size = gauge.size;
                size.x = newWidth;
                gauge.size = size;
            }
        }

        if(remainingRatio > 0.7f)
        {
            if(state!= 0)
            animator.SetTrigger("Excellent");
            state = 0;
        }
        else if (remainingRatio > 0.4f)
        {
            if(state!= 1)
            animator.SetTrigger("Good");
            state = 1;

        }
        else
        {

            if(state!= 2)
            animator.SetTrigger("Bad");
            state = 2;
        }


        if (remainingRatio <= 0f)
        {
            GameManager.Instance.GameOver();
        }
    }
}
