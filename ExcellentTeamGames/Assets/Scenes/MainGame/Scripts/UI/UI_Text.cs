using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Text : MonoBehaviour
{
    [SerializeField]
    public TMPro.TextMeshProUGUI m_TextMeshProUGUI;

    private void Update()
    {
        if(GameManager.Instance != null)
        m_TextMeshProUGUI.text = $"�o�ߎ��ԁF{GameManager.Instance.PlayTime.ToString("F1")}�b";
    }
}
