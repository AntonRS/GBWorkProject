using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Extensions;

namespace Game.CameraMovement
{

    public class CameraMovementModule
    {
        private Transform _cameraHolder;
        private float _zoomMaxDistance;
        private float _zoomMinDistance;

        private Camera _camera;

        private Vector3 _defaultPosition;

        private float _westBorder;
        private float _eastBorder;
        private float _southBorder;
        private float _northBorder;

        public CameraMovementModule(Transform cameraHolder, float zoomMinDistance, float zoomMaxDistance)
        {
            _cameraHolder = cameraHolder;
            _zoomMinDistance = zoomMinDistance;
            _zoomMaxDistance = zoomMaxDistance;

            _camera = _cameraHolder.GetComponent<Camera>();
        }

        /// <summary>
        /// Сдвигает камеру по подаваемому на вход вектору
        /// </summary>
        /// <param name="moveVector">Направление и скорость смещения камеры</param>
        public void MoveCamera(Vector3 moveVector)
        {
            _cameraHolder.position += moveVector * Time.deltaTime;

            if (_cameraHolder.position.x < _westBorder)
                _cameraHolder.SetX(_westBorder);

            if (_cameraHolder.position.x > _eastBorder)
                _cameraHolder.SetX(_eastBorder);

            if (_cameraHolder.position.z < _southBorder)
                _cameraHolder.SetZ(_southBorder);

            if (_cameraHolder.position.z > _northBorder)
                _cameraHolder.SetZ(_northBorder);

        }

        /// <summary>
        /// Придвигает или отодвигает камеру.
        /// </summary>
        /// <param name="moveIncrement">Скорость смещения камеры, положительная - zoom in, отрицательная - zoom out</param>
        public void ZoomCamera(float moveIncrement)
        {
            _camera.fieldOfView -= moveIncrement * Time.deltaTime;
            if (_camera.fieldOfView > _zoomMaxDistance)
                _camera.fieldOfView = _zoomMaxDistance;
            if (_camera.fieldOfView < _zoomMinDistance)
                _camera.fieldOfView = _zoomMinDistance;
        }

        public void SetCameraToDefaultPosition()
        {
            _cameraHolder.SetX(_defaultPosition.x);
            _cameraHolder.SetZ(_defaultPosition.z);
        }

        public void SetCamDefaultPositionAndBounds(float southBorder, float westBorder, float northBorder, float eastBorder, float centerOffsetZ)
        {
            _defaultPosition = new Vector3 ((westBorder + eastBorder) / 2, 0, ((southBorder + northBorder) / 2) + centerOffsetZ);
            _westBorder = westBorder;
            _southBorder = southBorder;
            _northBorder = northBorder;
            _eastBorder = eastBorder;
        }
    }

}

