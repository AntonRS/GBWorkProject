using UnityEngine;

namespace Game.CameraMovement
{

    public class CameraMovementInputWindowsPC : ICameraMovementInput
    {
        private Vector3 _mousePosition;
        private float _horizontalAxisValue;
        private float _verticalAxisValue;
        private float _mouseScrollAxisValue;

        private float _rightBorder;
        private float _leftBorder;
        private float _topBorder;
        private float _bottomBorder;

        public CameraMovementInputWindowsPC(float mouseMoveSideTriggerWidth)
        {
            _leftBorder = Screen.width * mouseMoveSideTriggerWidth;
            _bottomBorder = Screen.height * mouseMoveSideTriggerWidth;

            _rightBorder = Screen.width - _leftBorder;
            _topBorder = Screen.height - _bottomBorder;
        }

        public void GetInput(ref Vector3 cameraMovementVector, ref float cameraZoomValue)
        {
            //движение камеры
            cameraMovementVector = Vector3.zero;

            _mousePosition = Input.mousePosition;

            _horizontalAxisValue = Input.GetAxisRaw("Horizontal");
            _verticalAxisValue = Input.GetAxisRaw("Vertical");

            if (_mousePosition.x <= _leftBorder || _horizontalAxisValue < 0)
                cameraMovementVector += new Vector3(-1, 0, 0);

            if (_mousePosition.x >= _rightBorder || _horizontalAxisValue > 0)
                cameraMovementVector += new Vector3(1, 0, 0);

            if (_mousePosition.y <= _bottomBorder || _verticalAxisValue < 0)
                cameraMovementVector += new Vector3(0, 0, -1);

            if (_mousePosition.y >= _topBorder || _verticalAxisValue > 0)
                cameraMovementVector += new Vector3(0, 0, 1);

            //зум
            cameraZoomValue = Input.GetAxis("Mouse ScrollWheel");
        }
    }
}