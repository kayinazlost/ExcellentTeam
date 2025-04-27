using Mandegan;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameRecordData {
    public float PlayTime;

}
public class GameRecordManager
{
    public static void Save()
    {
        var saveData = SaveLoadManager.Load("SKGJ_ETeam");
        // �v���C�����iSaveData.ScoreListType.History�j��0�Ԗڂ�����Ă����
        var data = saveData.GetData<GameRecordData>(SaveData.ScoreListType.History, 0);
        if (data == null)
        {
            // �Z�[�u�f�[�^��V�K�쐬
            data = new GameRecordData();
        }
        data.PlayTime = GameManager.Instance.PlayTime;
        var t = (int)(data.PlayTime * 100f);
        saveData.AddData(data, t);
        SaveLoadManager.Save(saveData);
    }

    public static SaveData Load()
    {
        return SaveLoadManager.Load("SKGJ_ETeam");
    }

    public static GameRecordData[] GetHistory()
    {
        var saveData = SaveLoadManager.Load("SKGJ_ETeam");
        var list = new List<GameRecordData>();
        for (int i = 0; i < saveData.scoreHistory.Count; i++)
        {
            var data = saveData.GetData<GameRecordData>(SaveData.ScoreListType.History, i);
            list.Add(data);
        }
        return list.ToArray();
    }

    public static GameRecordData[] GetRanking()
    {
        var saveData = SaveLoadManager.Load("SKGJ_ETeam");
        var list = new List<GameRecordData>();
        for (int i = 0; i < saveData.scoreRanking.Count; i++)
        {
            var data = saveData.GetData<GameRecordData>(SaveData.ScoreListType.Ranking, i);
            list.Add(data);
        }
        return list.ToArray();
    }
}

namespace Mandegan
{
    [Serializable]
    public class SaveData
    {
        public enum ScoreListType {
            History,
            Ranking
        }

        public SaveData(string gameTitle)
        {
            GameTitle = gameTitle;
        }

        [Serializable]
        public struct ScoreData
        {
            public int rankScore;
            public string jsonData;
            public string day;
        }

        /// <summary>
        /// �X�R�A��ǉ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scoreData">�C�ӂ̃f�[�^�i���l�E�N���X�ȂǁA�Ȃ�ł��j</param>
        /// <param name="rankScore">�����L���O�ɓo�^���邽�߂̃X�R�A�i�g�[�^���l�Ȃǁj</param>
        public void AddData<T>(T scoreData, int rankScore)
        {
            // �f�[�^��JSON�ɃV���A���C�Y
            string jsonData = JsonUtility.ToJson(scoreData, true);
            // �V�����X�R�A�f�[�^���쐬
            var data = new ScoreData();
            data.jsonData = jsonData;
            data.day = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            // scoreHistory �ɒǉ�
            if (scoreHistory == null)
                scoreHistory = new List<ScoreData>();
            scoreHistory.Insert(0, data);
            while (scoreHistory.Count > 30)
                scoreHistory.RemoveAt(scoreHistory.Count - 1);

            // scoreRanking �Ƀ����L���O�Ƃ��Ēǉ�
            if (scoreRanking == null)
                scoreRanking = new List<ScoreData>();
            scoreRanking.Add(data);

            // �X�R�A�Ń\�[�g���ď��30������ێ�
            scoreRanking = scoreRanking.OrderByDescending(x => x.rankScore).Take(30).ToList();
        }
        public T GetData<T>(ScoreListType listType, int num)
        {
            try
            {
                // JSON�f�[�^���w�肵���^�Ƀf�V���A���C�Y
                if (scoreHistory.Count > num)
                {
                    switch (listType)
                    {
                        case ScoreListType.History:
                            {
                                T data = JsonUtility.FromJson<T>(scoreHistory[num].jsonData);
                                return data;
                            }
                        case ScoreListType.Ranking:
                            {
                                T data = JsonUtility.FromJson<T>(scoreRanking[num].jsonData);
                                return data;
                            }
                    }

                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load data: " + e.Message);
                return default;
            }

            return default;
        }

        public readonly string GameTitle = "Mandegan";
        public List<ScoreData> scoreHistory = new List<ScoreData>();
        public List<ScoreData> scoreRanking = new List<ScoreData>();
    }

    public class SaveLoadManager
    {
        public static bool Save(SaveData data)
        {
            return DataSave(data);

        }
        public static SaveData Load(string gameTitle)
        {
            // �Q�[���̃Z�[�u�f�[�^��ǂݍ���
            SaveData saveData = LoadSaveData(gameTitle);

            // �ǂݍ��񂾃f�[�^��\��
            Debug.Log($"GameTitle: {saveData.GameTitle}, History: {saveData.scoreHistory}");

            return saveData;
        }

        private static bool DataSave(SaveData data)
        {
            if (data.GameTitle == "")
            {
                Debug.LogError("�Z�[�u�f�[�^�̏�񂪕s�����Ă��܂�");
                return false;
            }

            // �t�@�C���p�X���\�z
            string folderPath = Path.Combine(Application.persistentDataPath, "MandeganSaveData");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // JSON�ɃV���A���C�Y���ĈÍ����E���k������
            var jsonData = Compressor.CompressString(JsonUtility.ToJson(data));

            var filePath = Path.Combine(folderPath, $"{data.GameTitle}.savedata");

            // JSON���t�@�C���ɕۑ�
            File.WriteAllBytes(filePath, jsonData);

            Debug.Log("�f�[�^���ۑ�����܂���: " + filePath);

            return true;
        }

        private static SaveData LoadSaveData(string gameTitle)
        {
            // �t�H���_�p�X���\�z
            string folderPath = Path.Combine(Application.persistentDataPath, "MandeganSaveData");

            if (Directory.Exists(folderPath))
            {
                // gameTitle�ƈ�v���閼�O��json�t�@�C���̃p�X���\�z
                string jsonFilePath = Path.Combine(folderPath, $"{gameTitle}.savedata");

                if (File.Exists(jsonFilePath))
                {
                    // �t�@�C������JSON�������ǂݍ���ŉ�ǂ���
                    string jsonData = Compressor.DecompressString(File.ReadAllBytes(jsonFilePath));

                    // JSON�������SaveData�I�u�W�F�N�g�Ƀf�V���A���C�Y
                    var load = JsonUtility.FromJson<SaveData>(jsonData);
                    var data = new SaveData(gameTitle);
                    data.scoreHistory = load.scoreHistory;
                    data.scoreRanking = load.scoreRanking;

                    // �f�V���A���C�Y�����f�[�^��Ԃ�
                    return data;
                }
                else
                {
                    Debug.LogWarning("��v����JSON�t�@�C����������܂���ł���: " + jsonFilePath);
                }
            }
            else
            {
                Debug.LogWarning("�w�肳�ꂽ�t�H���_�����݂��܂���: " + folderPath);
            }

            // �f�[�^��������Ȃ������ꍇ�A�V����SaveData�I�u�W�F�N�g��Ԃ�
            SaveData saveData = new SaveData(gameTitle);
            return saveData;
        }
    }

    /// <summary>
    /// ���k�@�\�N���X
    /// </summary>
    internal class Compressor
    {
        // ����������k����
        internal static byte[] CompressString(string text)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);

            using (MemoryStream outputStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(inputBytes, 0, inputBytes.Length);
                }
                return outputStream.ToArray();
            }
        }

        // ���k���ꂽ�o�C�g�z����𓀂��ĕ�����ɖ߂�
        internal static string DecompressString(byte[] compressedData)
        {
            using (MemoryStream inputStream = new MemoryStream(compressedData))
            {
                using (GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        gzipStream.CopyTo(outputStream);
                        byte[] outputBytes = outputStream.ToArray();
                        return Encoding.UTF8.GetString(outputBytes);
                    }
                }
            }
        }
    }
}
