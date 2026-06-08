using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minigame.Battlefield
{
    public class ModCtr : MonoBehaviour, IPointerClickHandler
    {
        internal PopupController popupController;
        internal Image image;

        public int modIndex;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            popupController.OnPick(this);
        }

        public void OnPick()
        {
            popupController.hightlightPopup.gameObject.SetActive(true);
            popupController.hightlightPopup.position = transform.position;

            popupController.modIndex = modIndex;
            image.color = popupController.chooseColor;
        }

        public void OnUnpick()
        {
            image.color = popupController.disbaleColor;
        }
    }
}
