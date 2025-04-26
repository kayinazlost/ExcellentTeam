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
            // タイマー更新
            _timer += Time.fixedDeltaTime;

            if (_timer >= jumpInterval)
            {
                // 地面にいるか判定
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
            // 足元にレイを飛ばして、地面に当たってるか調べる
            return Physics.Raycast(transform.position, Vector3.down, 0.5f + 0.05f);
        }
    }

}
