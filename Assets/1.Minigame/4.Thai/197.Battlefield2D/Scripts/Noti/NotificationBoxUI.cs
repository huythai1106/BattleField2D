using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.Battlefield
{
    public class NotificationBoxUI : MonoBehaviour
    {
        private KillNotifcationUI parent;
        public TextMeshProUGUI killer;
        public TextMeshProUGUI killed;
        public Image weapon;

        void OnDestroy()
        {
            CancelInvoke();
        }

        public void Initialize(KillInfo killInfo, KillNotifcationUI parent)
        {
            killer.text = killInfo.killer.controller.name;
            killed.text = killInfo.killedPerson.controller.name;
            weapon.sprite = killInfo.weapon.setting.imgWhite;
            weapon.transform.localScale = Vector3.one * killInfo.weapon.setting.scaleOnNotification;
            weapon.SetNativeSize();

            this.parent = parent;

            Invoke(nameof(DestroyBox), 3f);
        }

        public void DestroyBox()
        {
            parent.notificationBoxes.Remove(this);
            Destroy(gameObject);
        }
    }
}
