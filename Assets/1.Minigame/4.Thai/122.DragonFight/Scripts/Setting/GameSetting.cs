using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.DragonFight
{
    [CreateAssetMenu(menuName = "2Player/DragonFight/GameSetting")]
    public class GameSetting : ScriptableObject
    {
        public float maxSpeed;
        public float force;
        public float TimeCountDownSkillOne;
        public float TimeCountDownSkillTwo;
        public float manaUseSkillOne;
        public float manaUseSkillTwo;
        public float manaUseChaos;
        public float manaGainWhenAttack;
        public float manaGainWhenTakeDamage;
    }
}
