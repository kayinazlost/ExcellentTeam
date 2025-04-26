using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeGameScene : MonoBehaviour
{
    /// <summary>
    /// Title‚©‚çMainGame‚ÉˆÚs
    /// </summary>
    public void OnStartButton()
    {
        SceneManager.LoadScene("MainGame");
        Debug.Log("Ø‚è‘Ö‚¦‚Ü‚µ‚½!");
    }
}
