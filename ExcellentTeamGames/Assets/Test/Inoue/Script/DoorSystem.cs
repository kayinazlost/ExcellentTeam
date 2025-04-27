using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    // --- インスタンス保持用のstatic変数 ---
    public static DoorSystem instance;

    // --- 変更対象のフラグ ---
    public bool m_OpenFlag;

    public Animator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        // --- 起動時に自身をstatic変数に代入しておく ---
        instance = this;
        SetOpenFlag(m_OpenFlag);
    }

    // --- static関数でm_OpenFlagを変更できるようにする ---
    public static void SetOpenFlag(bool flag)
    {
        if (instance != null)
        {
            instance.m_OpenFlag = flag;

            instance.m_Animator.SetBool("開く", flag);
        }
    }

}
