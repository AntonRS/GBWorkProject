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

        public CameraMovementModule(Transform cameraHolder, float zoomMinDistance, float zoomMaxDistance)
        {
            _cameraHolder = cameraHolder;
            _zoomMinDistance = zoomMinDistance;
            _zoomMaxDistance = zoomMaxDistance;
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
            _cameraHolder.SetHeight(_cameraHolder.position.y - (moveIncrement * Time.deltaTime));

            if (_cameraHolder.position.y > _zoomMaxDistance)
                _cameraHolder.SetHeight(_zoomMaxDistance);
            else if (_cameraHolder.position.y < _zoomMinDistance)
                _cameraHolder.SetHeight(_zoomMinDistance);
        }
    }

}

