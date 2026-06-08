using UnityEngine;

namespace Minigame.Battlefield
{
    [CreateAssetMenu(menuName = "2Player/Battlefield/CharacterSetting")]
    public class CharacterSetting : ScriptableObject
    {
        public float maxHealth;
        public float speed;
        public float armor;
    }
}
