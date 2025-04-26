using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerTab : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI posTxt;

    /// <summary>
    /// �F�ƃe�L�X�g���ꊇ�Őݒ肷��
    /// </summary>
    /// <param name="color">Image�̐F</param>
    /// <param name="textValue">�\�����镶����</param>
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
