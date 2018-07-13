using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CameraMovement
{
    public interface ICameraMovementInput
    {

        void GetInput(ref Vector3 cameraMovementVector, ref float cameraZoomValue);

    }
}
