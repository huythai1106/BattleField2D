using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class KillNotifcationUI : MonoBehaviour
    {
        public int maxChildren = 4;

        public NotificationBoxUI boxPrefab;
        public List<NotificationBoxUI> notificationBoxes = new();

        private void Start()
        {
            LogSystem.Instance.notificationUIs.Add(this);
        }

        public void AddNotification(KillInfo killInfo)
        {
            var box = Instantiate(boxPrefab, transform);
            box.Initialize(killInfo, this);

            notificationBoxes.Add(box);

            if (notificationBoxes.Count > maxChildren)
            {
                Destroy(notificationBoxes[0].gameObject);
                notificationBoxes.RemoveAt(0);
            }
        }
    }
}
