using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.TestUI
{
    [RequireComponent(typeof(Collider))]
    public class SelectebleObj : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            FindObjectOfType<SelectedObjManager>().SelectedObj = gameObject;
        }
    }
}

