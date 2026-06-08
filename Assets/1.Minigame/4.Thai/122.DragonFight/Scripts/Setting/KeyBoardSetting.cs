using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.DragonFight
{
    [CreateAssetMenu(menuName = "2Player/DragonFight/KeyBoardSetting")]
    public class KeyBoardSetting : ScriptableObject
    {
        public KeyBoard player1;
        public KeyBoard player2;

        [System.Serializable]
        public class KeyBoard
        {
            public KeyCode left, right, up, down, attack, skillOne, skillTwo, block, chaos;

            public KeyCode this[int i]
            {
                get
                {
                    return new KeyCode[] { left, right, up, down, attack, skillOne, skillTwo, block, chaos }[i];
                }
            }
        }
    }
}
