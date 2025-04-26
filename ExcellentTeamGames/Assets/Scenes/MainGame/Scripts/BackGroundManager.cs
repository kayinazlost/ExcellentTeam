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
        [SerializeField, Header("10m�����艽�z�u����H")]
        private int density = 3;

        void GetTableSize(out float min, out float max)
        {
            var obj = _table; // �Ώۂ̃I�u�W�F�N�g
            var collider = obj.GetComponent<BoxCollider>(); // 3D�Ȃ�BoxCollider�A2D�Ȃ�BoxCollider2D�ɂ���

            min = 0;
            max = 0;
            if (collider != null)
            {
                // ���[�J���T�C�Y
                Vector3 localSize = collider.size;
                // ���[���h�X�P�[�����|���āA���[���h�T�C�Y��
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
            int spawnCount = Mathf.RoundToInt(distance * (density / 10f)); // �����ɉ����Č����v�Z

            for (int i = 0; i < spawnCount; i++)
            {
                // ���ʂ̃����_��
                float baseX = Random.Range(minX, maxX);

                // �M���b�ƌł܂� or �o������u�����_�����v��ǉ�
                float offset = Random.Range(-1f, 1f);
                offset *= Random.Range(0f, 2f); // ����ɍL����or���܂郉���_������������

                float spawnX = baseX + offset;

                // Y���W��Z���W�͓K���Ɂi��ł̓e�[�u���̍����ƍ��킹��j
                Vector3 spawnPos = new Vector3(spawnX, 0, _table.transform.lossyScale.z / 2);

                var obj = Instantiate(_prefab, spawnPos, Quaternion.identity);
                obj.transform.parent = transform;
            }
        }
    }
}