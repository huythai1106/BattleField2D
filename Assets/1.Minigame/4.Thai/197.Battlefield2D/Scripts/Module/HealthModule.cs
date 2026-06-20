using System.Collections;
using System.Collections.Generic;
using Minigame.MoneyHeist;
using Thai.Lib;
using UnityEngine;

namespace Minigame.Battlefield
{
    [System.Serializable]
    public class HealthModule : BaseModule
    {
        public HealthModule(BaseCharacter character) : base(character)
        {
        }

        public void TakenDamage(SourceDamage source)
        {
            if (character.isDead) return;

            var damageValue = source.GetDamage();

            var armor = character.properties.armor.Value;

            character.properties.armor.Value = Mathf.Max(character.properties.armor.Value - damageValue * 0.66f, 0);
            character.properties.health.Value = Mathf.Max(character.properties.health.Value - (damageValue - (armor - character.properties.armor.Value)), 0);

            character.PlayAnimHit();

            if (character.properties.health.Value <= 0)
            {
                KillInfo killInfo = new KillInfo()
                {
                    killer = source.sourceCharacter,
                    killedPerson = character,
                    weapon = ItemManager.Instance.itemDict[source.weaponName],
                };

                LogSystem.Instance.AddNotification(killInfo);

                CharacterDie();

                if (source.sourceCharacter)
                {
                    source.sourceCharacter.team.TotalKill++;
                }
            }
        }

        public void Health(float value)
        {
            character.properties.health.Value = Mathf.Min(character.properties.health.Value + value, character.characterSetting.maxHealth);

            // EffectManager.Instance.CreatedEffect("healing", character.transform);
        }

        public void CharacterDie()
        {
            character.ClearOnDeath();
            // GameManager.Instance.CreateBlood(character.transform);

            if (character.controller is HumanController)
            {
                SoundManager.instance.PlaySoundEffectList("die");
            }

            character.team.BackToAvailable(character);
            character.team.SpawnCharacter(character.controller);

            character.controller.OnCharacterDead();
            character.isDead = true;
        }
    }
}
