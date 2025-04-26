using Mandegan;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiyazakiScript
{
    public class GoalObj : MonoBehaviour
    {
        [SerializeField] private GameObject table; // 右端を取りたいオブジェクト

        private bool isGoal = false;

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
            if (isGoal) return;
            if (other.CompareTag("Player"))
            {
                isGoal = true;
                // TODO
                GameRecordManager.Save();
                SceneManager.LoadScene("Title");
            }
        }
    }
}
