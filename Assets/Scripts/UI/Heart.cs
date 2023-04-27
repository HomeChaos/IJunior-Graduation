using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class Heart : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _fullHeart;
        [SerializeField] private Sprite _halfHeart;
        [SerializeField] private Sprite _emptyHeart;

        private HeartState _state = HeartState.Full;

        private void Start()
        {
            ChangeSprite(_state);
        }

        public void ChangeState(HeartState state)
        {
            _state = state;
            ChangeSprite(_state);
        }

        private void ChangeSprite(HeartState state)
        {
            switch (state)
            {
                case HeartState.Full:
                    _image.sprite = _fullHeart;
                    break;
                case HeartState.Half:
                    _image.sprite = _halfHeart;
                    break;
                case HeartState.Empty:
                    _image.sprite = _emptyHeart;
                    break;
            }
        }
    }
}