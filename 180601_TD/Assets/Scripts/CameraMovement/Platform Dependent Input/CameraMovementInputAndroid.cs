using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CameraMovement
{
    public class CameraMovementInputAndroid : ICameraMovementInput
    {

        private Touch _touchZero;
        private Touch _touchOne;

        private Touch[] _touchPoints = new Touch[2];

        private float _minDetectionDistance;

        public CameraMovementInputAndroid(float minDetectionDistance)
        {
            _minDetectionDistance = minDetectionDistance;
        }

        //public void GetInput(ref Vector3 cameraMovementVector, ref float cameraZoomValue)
        //{
        //    throw new System.NotImplementedException();
        //}

        public void GetInput(ref Vector3 cameraMovementVector, ref float cameraZoomValue)
        {
            if (Input.touchCount == 0)
            {
                cameraMovementVector = Vector3.zero;
                cameraZoomValue = 0;
                return;
            }
            

            if (Input.touchCount == 1)
            {
                cameraMovementVector = GetMoveVector();
                return;
            }

            if (Input.touchCount == 2)
            {
                cameraZoomValue = GetZoomValue();
                return;
            }
        }


        private Vector3 GetMoveVector()
        {
            _touchPoints[0] = Input.GetTouch(0);

            return new Vector3(- _touchPoints[0].deltaPosition.x, 0, - _touchPoints[0].deltaPosition.y);

        }

        private float GetZoomValue()
        {
            _touchPoints[0] = Input.GetTouch(0);
            _touchPoints[1] = Input.GetTouch(1);
            
            return (_touchPoints[0].position - _touchPoints[1].position).sqrMagnitude -
            ((_touchPoints[0].position - _touchPoints[0].deltaPosition) - (_touchPoints[1].position - _touchPoints[1].deltaPosition)).sqrMagnitude;
        }
    }
}