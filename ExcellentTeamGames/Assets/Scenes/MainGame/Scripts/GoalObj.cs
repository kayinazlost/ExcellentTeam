using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiyazakiScript
{
    public class GoalObj : MonoBehaviour
    {
        [SerializeField] private GameObject table; // �E�[����肽���I�u�W�F�N�g

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
            if (other.CompareTag("Player"))
            {
                // TODO
                SceneManager.LoadScene("MainGame");
            }
        }
    }
}
