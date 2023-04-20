using System;
using UnityEngine;
using DG.Tweening;

namespace Scripts.Components
{
    public class FollowToTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _delay;
        [SerializeField] private float _offset = -10f;

        private Vector3 _targetLastPosition;
        private Tweener _tweener;

        private void Start()
        {
            var targetPos = GetTargetPositionWithOffset(_target.position, _offset);
            _tweener = transform.DOMove(targetPos, _delay).SetAutoKill(false);
            _targetLastPosition = targetPos;
        }

        private void LateUpdate()
        {
            var targetPos = GetTargetPositionWithOffset(_target.position, _offset);
            
            if (_targetLastPosition.x != _target.position.x || _targetLastPosition.y != _target.position.y)
            {
                _tweener.ChangeEndValue(targetPos, true).Restart();
                _targetLastPosition = targetPos;
            }
        }

        private void OnDestroy()
        {
            _tweener.Kill();
        }

        private Vector3 GetTargetPositionWithOffset(Vector3 target, float offset)
        {
            return new Vector3(target.x, target.y, offset);
        }
    }
}