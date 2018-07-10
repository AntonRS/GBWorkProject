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

        private CameraMovementModule _cameraMovementModule;
        private ICameraMovementInput _cameraInputModule;
                
        private Vector3 _cameraMovementVector;
        private float _cameraZoomValue;
        

        // Use this for initialization
        void Awake()
        {   
            _cameraMovementModule = new CameraMovementModule(this.transform, _zoomMinCamHeight, _zoomMaxCamHeight);

            //выбираем модуль ввода в зависимости от платформы
#if UNITY_ANDROID
            _cameraInputModule = null;
#else
            _cameraInputModule = new CameraMovementInputWindowsPC(_mouseMoveSideTriggerWidth);
#endif
                       
        }
        
        
        //не забыть выключить, когда мы в главном меню, а не в основной игре
        //проще всего это сделать, задизейблив скрипт при выходе в меню
        void Update()
        {

            _cameraInputModule.GetInput(ref _cameraMovementVector, ref _cameraZoomValue);

            if (_cameraMovementVector != Vector3.zero)
                _cameraMovementModule.MoveCamera(_cameraMovementVector * _cameraMoveSpeed);

            if (_cameraZoomValue != 0)
                _cameraMovementModule.ZoomCamera(_cameraZoomValue * _zoomSpeed);

        }
        

    }
}
