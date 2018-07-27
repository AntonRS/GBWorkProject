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

        [Tooltip("Ширина расположенных по краям экрана секторов," +
                " при попадании в которых курсора мыши камера двигается в соответствующем направлении")]
        [SerializeField]
        [Range(0.01f, 0.3f)]
        private float _mouseMoveSideTriggerWidthPC;

        [SerializeField]
        private float _cameraMoveSpeedAndroid;

        [SerializeField]
        private float _zoomSpeedAndroid;

        [SerializeField]
        private float _cameraMoveSpeedPC;

        [SerializeField]
        private float _zoomSpeedPC;

        [SerializeField]
        private float _zoomMinCamHeight;

        [SerializeField]
        private float _zoomMaxCamHeight;

        [Header("Отступы для камеры: от краёв карты и от центра")]
        [SerializeField]
        private float _mapBordersSouthPadding;

        [SerializeField]
        private float _mapBordersNorthPadding;

        [SerializeField]
        private float _mapBordersSidePadding;
        
        [Tooltip("Отступ от центра по оси Z для установки начального положения камеры")]
        [SerializeField]
        private float _cameraDefaultPositionOffsetZ;

        private float _cameraMoveSpeed;
        private float _zoomSpeed;

        private CameraMovementModule _cameraMovementModule;
        private ICameraMovementInput _cameraInputModule;
                
        private Vector3 _cameraMovementVector;
        private float _cameraZoomValue;

        private TerrainGeneratorController _mapData;

        bool _moveCameraToDefaultPosition;

        // Use this for initialization
        void Awake()
        {   
            _cameraMovementModule = new CameraMovementModule(this.transform, _zoomMinCamHeight, _zoomMaxCamHeight);

            //выбираем модуль ввода в зависимости от платформы
#if UNITY_ANDROID
                        _cameraInputModule = new CameraMovementInputAndroid(0f);
                         _cameraMoveSpeed = _cameraMoveSpeedAndroid;
                        _zoomSpeed = _zoomSpeedAndroid;
#else
            _cameraInputModule = new CameraMovementInputWindowsPC(_mouseMoveSideTriggerWidthPC);
            _cameraMoveSpeed = _cameraMoveSpeedPC;
            _zoomSpeed = _zoomSpeedPC;
#endif

            _mapData = FindObjectOfType<TerrainGeneratorController>();            
        }


        private void SetMapCenterAndBounds()
        {
            _cameraMovementModule.SetCamDefaultPositionAndBounds(_mapData.MapSouthernEdgeZ - _mapBordersSouthPadding, _mapData.MapWesternEdgeX - _mapBordersSidePadding, _mapData.MapNorthernEdgeZ + _mapBordersNorthPadding, _mapData.MapEasternEdgeX + _mapBordersSidePadding, _cameraDefaultPositionOffsetZ);
            _moveCameraToDefaultPosition = true;
        }

        //не забыть выключить, когда мы в главном меню, а не в основной игре
        //проще всего это сделать, задизейблив скрипт при выходе в меню
        void Update()
        {
            _cameraInputModule.GetInput(ref _cameraMovementVector, ref _cameraZoomValue);

            if (_cameraZoomValue != 0)
                _cameraMovementModule.ZoomCamera(_cameraZoomValue * _zoomSpeed);

            //ДОБАВИТЬ КНОПКУ НА ЭКРАН - андроид без неё этой функции иметь не будет
#if UNITY_EDITOR
            if (Input.GetButtonDown("Center Camera"))
                _moveCameraToDefaultPosition = true;
#elif UNITY_STANDALONE
            if (Input.GetButtonDown("Center Camera"))
                _moveCameraToCenter = true;
#endif

            if (_moveCameraToDefaultPosition == true)
            {
                _cameraMovementModule.SetCameraToDefaultPosition();
                _moveCameraToDefaultPosition = false;
            }

            else
            {
                if (_cameraMovementVector != Vector3.zero)
                    _cameraMovementModule.MoveCamera(_cameraMovementVector * _cameraMoveSpeed);
            }
        }

        /// <summary>
        /// Устанавливает камеру в стартовую позицию. Вызывать после того как карта была сгенерирована.
        /// </summary>
        public void SetCameraToDefaultPosition()
        {
            SetMapCenterAndBounds();
        }

    }
}
