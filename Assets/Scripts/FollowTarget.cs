using System;
using UnityEngine;
using DG.Tweening;

namespace Scripts
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _time;
        [SerializeField] private float _offset;

        private Vector3 _targetLastPostion;
        private Tweener _tweener;

        private void Start()
        {
            var targetPos = GetTargetPositionWithOffset(_target.position, _offset);
            _tweener = transform.DOMove(targetPos, _time).SetAutoKill(false);
            _targetLastPostion = targetPos;
        }

        private void LateUpdate()
        {
            var targetPos = GetTargetPositionWithOffset(_target.position, _offset);
            
            if (_targetLastPostion.x != _target.position.x || _targetLastPostion.y != _target.position.y)
            {
                _tweener.ChangeEndValue(targetPos, true).Restart();
                _targetLastPostion = targetPos;
            }
        }

        private Vector3 GetTargetPositionWithOffset(Vector3 target, float offset)
        {
            return new Vector3(target.x, target.y, offset);
        }
    }
}