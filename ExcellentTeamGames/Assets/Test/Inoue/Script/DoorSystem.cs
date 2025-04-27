using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    // --- �C���X�^���X�ێ��p��static�ϐ� ---
    public static DoorSystem instance;

    // --- �ύX�Ώۂ̃t���O ---
    public bool m_OpenFlag;

    public Animator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        // --- �N�����Ɏ��g��static�ϐ��ɑ�����Ă��� ---
        instance = this;
        SetOpenFlag(m_OpenFlag);
    }

    // --- static�֐���m_OpenFlag��ύX�ł���悤�ɂ��� ---
    public static void SetOpenFlag(bool flag)
    {
        if (instance != null)
        {
            instance.m_OpenFlag = flag;

            instance.m_Animator.SetBool("�J��", flag);
        }
    }

}
