using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class Curtain : MonoBehaviour
    {
        private readonly int ShowKey = Animator.StringToHash("Show");
        
        public event UnityAction AnimationOver;

        private Animator _animator;

        public void ShowCurtain()
        {
            _animator.SetTrigger(ShowKey);
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void EndOfAnimation()
        {
            AnimationOver?.Invoke();
        }
    }
}