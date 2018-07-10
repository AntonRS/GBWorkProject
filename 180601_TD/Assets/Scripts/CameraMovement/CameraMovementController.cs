using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.CameraMovement;

namespace Game
{
    /// <summary>
    /// Вешается на камеру, обеспечивает движение камеры
    /// </summary>
    public class CameraMovementController : MonoBehaviour
    {

        [Tooltip("Ширина расположенных по краям экрана секторов, при попадании в которых курсора мыши камера двигается в соответствующем направлении")]
        [SerializeField]
        [Range(0.01f, 0.3f)]
        private float _mouseMoveSideTriggerWidth;

        [SerializeField]
        private float _cameraMoveSpeed;

        [SerializeField]
        private float _zoomSpeed;

        [SerializeField]
        private float _zoomMinCamHeight;

        [SerializeField]
        private float _zoomMaxCamHeight;

        private CameraMovementModule _cameraMovement;

        private float _rightBorder;
        private float _leftBorder;
        private float _topBorder;
        private float _bottomBorder;

        private Vector3 _mousePosition;
        private float _horizontalAxisValue;
        private float _verticalAxisValue;
        private float _mouseScrollAxisValue;

        //private const float ScreenSizeChangeCheckDelay = 1f;

        // Use this for initialization
        void Awake()
        {   
            _cameraMovement = new CameraMovementModule(this.transform, _zoomMinCamHeight, _zoomMaxCamHeight);
            SetMouseCamMoveBorderZones();
            //InvokeRepeating(CheckForScreenSizeChange, ScreenSizeChangeCheckDelay, ScreenSizeChangeCheckDelay);
        }



        // Update is called once per frame

        //не забыть выключить, когда мы в главном меню, а не в основной игре
        //проще всего это сделать, задизейблив скрипт при выходе в меню
        void Update()
        {

            _mousePosition = Input.mousePosition;

            _horizontalAxisValue = Input.GetAxisRaw("Horizontal");
            _verticalAxisValue = Input.GetAxisRaw("Vertical");
            _mouseScrollAxisValue = Input.GetAxis("Mouse ScrollWheel");

            if (_mousePosition.x <= _leftBorder || _horizontalAxisValue < 0)
                _cameraMovement.MoveCamera(new Vector3(-_cameraMoveSpeed, 0, 0));

            if (_mousePosition.x >= _rightBorder || _horizontalAxisValue > 0)
                _cameraMovement.MoveCamera(new Vector3(_cameraMoveSpeed, 0, 0));

            if (_mousePosition.y <= _bottomBorder || _verticalAxisValue < 0)
                _cameraMovement.MoveCamera(new Vector3(0, 0, -_cameraMoveSpeed));

            if (_mousePosition.y >= _topBorder || _verticalAxisValue > 0)
                _cameraMovement.MoveCamera(new Vector3(0, 0, _cameraMoveSpeed));

            if (_mouseScrollAxisValue != 0)
                _cameraMovement.ZoomCamera(_zoomSpeed * _mouseScrollAxisValue);

        }

        private void SetMouseCamMoveBorderZones()
        {
            _leftBorder = Screen.width * _mouseMoveSideTriggerWidth;
            _bottomBorder = Screen.height * _mouseMoveSideTriggerWidth;

            _rightBorder = Screen.width - _leftBorder;
            _topBorder = Screen.height - _bottomBorder;
        }

        //private void CheckForScreenSizeChange()
        //{
        //    Debug.Log("проверяется размер экрана");
        //        //добавить переменную для хранения размера экрана и сравнить старое ее значение с текущим
        //}

        //private void InvokeRepeating(Action method, float time, float repeatRate)
        //{
        //    InvokeRepeating(method.Method.Name, time, repeatRate);
        //}

    }
}
