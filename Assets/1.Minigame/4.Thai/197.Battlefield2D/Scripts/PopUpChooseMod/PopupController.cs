using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class PopupController : MonoBehaviour
    {
        public Color chooseColor;
        public Color disbaleColor;
        public int modIndex = -1;
        public RectTransform hightlightPopup;

        public ModCtr[] modCtrs;
        private readonly string keyModIndex = "Map_Index";

        private void Start()
        {
            // if (GameData.playFromMenu)
            // {
            //     GameData.playFromMenu = false;

            // }
            // else
            // {
            //     modIndex = PlayerPrefs.GetInt(keyModIndex);
            //     OnPlay();
            // }
            Screen.orientation = ScreenOrientation.Portrait;
            PlayerPrefs.SetInt(keyModIndex, -1);
            Setup();

            // GameManager.Instance.extiBtn.GetComponent<CanvasGroup>().alpha = 0;
            // GameManager.Instance.extiBtn.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        private void Setup()
        {
            foreach (var item in modCtrs)
            {
                item.popupController = this;
            }
        }

        public void OnPick(ModCtr modCtr)
        {
            foreach (var item in modCtrs)
            {
                if (item == modCtr) continue;

                item.OnUnpick();
            }

            modCtr.OnPick();
        }

        public void OnPlay()
        {
            // if (modIndex == -1) return;

            GameManager.Instance.StartGame(modIndex);
            gameObject.SetActive(false);

            PlayerPrefs.SetInt(keyModIndex, modIndex);
        }
    }
}
