using UnityEngine;
using System.Collections.Generic;

public class UI_PlayerTabSpawner : MonoBehaviour
{
    [SerializeField] private UI_PlayerTab playerTabPrefab;
    [SerializeField] private List<Color> playerColors = new List<Color>();

    private List<UI_PlayerTab> spawnedTabs = new List<UI_PlayerTab>();

    /// <summary>
    /// 指定個数分PlayerTabを生成してセットする
    /// </summary>
    /// <param name="count">生成する個数（1～4）</param>
    public void SpawnTabs(int count)
    {
        // 安全対策
        count = Mathf.Clamp(count, 1, 4);

        // すでに生成済みなら一旦全部消す
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
            // プレハブから生成
            var tab = Instantiate(playerTabPrefab, transform);
            spawnedTabs.Add(tab);

            // 色とテキストをセット
            Color color = (i < playerColors.Count) ? playerColors[i] : Color.white;
            string text = $"Player {i + 1}";
            switch (i)
            {
                case 0:
                    text += "[A]";
                    break;
                case 1:
                    text += "[→]";
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
    /// プレイヤーの位置リストを受け取って、対応するUIを更新する
    /// </summary>
    public void UpdatePlayerPositions(List<float> posXs)
    {
        for (int i = 0; i < spawnedTabs.Count && i < posXs.Count; i++)
        {
            spawnedTabs[i].SetPosTxt(posXs[i]);
        }
    }
}
