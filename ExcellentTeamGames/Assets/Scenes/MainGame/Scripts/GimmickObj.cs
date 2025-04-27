using UnityEngine;
using System.Collections.Generic;

public class GimmickObj : MonoBehaviour
{
    [System.Serializable]
    public class GimmickEntry
    {
        public GameObject prefab;
        public GimmickType type;
    }

    public enum GimmickType
    {
        Fixed,
        Random
    }

    [SerializeField] private List<GimmickEntry> gimmickList = new List<GimmickEntry>();

    private GameObject spawnedObj;
    private GimmickType selectedType;

    public void SetupPosition(float baseX, Transform table)
    {
        if (gimmickList.Count == 0 || table == null) return;

        var entry = gimmickList[Random.Range(0, gimmickList.Count)];
        spawnedObj = Instantiate(entry.prefab, transform);
        selectedType = entry.type;

        Vector3 spawnPos = new Vector3(baseX, GetOffsetY(spawnedObj, table) * -1f, table.position.z);

        if (selectedType == GimmickType.Random)
        {
            var tableCollider = table.GetComponent<Collider>();
            if (tableCollider != null)
            {
                float zRange = tableCollider.bounds.size.z / 2f;
                float randomZ = Random.Range(-zRange, zRange);
                float randomY = Random.Range(0.5f, 2f); // çDÇ´Ç»Yé≤è„è∏ïù

                spawnPos.z += randomZ;
                spawnPos.y += randomY;
            }
        }

        spawnedObj.transform.position = spawnPos;
    }

    private float GetOffsetY(GameObject obj, Transform table)
    {
        var collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds.min.y;
        }
        return 0f;
    }

}
