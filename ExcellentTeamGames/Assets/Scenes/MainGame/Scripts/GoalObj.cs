using Mandegan;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiyazakiScript
{
    public class GoalObj : MonoBehaviour
    {
        [SerializeField] private GameObject table; // �E�[����肽���I�u�W�F�N�g
        [SerializeField] private GameObject goaltext;

        private bool isGoal = false;

        public float m_Times = 1.5f;

        private void Awake()
        {
            goaltext.SetActive(false);
        }

        void Start()
        {
            var collider = table.GetComponent<Collider>();
            // �E�[��X���W���擾
            float rightEdgeX = collider.bounds.max.x;

            // �����̃|�W�V�������X�V
            Vector3 myPos = transform.position;
            myPos.x = rightEdgeX;
            transform.position = myPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (GameManager.Instance.IsRun == false)
                return;

            if (isGoal) return;

            GameManager.Instance.StopTimer();
            DoorSystem.SetOpenFlag(false);

            if (other.CompareTag("Player"))
            {
                isGoal = true;
                goaltext.SetActive(true);
                // TODO
                GameRecordManager.Save();
            }
        }
        private void Update()
        {
            if (isGoal)
            {
                if (m_Times <= 0.0f)
                {
                    SceneManager.LoadScene("Result");
                    Debug.Log("�؂�ւ��܂���!");
                }
                else
                    m_Times -= Time.deltaTime;
            }
        }

    }
}
