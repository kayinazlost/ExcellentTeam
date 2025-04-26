using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Sound/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    [System.Serializable]
    public class SoundEntry
    {
        public string name; // �Ăяo���p�̖��O
        public AudioClip clip; // �Đ�����N���b�v
    }

    public List<SoundEntry> sounds = new List<SoundEntry>();
    public AudioClip GetClip(string name)
    {
        foreach (var item in sounds)
        {
            if (item.name == name)
                return item.clip;
        }
        Debug.LogWarning($"'{name}' ��������܂���ł����I");
        return null;
    }
}
