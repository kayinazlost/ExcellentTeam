using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCamera : MonoBehaviour
{
    [Header("�v���C���[A")]
    public Transform m_PlayerA;
    [Header("�v���C���[B")]
    public Transform m_PlayerB;

    void LateUpdate()
    {
        transform.position = (m_PlayerA.position + m_PlayerB.position) * 0.5f;
    }
}
