using System.Collections.Generic;
using UnityEngine;

public class GimmickManager : MonoBehaviour
{
    public static GimmickManager Instance { get; private set; }

    [SerializeField] private GameObject gimmickObjPrefab;
    [SerializeField] private Transform tableTransform;
    [SerializeField] private float minInterval = 1f;
    [SerializeField] private float maxInterval = 3f;

    [System.Serializable]
    public class ForbiddenZone
    {
        public float minX;
        public float maxX;
    }

    [SerializeField] private List<ForbiddenZone> forbiddenZones = new List<ForbiddenZone>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        GenerateGimmicks();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void GenerateGimmicks()
    {
        if (gimmickObjPrefab == null || tableTransform == null) return;

        var tableCollider = tableTransform.GetComponent<Collider>();
        if (tableCollider == null) return;

        float startX = tableCollider.bounds.min.x;
        float endX = tableCollider.bounds.max.x;
        float currentX = startX;

        while (currentX < endX)
        {
            if (IsForbidden(currentX))
            {
                // 禁止区間だったらスキップして次の位置へ進む
                currentX += Random.Range(minInterval, maxInterval);
                continue;
            }

            var obj = Instantiate(gimmickObjPrefab, transform);
            var gimmick = obj.GetComponent<GimmickObj>();

            if (gimmick != null)
            {
                gimmick.SetupPosition(currentX, tableTransform);
            }

            currentX += Random.Range((int)minInterval, (int)maxInterval);
        }
    }

    private bool IsForbidden(float x)
    {
        foreach (var zone in forbiddenZones)
        {
            if (x >= zone.minX && x <= zone.maxX)
            {
                return true;
            }
        }
        return false;
    }
}
