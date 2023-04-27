using UnityEngine;
using UnityEngine.Events;

namespace Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class Curtain : MonoBehaviour
    {
        private readonly int _showKey = Animator.StringToHash("Show");
        
        public event UnityAction AnimationOver;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void ShowCurtain()
        {
            _animator.SetTrigger(_showKey);
        }

        private void EndOfAnimation()
        {
            AnimationOver?.Invoke();
        }
    }
}