using System;
using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Minigame.Battlefield
{
    public enum CharacterState
    {
        Idle,
        Running,
    }
    [System.Serializable]
    public class StateModule : BaseModule
    {
        public SkeletonAnimation anim;
        private CharacterState currentState = CharacterState.Idle;
        public EventData eventAttack;

        public StateModule(BaseCharacter character) : base(character)
        {
        }

        protected override void Initialize()
        {
            anim = character.anim;
            UpdateAnimation();

            eventAttack = anim.skeleton.Data.FindEvent("Shoot");

            // anim.skeleton.SetAttachment("Weapon_B", "None");
        }

        public void UpdateAnimation()
        {
            switch (currentState)
            {
                case CharacterState.Idle:
                    anim.state.SetAnimation(0, $"Idle_Weapon_0", true);
                    break;
                case CharacterState.Running:
                    anim.state.SetAnimation(0, $"Walk_Weapon_0", true);
                    break;
                default:
                    break;
            }
        }

        public void SetStatePlayer(CharacterState name)
        {
            if (currentState == name)
            {
                return;
            }
            else
            {
                currentState = name;
                UpdateAnimation();
            }
        }

        private Coroutine coroutineSetEmpty;

        private IEnumerator SetEmpty()
        {
            yield return new WaitForSeconds(1f);
            anim.state.SetEmptyAnimation(1, 0);
        }

        public void PlayAnimShoot(string name, Action callback = null, Spine.AnimationState.TrackEntryEventDelegate handleEvent = null)
        {
            if (handleEvent != null)
            {
                anim.AnimationState.Event += handleEvent;
            }

            if (coroutineSetEmpty != null)
            {
                GameManager.Instance.StopCoroutine(coroutineSetEmpty);
            }

            anim.state.SetAnimation(1, name, false).Complete += (e) =>
            {
                anim.AnimationState.Event -= handleEvent;

                callback?.Invoke();
                coroutineSetEmpty = GameManager.Instance.StartCoroutine(SetEmpty());
            };
        }

        public void ResetAnim(int trackIndex)
        {
            anim.state.SetEmptyAnimation(trackIndex, 0);
        }
    }
}
