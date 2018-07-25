using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.TestUI
{
    public class Menu : MonoBehaviour
    {

        public List<MenuItm> MenuItems;
        void Start()
        {
            FindObjectOfType<SelectedObjManager>().OnSelectedObjectChanged += OnSelectableObjectChanged;
        }

        private void OnSelectableObjectChanged(GameObject selectedObject)
        {
            if (true)
            {

            }
            DisplayUIForObj(selectedObject);
        }
        private void DisplayUIForObj(GameObject forObject)
        {
            SelectebleObj obj = forObject.GetComponent<SelectebleObj>();
            MenuItm menuPrefab = null;
            foreach (var item in MenuItems)
            {               
                if (item.SelectedObjType == obj.SelectedObjType)
                {
                    menuPrefab = item;
                }
            }
            var newMenu = Instantiate(menuPrefab, forObject.transform.position, Quaternion.identity);
            newMenu.transform.Translate(new Vector3(0, 3, 0));

        }

    }
}

