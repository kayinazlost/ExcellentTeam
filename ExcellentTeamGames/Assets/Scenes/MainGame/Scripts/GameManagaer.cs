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
        StartCoroutine(ReturnToTitle());
    }

    private IEnumerator ReturnToTitle()
    {
        yield return new WaitForSeconds(_gameOver_GoTitleTime);
        SceneManager.LoadScene("Title");
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
