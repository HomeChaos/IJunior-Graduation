using UnityEngine;
using DG.Tweening;

namespace Scripts.Components
{
    public class FollowToTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _offset = -10f;

        private Vector3 _targetPosition;
        private Vector3 _targetLastPosition;
        private Tweener _tweener;

        private void Start()
        {
            _targetPosition = GetTargetPositionWithOffset(_target.position, _offset);
            _tweener = transform.DOMove(_targetPosition, _duration).SetAutoKill(false);
            _targetLastPosition = _targetPosition;
        }

        private void LateUpdate()
        {
            _targetPosition = GetTargetPositionWithOffset(_target.position, _offset);
            
            if ((_targetLastPosition.x != _target.position.x) || (_targetLastPosition.y != _target.position.y))
            {
                _tweener.ChangeEndValue(_targetPosition, true).Restart();
                _targetLastPosition = _targetPosition;
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