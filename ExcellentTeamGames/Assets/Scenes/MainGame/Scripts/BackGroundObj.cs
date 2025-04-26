using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiyazakiScript
{
    public class BackGroundObj : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] _sprites;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
        }
    }
}