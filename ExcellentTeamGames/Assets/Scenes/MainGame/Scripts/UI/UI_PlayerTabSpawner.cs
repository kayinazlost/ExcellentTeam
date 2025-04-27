using UnityEngine;
using System.Collections.Generic;

public class UI_PlayerTabSpawner : MonoBehaviour
{
    [SerializeField] private UI_PlayerTab playerTabPrefab;
    [SerializeField] private List<Color> playerColors = new List<Color>();

    private List<UI_PlayerTab> spawnedTabs = new List<UI_PlayerTab>();

    /// <summary>
    /// �w�����PlayerTab�𐶐����ăZ�b�g����
    /// </summary>
    /// <param name="count">����������i1�`4�j</param>
    public void SpawnTabs(int count)
    {
        // ���S�΍�
        count = Mathf.Clamp(count, 1, 4);

        // ���łɐ����ς݂Ȃ��U�S������
        foreach (var tab in spawnedTabs)
        {
            if (tab != null)
            {
                Destroy(tab.gameObject);
            }
        }
        spawnedTabs.Clear();

        for (int i = 0; i < count; i++)
        {
            // �v���n�u���琶��
            var tab = Instantiate(playerTabPrefab, transform);
            spawnedTabs.Add(tab);

            // �F�ƃe�L�X�g���Z�b�g
            Color color = (i < playerColors.Count) ? playerColors[i] : Color.white;
            string text = $"Player {i + 1}";
            switch (i)
            {
                case 0:
                    text += "[A]";
                    break;
                case 1:
                    text += "[��]";
                    break;
                case 2:
                    text += "[B]";
                    break;
                default:
                    text += "[C]";
                    break;
            }


            tab.Set(color, text);
        }
    }

    /// <summary>
    /// �v���C���[�̈ʒu���X�g���󂯎���āA�Ή�����UI���X�V����
    /// </summary>
    public void UpdatePlayerPositions(List<float> posXs)
    {
        for (int i = 0; i < spawnedTabs.Count && i < posXs.Count; i++)
        {
            spawnedTabs[i].SetPosTxt(posXs[i]);
        }
    }
}
