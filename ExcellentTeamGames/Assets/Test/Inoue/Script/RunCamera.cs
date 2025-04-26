using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCamera : MonoBehaviour
{
    [Header("プレイヤーA")]
    public Transform m_PlayerA;
    [Header("プレイヤーB")]
    public Transform m_PlayerB;

    void LateUpdate()
    {
        transform.position = (m_PlayerA.position + m_PlayerB.position) * 0.5f;
    }
}
