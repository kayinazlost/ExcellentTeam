using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiyazakiScript
{
    public class BackGroundManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _table;
        [SerializeField]
        private GameObject _prefab;
        [SerializeField, Header("10mあたり何個配置する？")]
        private int density = 3;

        void GetTableSize(out float min, out float max)
        {
            var obj = _table; // 対象のオブジェクト
            var collider = obj.GetComponent<BoxCollider>(); // 3DならBoxCollider、2DならBoxCollider2Dにする

            min = 0;
            max = 0;
            if (collider != null)
            {
                // ローカルサイズ
                Vector3 localSize = collider.size;
                // ワールドスケールを掛けて、ワールドサイズに
                Vector3 worldSize = Vector3.Scale(localSize, obj.transform.lossyScale);

                float halfWidth = worldSize.x / 2f;
                float centerX = obj.transform.position.x;

                min = centerX - halfWidth;
                max = centerX + halfWidth;
            }
        }

        private void Start()
        {
            GetTableSize(out float minX, out float maxX);

            float distance = maxX - minX;
            int spawnCount = Mathf.RoundToInt(distance * (density / 10f)); // 距離に応じて個数を計算

            for (int i = 0; i < spawnCount; i++)
            {
                // 普通のランダム
                float baseX = Random.Range(minX, maxX);

                // ギュッと固まる or バラける「ランダム性」を追加
                float offset = Random.Range(-1f, 1f);
                offset *= Random.Range(0f, 2f); // さらに広がるor狭まるランダムさをかける

                float spawnX = baseX + offset;

                // Y座標やZ座標は適当に（例ではテーブルの高さと合わせる）
                Vector3 spawnPos = new Vector3(spawnX, 0, _table.transform.lossyScale.z / 2);

                var obj = Instantiate(_prefab, spawnPos, Quaternion.identity);
                obj.transform.parent = transform;
            }
        }
    }
}