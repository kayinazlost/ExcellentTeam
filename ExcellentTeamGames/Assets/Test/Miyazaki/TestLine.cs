using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float time = 0;
    [SerializeField]
    private float speed = 0;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 60;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(0, 0f, 0.1f * i));
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) {
            time += Time.deltaTime * speed;
        }
        var y = 0f;
        for (int i = 0; i < lineRenderer.positionCount / 2; i++)
        {
            y = Mathf.Cos(0.1f * i + time);
            lineRenderer.SetPosition(i, new Vector3(0, y, 0.1f * i));
        }
        for (int i = lineRenderer.positionCount / 2; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(0, y, 0.1f * i));
        }
    }
}
