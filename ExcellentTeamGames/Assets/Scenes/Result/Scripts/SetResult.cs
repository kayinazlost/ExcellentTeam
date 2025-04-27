using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SetResult : MonoBehaviour
{
    public Text m_Text;
    void Start()
    {
        var data = GameRecordManager.GetHistory();
        Debug.Log(data.Length);
        if (data.Length != 0)
            m_Text.text = data[0].PlayTime.ToString("F1")+"ïb";
        else
            m_Text.text = "ÉfÅ[É^Ç»Çµ";
    }
}
