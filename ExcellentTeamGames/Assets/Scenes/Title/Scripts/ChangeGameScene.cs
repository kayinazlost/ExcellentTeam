using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeGameScene : MonoBehaviour
{
    /// <summary>
    /// Title����MainGame�Ɉڍs
    /// </summary>
    public void OnStartButton()
    {
        SceneManager.LoadScene("MainGame");
        Debug.Log("�؂�ւ��܂���!");
    }
}
