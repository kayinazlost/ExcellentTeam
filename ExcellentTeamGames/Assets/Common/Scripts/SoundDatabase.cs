using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Sound/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    [System.Serializable]
    public class SoundEntry
    {
        public string name; // 呼び出し用の名前
        public AudioClip clip; // 再生するクリップ
    }

    public List<SoundEntry> sounds = new List<SoundEntry>();
    public AudioClip GetClip(string name)
    {
        foreach (var item in sounds)
        {
            if (item.name == name)
                return item.clip;
        }
        Debug.LogWarning($"'{name}' が見つかりませんでした！");
        return null;
    }
}
