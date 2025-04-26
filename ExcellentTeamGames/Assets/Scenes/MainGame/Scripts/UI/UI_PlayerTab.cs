using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerTab : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI posTxt;

    /// <summary>
    /// 色とテキストを一括で設定する
    /// </summary>
    /// <param name="color">Imageの色</param>
    /// <param name="textValue">表示する文字列</param>
    public void Set(Color color, string textValue)
    {
        if (image != null)
        {
            image.color = color;
        }

        if (nameTxt != null)
        {
            nameTxt.text = textValue;
        }
    }

    public void SetPosTxt(float posX)
    {
        posTxt.text = $"{posX:F1}m";
    }
}
