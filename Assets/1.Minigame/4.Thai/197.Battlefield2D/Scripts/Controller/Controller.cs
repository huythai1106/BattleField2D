using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class Controller : MonoBehaviour
    {
        public Team team;
        public BaseCharacter character;
        public HealthBarUI healthBarUI;

        protected virtual void Start()
        {
        }

        private void Update()
        {
            if (!character || character.isDead || GameManager.Instance.isFinishGame || !GameManager.Instance.isStartGame) return;

            Controll();
        }

        public virtual void SetCharacter(BaseCharacter c)
        {
            if (GameManager.Instance.isFinishGame) return;

            team.menberAvailable.Remove(c);
            team.memberInGame.Add(c);

            character = c;
            character.transform.SetParent(transform);
            character.controller = this;

            character.Spawn();
            OnSpawnCharacter(character);

            healthBarUI.RegisterFunc(c);
            healthBarUI.transform.SetParent(c.transform);
            healthBarUI.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public virtual void OnCharacterDead()
        {
            character = null;
            StopAllCoroutines();
            DOTween.Kill(transform);
        }

        public void OnUseSkill()
        {
            if (character && character.isCanUseSkill)
            {
                character.OnUseSkill();
            }
        }

        protected virtual void Controll()
        {

        }

        protected virtual void OnSpawnCharacter(BaseCharacter c)
        {

        }
    }
}
