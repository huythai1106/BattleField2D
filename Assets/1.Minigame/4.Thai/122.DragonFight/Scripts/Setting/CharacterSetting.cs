using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.DragonFight
{
    [CreateAssetMenu(menuName = "2Player/DragonFight/PlayerSetting")]
    public class CharacterSetting : ScriptableObject
    {
        // public NameCharacter nameCharacter;
        public string characterName;
        public float maxHealth;
        public float maxMana;
        public float startMana;
        public float attackDamage;
        public float critRate;
        public float scaleDamageSkillOne;
        public float scaleDamageSkillTwo;
        public Sprite avatar;
    }
}
