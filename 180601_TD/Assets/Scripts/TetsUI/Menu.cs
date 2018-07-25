using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.TestUI
{
    public class Menu : MonoBehaviour
    {

        public GameObject[] MenuItems;
        void Start()
        {
            FindObjectOfType<SelectedObjManager>().OnSelectedObjectChanged += OnSelectableObjectChanged;
        }

        private void OnSelectableObjectChanged(GameObject selectedObject)
        {
            Debug.Log(selectedObject.name);
        }
        private void DisplayUIForObj(GameObject forObject)
        {

        }

    }
}

