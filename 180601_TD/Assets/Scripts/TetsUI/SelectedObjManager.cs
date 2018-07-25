using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.TestUI
{
    public class SelectedObjManager : MonoBehaviour
    {
        private GameObject _selectedObj = null;
        public GameObject SelectedObj
        {
            get
            { return _selectedObj; }
            set
            {
                if (value == _selectedObj)
                {
                    _selectedObj = null;
                }
                else
                {
                    _selectedObj = value;
                    OnSelectedObjectChanged.Invoke(_selectedObj);
                }
            }
        }
        
        public delegate void SelectedObjectChanged(GameObject obj);
        public event SelectedObjectChanged OnSelectedObjectChanged;


    }
}

