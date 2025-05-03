using UnityEngine;
using Mandegan;

public class WebGLSaveHolder : MonoBehaviour
{
    public static WebGLSaveHolder Instance;

    public SaveData CachedSaveData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
