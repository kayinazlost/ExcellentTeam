using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private SoundDatabase soundDatabase;
    [SerializeField] private GameObject prefab;

    private AudioSource bgmSource;
    private AudioSource[] seSources;
    private int seSourceIndex = 0;

    public int SeSourcePoolSize = 5;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeOnLoad()
    {
        var prefab = Resources.Load<GameObject>("SoundManager");

        var obj = Instantiate(prefab);
        obj.name = "SoundManager";
        DontDestroyOnLoad(obj);

        Instance = obj.GetComponent<SoundManager>();
    }

    private void Start()
    {
        InitializeSeSources();
        PlayBgmBySceneName();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void InitializeSeSources()
    {
        seSources = new AudioSource[SeSourcePoolSize];
        for (int i = 0; i < SeSourcePoolSize; i++)
        {
            var obj = Instantiate(prefab, transform);
            obj.name = "SE_" + i.ToString();
            obj.hideFlags = HideFlags.HideAndDontSave;
            var source = obj.GetComponent<AudioSource>();
            seSources[i] = source;

        }

        {
            var obj = Instantiate(prefab, transform);
            obj.name = "BGM";
            obj.hideFlags = HideFlags.HideAndDontSave;
            var source = obj.GetComponent<AudioSource>();
            source.loop = true;
            bgmSource = source;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBgm(scene.name);
    }

    private void PlayBgmBySceneName()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        PlayBgm(sceneName);
    }

    /// <summary>
    /// BGMÇçƒê∂Ç∑ÇÈ
    /// </summary>
    /// <param name="name"></param>
    public void PlayBgm(string name)
    {
        AudioClip clip = soundDatabase.GetClip(name);
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    /// <summary>
    /// SEÇçƒê∂Ç∑ÇÈ
    /// </summary>
    /// <param name="name"></param>
    public void PlaySe(string name)
    {
        AudioClip clip = soundDatabase.GetClip(name);
        if (clip != null)
        {
            seSources[seSourceIndex].PlayOneShot(clip);
            seSourceIndex = (seSourceIndex + 1) % SeSourcePoolSize; // éüÇ…égÇ§AudioSourceÇÃî‘çÜ
        }
    }
}
