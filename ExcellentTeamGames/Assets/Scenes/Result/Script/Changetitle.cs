using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Changetitle : MonoBehaviour
{
    public float m_Times = 1.5f;
    public bool m_Flag = false;

    public void OnChangetitle()
    {
        return;
        //SceneManager.LoadScene("Title");
        if (!m_Flag)
        {
            DoorSystem.SetOpenFlag(false);
            m_Flag = true;
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!m_Flag)
            {
                DoorSystem.SetOpenFlag(false);
                m_Flag = true;
            }
        }
    }
    private void LateUpdate()
    {
        if (m_Flag)
        {
            if (m_Times <= 0.0f)
            {
                SceneManager.LoadScene("Title");
                Debug.Log("Ø‚è‘Ö‚¦‚Ü‚µ‚½!");
            }
            else
                m_Times -= Time.deltaTime;
        }
    }
}
