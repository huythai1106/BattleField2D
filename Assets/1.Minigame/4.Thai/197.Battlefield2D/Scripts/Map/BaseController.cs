using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class BaseController : MonoBehaviour
    {
        public Sprite[] flags;
        public List<Base> baseRed;
        public List<Base> baseBlue;
        public List<Base> baseNautral;

        public List<Base> targetRed = new();
        public List<Base> targetBlue = new();
        public Action<Base> onChangeBase;

        private void Start()
        {
            Setup();
            SetupTarget();
        }

        private void Setup()
        {
            for (int i = 0; i < baseRed.Count; i++)
            {
                baseRed[i].baseController = this;
                baseRed[i].team = GameManager.Instance.teamRed;
                baseRed[i].SetFlag();
            }

            for (int i = 0; i < baseBlue.Count; i++)
            {
                baseBlue[i].baseController = this;
                baseBlue[i].team = GameManager.Instance.teamBlue;
                baseBlue[i].SetFlag();
            }

            for (int i = 0; i < baseNautral.Count; i++)
            {
                baseNautral[i].baseController = this;
                baseNautral[i].team = null;
                baseNautral[i].SetFlag();
            }
        }

        public void OnBaseChange(Base b)
        {
            onChangeBase?.Invoke(b);

            if (baseBlue.Count == 0 || baseRed.Count == 0)
            {
                int value = baseRed.Count == 0 ? 0 : 1;
                GameManager.Instance.FinishGame(value);
            }
        }

        public void SetupTarget()
        {
            if (GameManager.Instance.currentMap.typeMap == TypeMap.Defend)
            {
                foreach (var item in baseRed)
                {
                    targetBlue.Add(item);
                }
                return;
            }

            targetRed.Clear();
            targetBlue.Clear();

            for (int i = 0; i < baseRed.Count; i++)
            {
                for (int j = 0; j < baseRed[i].baseConnect.Length; j++)
                {
                    if (baseRed[i].baseConnect[j].team != baseRed[i].team && baseRed[i].baseConnect[j].gameObject.activeSelf)
                    {
                        if (!targetRed.Contains(baseRed[i].baseConnect[j]))
                            targetRed.Add(baseRed[i].baseConnect[j]);
                    }
                }
            }

            for (int i = 0; i < baseBlue.Count; i++)
            {
                for (int j = 0; j < baseBlue[i].baseConnect.Length; j++)
                {
                    if (baseBlue[i].baseConnect[j].team != baseBlue[i].team && baseBlue[i].baseConnect[j].gameObject.activeSelf)
                    {
                        if (!targetBlue.Contains(baseBlue[i].baseConnect[j]))
                            targetBlue.Add(baseBlue[i].baseConnect[j]);
                    }
                }
            }
        }
    }
}
