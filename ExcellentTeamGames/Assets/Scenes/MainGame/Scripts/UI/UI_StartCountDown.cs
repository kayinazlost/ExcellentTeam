using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StartCountDown : MonoBehaviour
{
    public void GameStart()
    {
        GameManager.Instance.GameStart();
    }
}
