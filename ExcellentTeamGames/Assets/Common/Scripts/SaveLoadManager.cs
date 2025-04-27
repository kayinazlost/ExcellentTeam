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
        // プレイ履歴（SaveData.ScoreListType.History）の0番目を取ってくるよ
        var data = saveData.GetData<GameRecordData>(SaveData.ScoreListType.History, 0);
        if (data == null)
        {
            // セーブデータを新規作成
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
        /// スコアを追加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scoreData">任意のデータ（数値・クラスなど、なんでも）</param>
        /// <param name="rankScore">ランキングに登録するためのスコア（トータル値など）</param>
        public void AddData<T>(T scoreData, int rankScore)
        {
            // データをJSONにシリアライズ
            string jsonData = JsonUtility.ToJson(scoreData, true);
            // 新しいスコアデータを作成
            var data = new ScoreData();
            data.jsonData = jsonData;
            data.day = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            // scoreHistory に追加
            if (scoreHistory == null)
                scoreHistory = new List<ScoreData>();
            scoreHistory.Insert(0, data);
            while (scoreHistory.Count > 30)
                scoreHistory.RemoveAt(scoreHistory.Count - 1);

            // scoreRanking にランキングとして追加
            if (scoreRanking == null)
                scoreRanking = new List<ScoreData>();
            scoreRanking.Add(data);

            // スコアでソートして上位30個だけを保持
            scoreRanking = scoreRanking.OrderByDescending(x => x.rankScore).Take(30).ToList();
        }
        public T GetData<T>(ScoreListType listType, int num)
        {
            try
            {
                // JSONデータを指定した型にデシリアライズ
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
            // ゲームのセーブデータを読み込む
            SaveData saveData = LoadSaveData(gameTitle);

            // 読み込んだデータを表示
            Debug.Log($"GameTitle: {saveData.GameTitle}, History: {saveData.scoreHistory}");

            return saveData;
        }

        private static bool DataSave(SaveData data)
        {
            if (data.GameTitle == "")
            {
                Debug.LogError("セーブデータの情報が不足しています");
                return false;
            }

            // ファイルパスを構築
            string folderPath = Path.Combine(Application.persistentDataPath, "MandeganSaveData");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // JSONにシリアライズして暗号化・圧縮をする
            var jsonData = Compressor.CompressString(JsonUtility.ToJson(data));

            var filePath = Path.Combine(folderPath, $"{data.GameTitle}.savedata");

            // JSONをファイルに保存
            File.WriteAllBytes(filePath, jsonData);

            Debug.Log("データが保存されました: " + filePath);

            return true;
        }

        private static SaveData LoadSaveData(string gameTitle)
        {
            // フォルダパスを構築
            string folderPath = Path.Combine(Application.persistentDataPath, "MandeganSaveData");

            if (Directory.Exists(folderPath))
            {
                // gameTitleと一致する名前のjsonファイルのパスを構築
                string jsonFilePath = Path.Combine(folderPath, $"{gameTitle}.savedata");

                if (File.Exists(jsonFilePath))
                {
                    // ファイルからJSON文字列を読み込んで解読する
                    string jsonData = Compressor.DecompressString(File.ReadAllBytes(jsonFilePath));

                    // JSON文字列をSaveDataオブジェクトにデシリアライズ
                    var load = JsonUtility.FromJson<SaveData>(jsonData);
                    var data = new SaveData(gameTitle);
                    data.scoreHistory = load.scoreHistory;
                    data.scoreRanking = load.scoreRanking;

                    // デシリアライズしたデータを返す
                    return data;
                }
                else
                {
                    Debug.LogWarning("一致するJSONファイルが見つかりませんでした: " + jsonFilePath);
                }
            }
            else
            {
                Debug.LogWarning("指定されたフォルダが存在しません: " + folderPath);
            }

            // データが見つからなかった場合、新しいSaveDataオブジェクトを返す
            SaveData saveData = new SaveData(gameTitle);
            return saveData;
        }
    }

    /// <summary>
    /// 圧縮機能クラス
    /// </summary>
    internal class Compressor
    {
        // 文字列を圧縮する
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

        // 圧縮されたバイト配列を解凍して文字列に戻す
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
