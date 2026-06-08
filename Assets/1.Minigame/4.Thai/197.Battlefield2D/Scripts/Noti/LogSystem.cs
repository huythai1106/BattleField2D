using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class LogSystem : MonoBehaviour
    {
        public static LogSystem Instance { get; private set; }
        public List<KillNotifcationUI> notificationUIs = new List<KillNotifcationUI>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void AddNotification(KillInfo killInfo)
        {
            for (int i = 0; i < notificationUIs.Count; i++)
            {
                notificationUIs[i].AddNotification(killInfo);
            }
        }
    }
}
