using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiyazakiScript
{
    public class AutoMove : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private float jumpForce = 5f;
        [SerializeField]
        private float jumpInterval = 2f;

        private Rigidbody _rigidbody;
        private float _timer;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            var vel = _rigidbody.velocity;
            vel.x = speed;
            _rigidbody.velocity = vel;
            // �^�C�}�[�X�V
            _timer += Time.fixedDeltaTime;

            if (_timer >= jumpInterval)
            {
                // �n�ʂɂ��邩����
                if (IsGrounded())
                {
                    Jump();
                }
                _timer = 0f;
            }
        }

        private void Jump()
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private bool IsGrounded()
        {
            // �����Ƀ��C���΂��āA�n�ʂɓ������Ă邩���ׂ�
            return Physics.Raycast(transform.position, Vector3.down, 0.5f + 0.05f);
        }
    }

}
