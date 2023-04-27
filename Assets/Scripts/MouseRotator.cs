using UnityEngine;

namespace Scripts
{
    public class MouseRotator : MonoBehaviour
    {
        private const int LeftSide = -1;
        private const int RightSide = 1;
        
        private Vector2 _initialVector;
        private Camera _camera;
        
        protected Vector2 InitialVector => _initialVector;

        protected virtual void Start()
        {
            _camera = Camera.main;
            _initialVector = Vector2.up;
        }

        protected Vector3 GetMousePosition()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }

        protected int GetCurrentSide()
        {
            return GetMousePosition().x >= _initialVector.x ? LeftSide : RightSide;
        }
    }
}