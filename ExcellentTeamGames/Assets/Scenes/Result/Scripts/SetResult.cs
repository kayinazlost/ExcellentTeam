using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using unityroom.Api;

public class SetResult : MonoBehaviour
{
    public TMPro.TextMeshProUGUI m_Text;
    void Start()
    {
        var data = GameRecordManager.GetHistory();
        Debug.Log(data.Length);
        if (data.Length != 0)
        {
#if UNITY_WEBGL
            // �{�[�hNo1�ɃX�R�A�𑗐M����B
            UnityroomApiClient.Instance.SendScore(1, data[0].PlayTime, ScoreboardWriteMode.HighScoreDesc);
#endif
            m_Text.text = data[0].PlayTime.ToString("F1") + "�b";
        }
        else
            m_Text.text = "�f�[�^�Ȃ�";
    }
}
