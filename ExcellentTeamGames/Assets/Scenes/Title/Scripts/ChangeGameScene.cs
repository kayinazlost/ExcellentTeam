using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class ChangeGameScene : MonoBehaviour
{
    public float m_Times = 1.5f;
    public bool m_Flag = false;
    /// <summary>
    /// TitleÇ©ÇÁMainGameÇ…à⁄çs
    /// </summary>
    public void OnStartButton()
    {
        if (!m_Flag)
        {
            DoorSystem.SetOpenFlag(false);
            m_Flag = true;
        }
    }
    private void Update()
    {
        if (m_Flag)
        {
            if (m_Times <= 0.0f)
            {
                SceneManager.LoadScene("MainGame");
                Debug.Log("êÿÇËë÷Ç¶Ç‹ÇµÇΩ!");
            }
            else
                m_Times -= Time.deltaTime;
        }
    }

}
