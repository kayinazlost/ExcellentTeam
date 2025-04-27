using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _players = new List<GameObject>();
    [SerializeField]
    private GameObject _bodyObj;
    [SerializeField] private UI_PlayerTabSpawner tabSpawner;
    [SerializeField] private GameObject goalObject;

    private void Start()
    {
        tabSpawner.SpawnTabs(_players.Count);
    }

    private void FixedUpdate()
    {
        if (goalObject == null) return;

        float goalX = goalObject.transform.position.x;

        List<float> remainingDistances = new List<float>();

        foreach (var player in _players)
        {
            if (player != null)
            {
                float distance = Mathf.Max(0f, goalX - player.transform.position.x);
                remainingDistances.Add(distance);
            }
            else
            {
                remainingDistances.Add(0f);
            }
        }

        tabSpawner.UpdatePlayerPositions(remainingDistances);
    }
}
