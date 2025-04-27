using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] 
    private float maxTime = 30f;
    [SerializeField]
    private float _gameOver_GoTitleTime = 3f;
    [SerializeField]
    private Animator _gameOverAnim;

    public float MaxTime => maxTime;

    public bool IsRun { get => isGameRunning; }

    public float elapsedTime = 0f;
    private bool isGameRunning = true;
    public float PlayTime = 0f;

    public float m_Times = 1.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (isGameRunning)
        {
            elapsedTime += Time.deltaTime;
            PlayTime += Time.deltaTime;
        }
    }

    public void Kaihuku()
    {
        elapsedTime -= 10f;
        if (elapsedTime < 0f)
        {
            elapsedTime = 0;
        }

    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void StopTimer()
    {
        isGameRunning = false;
    }

    public void GameOver()
    {
        if (!isGameRunning) return;

        Debug.Log("Game Over!");
        isGameRunning = false;
        _gameOverAnim.SetTrigger("In");
        //StartCoroutine(ReturnToTitle());
        Invoke("End",5f);

    }
    private void End()
    {
        DoorSystem.SetOpenFlag(false);
    }
    private void LateUpdate()
    {
        if (!isGameRunning)
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
    /*
    private IEnumerator ReturnToTitle()
    {
        yield return new WaitForSeconds(_gameOver_GoTitleTime);
        SceneManager.LoadScene("Title");
    }
    */
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
