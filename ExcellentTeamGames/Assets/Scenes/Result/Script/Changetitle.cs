using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Changetitle : MonoBehaviour
{
    public void OnChangetitle()
    {
        SceneManager.LoadScene("Title");
    }
}
