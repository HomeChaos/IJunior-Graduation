using System;
using System.Collections;
using Scripts.PlayerScripts;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textTimer;
        [SerializeField] private Player _player;

        private Coroutine _coroutine;
        private int _time;

        private void OnEnable()
        {
            _player.OnDying += StopTimer;
        }

        private void OnDisable()
        {
            _player.OnDying -= StopTimer;
        }

        private void Start()
        {
            _textTimer.text = $"00:00";
            StartTimer();
        }

        private void StartTimer()
        {
            _coroutine = StartCoroutine(StartCountdown());
        }

        private void StopTimer()
        {
            StopCoroutine(_coroutine);
        }

        private IEnumerator StartCountdown()
        {
            bool isPlaying = true;
            var waitForSeconds = new WaitForSeconds(1f);

            while (isPlaying)
            {
                yield return waitForSeconds;
                _time += 1;
                _textTimer.text = $"{_time / 60:00}:{_time % 60:00}";
            }
        }
    }
}