using Mandegan;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiyazakiScript
{
    public class GoalObj : MonoBehaviour
    {
        [SerializeField] private GameObject table; // 右端を取りたいオブジェクト
        [SerializeField] private GameObject goaltext;
        [SerializeField] private GameObject effect;

        private bool isGoal = false;


        public float m_GoResultTimes = 1.5f;
        public float m_Times = 1.5f;

        private void Awake()
        {
            goaltext.SetActive(false);
        }

        void Start()
        {
            var collider = table.GetComponent<Collider>();
            // 右端のX座標を取得
            float rightEdgeX = collider.bounds.max.x;

            // 自分のポジションを更新
            Vector3 myPos = transform.position;
            myPos.x = rightEdgeX;
            transform.position = myPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (GameManager.Instance.IsRun == false)
                return;

            if (isGoal) return;

            GameManager.Instance.isGameClear = true;

            if (other.CompareTag("Player"))
            {
                isGoal = true;
                goaltext.SetActive(true);
                effect.SetActive(true);
                // TODO
                GameRecordManager.Save();
            }
        }
        private void Update()
        {
            if (isGoal)
            {
                if(m_GoResultTimes > 0)
                {
                    m_GoResultTimes -= Time.deltaTime;
                    if (m_GoResultTimes <= 0)
                    {
                        DoorSystem.SetOpenFlag(false);
                    }
                    return;
                }
                if (m_Times <= 0.0f)
                {
                    SceneManager.LoadScene("Result");
                    Debug.Log("切り替えました!");
                }
                else
                    m_Times -= Time.deltaTime;
            }
        }

    }
}
