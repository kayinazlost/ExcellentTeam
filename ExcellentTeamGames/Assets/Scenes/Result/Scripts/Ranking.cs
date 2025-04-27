using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI[] _texts;

    private void Start()
    {
        var data = GameRecordManager.GetRanking();
        for (int i = 0; i < _texts.Length; i++) {
            var time = 0.0f;
            if (data.Length > i)
            {
                time = data[i].PlayTime;
            }
            _texts[i].text = time <= 0.1f ? $"{i + 1}ˆÊ [" : $"{i + 1}ˆÊ {time}•b";
        }
    }
}
