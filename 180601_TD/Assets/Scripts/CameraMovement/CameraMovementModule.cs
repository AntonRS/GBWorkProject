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
    }

}

