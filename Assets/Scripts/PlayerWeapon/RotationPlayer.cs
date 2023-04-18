﻿using UnityEngine;

namespace Scripts.PlayerWeapon
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RotationPlayer : RotationObject
    {
        private SpriteRenderer _spriteRenderer;

        protected override void Start()
        {
            base.Start();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            UpdateSpriteRender();
        }
        
        private void UpdateSpriteRender()
        {
            var mousePosition = GetMousePosition();
            
            if (mousePosition.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (mousePosition.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }
    }
}