using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Extensions
{
    public static class Extensions
    {
        public static void SetHeight(this Transform transform, float value)
        {
            transform.position = new Vector3(transform.position.x, value, transform.position.z);
        }
    }

}