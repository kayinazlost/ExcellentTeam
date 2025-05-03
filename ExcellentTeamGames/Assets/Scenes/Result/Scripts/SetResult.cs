using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using unityroom.Api;

public class SetResult : MonoBehaviour
{
    public GameObject[] resultHankoObj;
    public TMPro.TextMeshProUGUI m_Text;

    private float _rate = 0f;
    void Start()
    {
        var data = GameRecordManager.GetHistory();
        Debug.Log(data.Length);
        if (data.Length != 0)
        {
            _rate = (300f - data[0].PlayTime) / 300f;
            Invoke("Stamp", 2f);
#if UNITY_WEBGL
            // ボードNo1にスコアを送信する。
            UnityroomApiClient.Instance.SendScore(1, data[0].PlayTime, ScoreboardWriteMode.HighScoreDesc);
#endif
            m_Text.text = data[0].PlayTime.ToString("F1") + "秒";
        }
        else
            m_Text.text = "データなし";
    }

    private void Stamp()
    {
        if(this == null) return;
        if (_rate > 0.7f)
        {
            resultHankoObj[0].SetActive(true);
        }
        else if (_rate > 0.4f)
        {
            resultHankoObj[1].SetActive(true);
        }
        else
        {
            resultHankoObj[2].SetActive(true);
        }
    }
}
