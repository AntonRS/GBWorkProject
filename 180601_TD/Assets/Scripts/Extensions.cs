using System;
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
        
        public static T[] RandomizeArray<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                var temp = array[j];
                array[j] = array[i];
                array[i] = temp;
            }

            return array;
        }

    }
}